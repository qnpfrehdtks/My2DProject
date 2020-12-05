using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IPoolingObject
{
    public int StartCount { get; set; } = 1;
    protected Dictionary<Type, List<UnityEngine.Object>> m_dicUIObjects = new Dictionary<Type, List<UnityEngine.Object>>();
    private int m_OrderLayer;

    public virtual void Initialize(GameObject factory)
    {
        transform.SetParent(factory.transform);
    }
    public virtual void OnPushToQueue()
    {
        gameObject.SetActive(false);
    }
    public virtual void OnPopFromQueue()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// UI를 켤때 실행되는 함수.
    /// </summary>
    public virtual void OnShowUp()
    {
        Debug.Log(gameObject.name + "Show up");
    }

    public int OrderLayer
    {
        get
        {
            return m_OrderLayer;
        } 
    }

    private void Awake()
    {
        m_OrderLayer = GetComponent<Canvas>().sortingOrder;
    }

    protected T GetUI<T>(int idx) where T : UnityEngine.Object
    {
        List<UnityEngine.Object> list = null;
        if (m_dicUIObjects.TryGetValue(typeof(T), out list))
        {
            return list[idx] as T;
        }

        return null;
    }

    static public void AddUIEvent(GameObject gameObject, System.Action<PointerEventData> action, E_UIEVENT type)
    {
        UI_EventHandler ev = Common.GetOrAddComponent<UI_EventHandler>(gameObject);

        if (ev == null) return;

        switch (type)
        {
            case E_UIEVENT.CLICK:
                ev.ClickAction = action;
                break;
            case E_UIEVENT.DOWN:
                ev.DownAction = action;
                break;
            case E_UIEVENT.UP:
                ev.UpAction = action;
                break;
            case E_UIEVENT.BEGIN_DRAG:
                ev.BeginDragAction = action;
                break;
            case E_UIEVENT.DRAG:
                ev.OnDragAction = action;
                break;
            case E_UIEVENT.EXIT_DRAG:
                ev.ExitDragAction = action;
                break;

        }
    }

    protected void BindUI<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        foreach (string n in names)
        {
            if (!m_dicUIObjects.ContainsKey(typeof(T)))
            {
                List<UnityEngine.Object> list = new List<UnityEngine.Object>();

                T child = Common.FindChild<T>(gameObject, n);

                if (child)
                {
                    list.Add(child);
                }
                else
                {
                    GridLayoutGroup parent = Common.FindChild<GridLayoutGroup>(gameObject);

                    if (parent != null)
                    {
                        child = Common.FindChild<T>(parent.gameObject, n);

                        if (child)
                        {
                            list.Add(child);
                        }
                    }
                    else
                        Common.LogError(n + "해당 Child가 없습니다. child로 등록 바람.");
                }
                m_dicUIObjects.Add(typeof(T), list);
            }
            else
            {
                T child = Common.FindChild<T>(gameObject, n);
                List<UnityEngine.Object> list = m_dicUIObjects[typeof(T)];
                if (child)
                {
                    list.Add(child);
                }
                else
                {
                    GridLayoutGroup parent = Common.FindChild<GridLayoutGroup>(gameObject);

                    if(parent != null)
                    {
                         child  = Common.FindChild<T>(parent.gameObject, n);

                        if (child)
                        {
                            list.Add(child);
                        }
                    }
                    else
                        Common.LogError(n + "해당 Child가 없습니다. child로 등록 바람.");
                }
            }
        }
    }



    public Button GetButton(int idx) { return GetUI<Button>(idx); }
    public Image GetImage(int idx) { return GetUI<Image>(idx); }
    public TMPro.TextMeshProUGUI GetText(int idx) { return GetUI<TMPro.TextMeshProUGUI>(idx); }
    public TMPro.TMP_InputField GetInputField(int idx) { return GetUI<TMPro.TMP_InputField>(idx); }

}
