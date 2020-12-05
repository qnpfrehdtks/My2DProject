using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    enum Text
    {
        GOLD_TEXT
    }

    enum GoldImage
    {
        GOLD_IMAGE
    }

    public int m_currentLayerOrder { private set; get; } = 0;

    Stack<UI_Popup> m_StackUI = new Stack<UI_Popup>();

    public UI_Base m_CurrentSceneUI;

    E_SCENE_UI_TYPE m_sceneType = E_SCENE_UI_TYPE.NONE;
    GameObject m_GoldPrefab;
    GameObject m_NumberCntPrefab;

    // Stack 쎃을때 카운트 UI
    UI_HUD m_currentStackCntHUD;
    Coroutine m_stackCntTimer_C;

    public GameObject m_Target { get; private set; }

    public override void InitializeManager()
    {
        m_GoldPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/GoldImage");
        PoolingManager.Instance.CreatePool(m_GoldPrefab.gameObject);

        m_NumberCntPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/HUD");
        PoolingManager.Instance.CreatePool(m_NumberCntPrefab.gameObject);

        m_currentStackCntHUD = PoolingManager.Instance.PopFromPool(m_NumberCntPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<UI_HUD>();
        m_currentStackCntHUD.gameObject.SetActive(false);
    }

    int m_currentStackCnt = 0;

    public void ShowStackHUD()
    {
        if (m_CurrentSceneUI == null)
            return;

        int stackCount = CharacterManager.Instance.m_CurrentMyCharacter.StackCount;
        Vector3 pos = Vector3.zero;

        stackCount = Mathf.Clamp(stackCount, 0, 10);
        pos = Camera.main.WorldToScreenPoint(CharacterManager.Instance.m_CurrentMyCharacter.transform.position + Vector3.up * 1.0f + stackCount * Vector3.up * 0.1f);

        m_currentStackCntHUD.transform.position = pos;
        m_currentStackCntHUD.transform.SetParent(m_CurrentSceneUI.transform);
        m_currentStackCntHUD.gameObject.SetActive(true);
        m_currentStackCntHUD.SetText("+" + (++m_currentStackCnt).ToString());

        if(m_stackCntTimer_C != null)
        {
            StopCoroutine(m_stackCntTimer_C);
            m_stackCntTimer_C = null;
        }

        m_stackCntTimer_C = StartCoroutine(StartStackCnt_C(1.0f));

    }

    IEnumerator StartStackCnt_C(float time)
    {
        yield return new WaitForSeconds(time);
        m_currentStackCnt = 0;
    }

    public void OnVictory(int Score)
    {
        if(m_CurrentSceneUI != null)
            m_Target = m_CurrentSceneUI.GetImage((int)GoldImage.GOLD_IMAGE).gameObject;

        if (Score > 0)
        {
            int coinCount = Score / GameMaster.ONE_GOLD_VALUE;
            for (int i = 0; i < coinCount; i++)
            {
                float w = Random.Range(-0.2f, 0.2f) * (float)Screen.currentResolution.width;
                float h = Random.Range(-0.2f, 0.2f) * (float)Screen.currentResolution.height;
                Vector3 pos = Camera.main.WorldToScreenPoint(CharacterManager.Instance.m_CurrentMyCharacter.transform.position) + new Vector3(w, h, 0);

                
                GameObject co = PoolingManager.Instance.PopFromPool(m_GoldPrefab.gameObject, pos, Quaternion.identity);
                co.transform.SetParent(m_CurrentSceneUI.transform);
                co.transform.position = pos;
            }
        }
    }

    public void OnChangeGoldUI(int earnedGold)
    {
        if (m_CurrentSceneUI == null)
            return;

        var GoldText = m_CurrentSceneUI.GetText((int)Text.GOLD_TEXT);

        if (GoldText != null)
        {
            int gold = int.Parse(GoldText.text);
            gold += earnedGold;
            GoldText.text = gold.ToString();
        }
    }

    public void CloseSceneUI() 
    {
       if(m_CurrentSceneUI != null)
       {
           ResourceManager.Instance.DestroyObject(m_CurrentSceneUI.gameObject);
       }

        m_Target = null;
        m_CurrentSceneUI = null;
    }

    public void ShowUIScene(E_SCENE_UI_TYPE type)
    {
        if(m_CurrentSceneUI != null && m_sceneType == type)
        {
            return;
        }
        m_sceneType = type;
        CloseSceneUI();
        GameObject go = ResourceManager.Instance.instantiate<GameObject>("Prefabs/UI/Scene/" + type.ToString());
        m_CurrentSceneUI = go.GetComponentInChildren<UI_Base>();
        m_Target = null;

        if (m_CurrentSceneUI != null)
        {
            m_CurrentSceneUI.OnShowUp();
        }

    }

    public T ShowPopUp<T>(string name) where T : UI_Popup
    {
        GameObject popUpgo = ResourceManager.Instance.instantiate<GameObject>("Prefabs/UI/Popup/" + name);

        if(popUpgo == null)
        {
            return null;
        }

        T popup = Common.GetOrAddComponent<T>(popUpgo);

        Canvas canvas = Common.GetOrAddComponent<Canvas>(popUpgo);
        canvas.sortingOrder = ++m_currentLayerOrder;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        m_StackUI.Push(popup);
        return popup;
    }

    public void ClosePopUp()
    {
        if (m_StackUI.Count == 0)
            return;

        UI_Popup popUp = m_StackUI.Pop();
        ResourceManager.Instance.DestroyObject(popUp.gameObject);

        if (m_StackUI.Count == 0)
        {
            m_currentLayerOrder = 0;
        }
        else
        {
            UI_Popup peekUI = m_StackUI.Peek();
            Canvas canvas = Common.GetOrAddComponent<Canvas>(peekUI.gameObject);

            m_currentLayerOrder = canvas.sortingOrder;
        }
    }
}
