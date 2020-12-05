using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CostumeSetItemtable
{
    public List<CostumeSetInfo> arrItem = new List<CostumeSetInfo>();
}

[System.Serializable]
public class CostumeSetInfo : ISerializationCallbackReceiver
{
    public int CostumeSetID;
    public Dictionary<E_COSTUME_TYPE, int> dic = new Dictionary<E_COSTUME_TYPE, int>();

    [SerializeField]
    List<E_COSTUME_TYPE> keys;
    [SerializeField]
    List<int> values;


    public CostumeSetInfo(int CostumeSetID, int Chest, int Bottom, int Costume, int hair, int foot, int hand = 2001)
    {
        this.CostumeSetID = CostumeSetID;
    }

    public Dictionary<E_COSTUME_TYPE, int> ToDictionary() { return dic; }

    public void OnBeforeSerialize()
    {
        keys = new List<E_COSTUME_TYPE>(dic.Keys);
        values = new List<int>(dic.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        dic = new Dictionary<E_COSTUME_TYPE, int>(count);
        for (var i = 0; i < count; ++i)
        {
            dic.Add(keys[i], values[i]);
        }
    }


    public void insertItem(E_COSTUME_TYPE type, int info)
    {
        dic.Add(type, info);
    }

}

[System.Serializable]
public class ItemInfoTable
{
    public int ID = 2002;
    public List<ItemInfo> arrItem;


    public ItemInfoTable()
    {
        arrItem = new List<ItemInfo>();
    }

    public void insertItem(ItemInfo info)
    {
        arrItem.Add(info);
    }

    public void clearItemInfo()
    {
        arrItem.Clear();
    }

    public void Print()
    {

        for (int idx = 0; idx < arrItem.Count; idx++)
        {
            Debug.Log(string.Format("arrItem[{0}] = {1}", idx, arrItem[idx]));
        }
    }
}

[System.Serializable]
public class CharacterStat
{
    float RunSpeed;
    float JumpSpeed;
}



[System.Serializable]
public class ItemStat
{
    public int ItemID;
    public float Speed;
    public float JumpSpeed;
    public float CoinRatio;
    public float Power;
    public int StartWood;

    public ItemStat(int itemID, float speed, float jumpSpeed, float coinRatio, float power, int wood)
    {
        ItemID = itemID;
        Speed = speed;
        JumpSpeed = jumpSpeed;
        CoinRatio = coinRatio;
        Power = power;
        StartWood = wood;
    }
}

[System.Serializable]
public class PlayerBuyItemTable : ISerializationCallbackReceiver
{
    public List<int> keys;
    public List<bool> values;
    public Dictionary<int, bool> dic = new Dictionary<int, bool>();

    public void OnBeforeSerialize()
    {
        keys = new List<int>(dic.Keys);
        values = new List<bool>(dic.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        dic = new Dictionary<int, bool>(count);
        for (var i = 0; i < count; ++i)
        {
            dic.Add(keys[i], values[i]);
        }
    }

    public void insertItem(int type, bool info)
    {
        dic.Add(type, info);
    }

    public bool getBuyItem(int type)
    {
        bool value = false;
        dic.TryGetValue(type, out value);
        return value;
    }
}

[System.Serializable]
public class ItemInfo
{
    public int ItemID;
    public string ItemName;
    public E_COSTUME_TYPE cosutumeType;
    public string Info;
    public string itemResourcePath;
    public E_AVATA_TYPE avataType;
    public int materialID;
    public E_ITEMRANK itemRank;
    public string itemIconResourcePath;
    public int Price;

    public ItemInfo(int itemID, string itemName, E_COSTUME_TYPE type, string info, string resourcePath, E_AVATA_TYPE avataType, int materialID, E_ITEMRANK itemRank = E_ITEMRANK.NORMAL, int price = 1500)
    {
        ItemID = itemID;
        cosutumeType = type;
        Info = info;
        ItemName = itemName;
        itemResourcePath = resourcePath;
        this.avataType = avataType;
        this.materialID = materialID;
        this.itemRank = itemRank;
        Price = price;
    }
}
