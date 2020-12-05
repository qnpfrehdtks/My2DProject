using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Dead : Ui_Scene
{
    enum Buttons
    {
        RETRY
    }

    protected void Start()
    {
        BindUI<Button>(typeof(Buttons));
        AddUIEvent(GetButton((int)Buttons.RETRY).gameObject, DownPlayBtn, E_UIEVENT.DOWN);
    }

    void DownPlayBtn(PointerEventData data)
    {
        GameMaster.Instance.ChangeToTitle();
    }
}
