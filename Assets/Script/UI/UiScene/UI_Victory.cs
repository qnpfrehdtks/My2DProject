using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Victory : Ui_Scene
{
    enum Buttons
    {
        RETRY
    }

    protected void Start()
    {
        //  BindUI<TMPro.TMP_InputField>(typeof(InputFields));
        BindUI<Button>(typeof(Buttons));
       // BindUI<TMPro.TextMeshProUGUI>(typeof(Texts));

        AddUIEvent(GetButton((int)Buttons.RETRY).gameObject, DownPlayBtn, E_UIEVENT.DOWN);
    }

    void DownPlayBtn(PointerEventData data)
    {
        GameMaster.Instance.ChangeToTitle();
    }
}
