using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMain : SceneMain
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnInitializeScene()
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.SHOP);
    }

    public override void ExitSceneInit()
    {
        base.ExitSceneInit();
    }
}
