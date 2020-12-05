using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : Singleton<GameMaster>
{
    public const int ONE_GOLD_VALUE = 20; 

    int m_Gold;
    List<ObjectRespawner> m_ListWoodRespawner = new List<ObjectRespawner>();

    public int Gold
    {
        get
        {
            return m_Gold;
        }
        set
        {
            if(UIManager.Instance.m_CurrentSceneUI != null)
            {
                UIManager.Instance.OnChangeGoldUI(value - m_Gold);
            }
            
            if(value < 0)
            {
                value = 0;
            }

            m_Gold = value;
            PlayerPrefs.SetInt("myGold", m_Gold);
        }
    }

    public System.Action<int> OnGoldChangeAction = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitializeManager()
    {
        InitGold();


    }

    /// <summary>
    /// 전에 있던 리스폰 오브젝트를 리셋 해주고, 현재 맵에서 리스폰 오브젝트를 찾고 초기화 시켜준다.
    /// </summary>
    public void InitializeRespawnObject()
    {
        GameObject wood = ResourceManager.Instance.Load<GameObject>("Prefabs/Wood");
        PoolingManager.Instance.PushAllObjectToPool(wood);

        for (int i = 0; i < m_ListWoodRespawner.Count; i++) {
            m_ListWoodRespawner[i].AllClearObject();
        }

        m_ListWoodRespawner.Clear();

        for (int i = 0; i < 8; i++) { 
            GameObject go = GameObject.Find("WoodPosition_" + i);
            if (go == null) continue;
            ObjectRespawner res = Common.GetOrAddComponent<ObjectRespawner>(go);
            if (res == null) continue;
            m_ListWoodRespawner.Add(res);
        }
        
        for (int i = 0; i < m_ListWoodRespawner.Count; i++){
            m_ListWoodRespawner[i].Init();
        }
    }

    public void ChangeToTitle()
    {
        SoundManager.Instance.PlayBGM(E_BGM.TITLE);
        CharacterManager.Instance.AllPushOtherCharacter();

        for (int i = 0; i < 6; i++) {
            GameObject go = GameObject.Find("StartPosition_" + i);

            if (go != null)
            {
                // 0은 내 캐릭터 자리~
                if (i == 0)
                {
                    CharacterManager.Instance.CreateMyCharacter(go.transform.position, go.transform.rotation);
                }
                else
                {
                    CharacterManager.Instance.CreateOtherCharacter(go.transform.position, go.transform.rotation);
                }
            }
        }

        InitializeRespawnObject();

        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.TITLE);
    }

    public void InitGold()
    {
        if (PlayerPrefs.HasKey("myGold"))
        {
            m_Gold = PlayerPrefs.GetInt("myGold");
        }
        else
        {
            m_Gold = 0;
            PlayerPrefs.SetInt("myGold", m_Gold);
        }
    }

    public void OnGameStart()
    {
        CharacterManager.Instance.OnStartGame();
    }

    public void OnGameGoal()
    {
        CameraManager.Instance.SetTarget(null);
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.VICTORY);
        UIManager.Instance.OnVictory(1000);

        PlayerPrefs.SetInt("myGold", Gold + 1000);
        m_Gold += 1000;
    }

}
