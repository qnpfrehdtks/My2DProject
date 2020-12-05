using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Shop : Ui_Scene
{
    enum Buttons
    {
        NORMAL,
        UNIQUE,
        LEGEND,
        OK,
        BUY,
        END
    }


    enum ShopButtons
    {
        COSTUME1,
        COSTUME2,
        COSTUME3,
        COSTUME4,
        COSTUME5,
        COSTUME6,
        COSTUME7,
        COSTUME8,
        COSTUME9,
        END
    }

    E_ITEMRANK m_CurrentPressedItemRank = E_ITEMRANK.END;
    Button m_BuyButton;
    TMPro.TextMeshProUGUI m_BuyBtnText;

    [SerializeField]
    List<int> m_ListCostume = new List<int>();

    [SerializeField]
    List<int> m_ListUniqueCostume = new List<int>();

    [SerializeField]
    List<int> m_ListLegendCostume = new List<int>();

    enum InputFields
    {
        PLAYER_INPUT
    }

    public override void OnShowUp()
    {
        base.OnShowUp();
        CharacterManager.Instance.m_CurrentMyCharacter.NickName = null;

        CharacterManager.Instance.SetShowOtherCharacter(false);
        CharacterManager.Instance.m_CurrentMyCharacter.gameObject.SetActive(true);

        CameraManager.Instance.SetCameraMode(E_CAMERA_TYPE.SHOP_VIEW);
        CameraManager.Instance.SetDistance(-3.52f, 0.82f);
    }

    protected void Start()
    {
        BindUI<Button>(typeof(Buttons));
        BindUI<ShopButton>(typeof(ShopButtons));

        for (ShopButtons t = ShopButtons.COSTUME1; t < ShopButtons.END; t++)
        {
            GameObject btn = GetUI<ShopButton>((int)t).gameObject;
            AddUIEvent(
            btn, 
            (PointerEventData data) =>
            {
                ShopButton shopbtn = btn.GetComponentInChildren<ShopButton>();

                if (shopbtn == null)
                    return;

                if (shopbtn.IsBuy == false)
                    return;

                ItemInfo info = shopbtn.ItemInfo;

                if (info != null)
                {
                    if (CharacterManager.Instance.m_CurrentWearCostume == info.ItemID)
                        return;

                    CharacterManager.Instance.SetCostumeMyCharacter(info.ItemID);
                }
            }, 
            E_UIEVENT.DOWN);
        }

        AddUIEvent(GetButton((int)Buttons.OK).gameObject, PressOKButton, E_UIEVENT.CLICK);

        AddUIEvent(GetButton((int)Buttons.NORMAL).gameObject, PressNormalTabButton, E_UIEVENT.DOWN);
        AddUIEvent(GetButton((int)Buttons.UNIQUE).gameObject, PressUniqueTabButton, E_UIEVENT.DOWN);
        AddUIEvent(GetButton((int)Buttons.LEGEND).gameObject, PressLegendTabButton, E_UIEVENT.DOWN);

        m_BuyButton = GetButton((int)Buttons.BUY);
        AddUIEvent(m_BuyButton.gameObject, PressBuyBtn, E_UIEVENT.DOWN);

        m_BuyBtnText = Common.FindChild<TMPro.TextMeshProUGUI>(m_BuyButton.gameObject);

        PressNormalTabButton(null);
    }

    void PressNormalTabButton(PointerEventData data)
    {
        GetButton((int)(Buttons.UNIQUE)).image.color = Color.white;
        GetButton((int)(Buttons.LEGEND)).image.color = Color.white;
        GetButton((int)(Buttons.NORMAL)).image.color = Color.grey;

        ShopButton shopButton;

        for (ShopButtons t = ShopButtons.COSTUME1; t < ShopButtons.END; t++)
        {
            shopButton = GetUI<ShopButton>((int)t);

            shopButton.ItemInfo = CostumeDataManager.Instance.GetItemInfo(m_ListCostume[(int)t]);
            shopButton.IsBuy = CostumeDataManager.Instance.GetBuyItem(m_ListCostume[(int)t]);
        }

        m_CurrentPressedItemRank = E_ITEMRANK.NORMAL;
        m_BuyBtnText.text = $"Buy {CostumeDataManager.Instance.GetCostumeCost(m_CurrentPressedItemRank)}";
    }

    void PressUniqueTabButton(PointerEventData data)
    {
        GetButton((int)(Buttons.UNIQUE)).image.color = Color.grey;
        GetButton((int)(Buttons.LEGEND)).image.color = Color.white;
        GetButton((int)(Buttons.NORMAL)).image.color = Color.white;

        ShopButton shopButton;

        for (ShopButtons t = ShopButtons.COSTUME1; t < ShopButtons.END; t++)
        {
            shopButton = GetUI<ShopButton>((int)t);

            shopButton.ItemInfo = CostumeDataManager.Instance.GetItemInfo(m_ListUniqueCostume[(int)t]);
            shopButton.IsBuy = CostumeDataManager.Instance.GetBuyItem(m_ListUniqueCostume[(int)t]);
        }

        m_CurrentPressedItemRank = E_ITEMRANK.RARE;
        m_BuyBtnText.text = $"Buy {CostumeDataManager.Instance.GetCostumeCost(m_CurrentPressedItemRank)}";
    }

    void PressLegendTabButton(PointerEventData data)
    {
        GetButton((int)(Buttons.UNIQUE)).image.color = Color.white;
        GetButton((int)(Buttons.LEGEND)).image.color = Color.grey;
        GetButton((int)(Buttons.NORMAL)).image.color = Color.white;

        ShopButton shopButton;

        for (ShopButtons t = ShopButtons.COSTUME1; t < ShopButtons.END; t++)
        {
            shopButton = GetUI<ShopButton>((int)t);

            shopButton.ItemInfo = CostumeDataManager.Instance.GetItemInfo(m_ListLegendCostume[(int)t]);
            shopButton.IsBuy = CostumeDataManager.Instance.GetBuyItem(m_ListLegendCostume[(int)t]);
        }

        m_CurrentPressedItemRank = E_ITEMRANK.LEGEND;
        m_BuyBtnText.text = $"Buy {CostumeDataManager.Instance.GetCostumeCost(m_CurrentPressedItemRank)}";
    }

    void PressBuyBtn(PointerEventData data)
    {
        int price = CostumeDataManager.Instance.GetCostumeCost(m_CurrentPressedItemRank);

        if ( GameMaster.Instance.Gold < price)
        {
            return;
        }

        List<ShopButtons> arrRandom = new List<ShopButtons>();

        for (ShopButtons t = ShopButtons.COSTUME1; t < ShopButtons.END; t++)
        {
            if(!GetUI<ShopButton>((int)t).IsBuy)
            {
                arrRandom.Add(t);
            }
        }

        if(arrRandom.Count == 0)
        {
            return;
        }

       int randomCount = Random.Range(0, arrRandom.Count);

        GameMaster.Instance.Gold -= price;
        GetUI<ShopButton>((int)arrRandom[randomCount]).IsBuy = true;
        CostumeDataManager.Instance.SaveBuyItem(GetUI<ShopButton>((int)arrRandom[randomCount]).ItemInfo.ItemID , true);
    }

    void PressOKButton(PointerEventData data)
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.TITLE);
    }

}
