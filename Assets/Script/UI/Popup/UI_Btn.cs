using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Btn : UI_Popup
{
    enum Buttons
    {
        OK,
        CANCLE
    }


    private void Start()
    {
        BindUI<Button>(typeof(Buttons));

        AddUIEvent(GetButton((int)Buttons.OK).gameObject, ClickBtn, E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)Buttons.OK).gameObject, UpBtn, E_UIEVENT.UP);
        AddUIEvent(GetButton((int)Buttons.OK).gameObject, DownBtn, E_UIEVENT.DOWN);

        AddUIEvent(GetButton((int)Buttons.CANCLE).gameObject, ClickBtn, E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)Buttons.CANCLE).gameObject, UpBtn, E_UIEVENT.UP);
        AddUIEvent(GetButton((int)Buttons.CANCLE).gameObject, DownBtn, E_UIEVENT.DOWN);
    }

    void ClickBtn(PointerEventData data)
    {
        Debug.Log("Click Test");
    }
    void UpBtn(PointerEventData data)
    {

        Debug.Log("Up Test");
    }
    void DownBtn(PointerEventData data)
    {

        Debug.Log("Down Test");
    }




}
