using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Title : Ui_Scene
{
    enum Buttons
    {
        PLAY,
        OPTION,
        SHOP
    }

    enum InputFields
    {
        NAME_INPUT
    }
    protected void Start()
    {
        BindUI<TMPro.TMP_InputField>(typeof(InputFields));
        BindUI<Button>(typeof(Buttons));

        AddUIEvent(GetButton((int)Buttons.PLAY).gameObject, DownPlayBtn, E_UIEVENT.DOWN);

        AddUIEvent(GetButton((int)Buttons.OPTION).gameObject, ClickOptionBtn, E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)Buttons.SHOP).gameObject, ClickShopBtn, E_UIEVENT.CLICK);

        GetInputField((int)InputFields.NAME_INPUT).text = CharacterManager.Instance.m_CurrentMyCharacter.NickName;
        GetInputField((int)InputFields.NAME_INPUT).onEndEdit.AddListener(
            (string str)=>
            {
                CharacterManager.Instance.m_CurrentMyCharacter.NickName = (GetInputField((int)InputFields.NAME_INPUT).text);
            });
    }

    /// <summary>
    /// UI를 켤때 실행되는 함수.
    /// </summary>
    public override void OnShowUp()
    {
        base.OnShowUp();

        CharacterManager.Instance.SetShowOtherCharacter(true);

        if (CharacterManager.Instance.m_CurrentMyCharacter != null)
            CharacterManager.Instance.m_CurrentMyCharacter.m_BillBoard.gameObject.SetActive(true);

        CameraManager.Instance.SetCameraMode(E_CAMERA_TYPE.QUATER_VIEW);
        CameraManager.Instance.SetDistance(4.0f, 4.0f);
    }

    void DownPlayBtn(PointerEventData data)
    {
        UIManager.Instance.CloseSceneUI();
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.IN_GAME);

        CameraManager.Instance.SetCameraMode(E_CAMERA_TYPE.QUATER_VIEW);
        CameraManager.Instance.SetDistance(5f, 5f);
    }

    void ClickOptionBtn(PointerEventData data)
    {
    }

    void ClickShopBtn(PointerEventData data)
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.SHOP);
    }
}
