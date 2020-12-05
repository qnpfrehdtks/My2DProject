using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : Button
{
    ItemInfo m_itemInfo;
    bool m_isBuy;

    protected override void Awake()
    {
        base.Awake();
        ItemInfo = null;
        IsBuy = false;
    }

    public ItemInfo ItemInfo
    {
        get
        {
            return m_itemInfo;
        }
        set
        {
            m_itemInfo = value;
            InitItemInfo();
        }
    }

    public bool IsBuy
    {
        get
        {
            return m_isBuy;
        }
        set
        {
            m_isBuy = value;
            InitItemInfo();
        }
    }

    void InitItemInfo()
    {
        if (m_isBuy == false)
        {
            image.color = Color.white;
            image.sprite = ResourceManager.Instance.GetSprite(Common.RANDOM_AVATA_ICON);
            return;
        }

        image.color = Color.white;

        if (m_itemInfo == null)
        {
            image.sprite = ResourceManager.Instance.GetSprite(Common.RANDOM_AVATA_ICON);
        }
        else
        {
            image.sprite = ResourceManager.Instance.GetSprite(m_itemInfo.itemIconResourcePath);
        }
    }

}
