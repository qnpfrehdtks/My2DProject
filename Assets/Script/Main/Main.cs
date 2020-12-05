using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneMain : MonoBehaviour
{
    [SerializeField]
    protected E_SCENE_TYPE m_SceneType;


    protected bool m_isInit = false;


    virtual protected void Awake()
    {
        OnInitializeScene();
    }


    virtual public void ExitSceneInit()
    {
        UIManager.Instance.ClosePopUp();
        UIManager.Instance.CloseSceneUI();
    }

    public abstract void OnInitializeScene();

}

