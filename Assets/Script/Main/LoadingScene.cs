using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMain : SceneMain
{

    protected override void Awake()
    {
        base.Awake();
    }
    public override void OnInitializeScene()
    {
        SceneChanger.Instance.LoadReservedScene();
    }

}
