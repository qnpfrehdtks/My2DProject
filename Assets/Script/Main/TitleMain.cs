using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMain : SceneMain
{
    GameObject m_WoodPrefab;

    protected override void Awake()
    {


        base.Awake();
    }

    public override void OnInitializeScene()
    {
        SoundManager.Instance.PlayBGM(E_BGM.TITLE);

        EffectManager.Instance.InitializeManager();

        GameMaster.Instance.ChangeToTitle();
        
    }

}
