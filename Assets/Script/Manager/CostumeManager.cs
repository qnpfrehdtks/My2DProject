
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 그냥 아바타 정보만 담아두는 매니저. 
// 아바타 정보를 전역적으로 가저올때 쓰도록 함.
// 캐릭터의 아바타 파츠를 바꾸고 싶다면 CharacterManager를 쓰십시오.
public class CostumeDataManager : Singleton<CostumeDataManager>
{
    Dictionary<E_COSTUME_TYPE, ItemInfo> m_myCostumeData = new Dictionary<E_COSTUME_TYPE, ItemInfo>();
    Dictionary<E_COSTUME_TYPE, List<ItemInfo>> m_dicItemTableByType = new Dictionary<E_COSTUME_TYPE, List<ItemInfo>>();
    Dictionary<int, ItemInfo> m_dicItemTableByID = new Dictionary<int, ItemInfo>();

    Dictionary<int, CostumeSetInfo> m_dicBaseCostume = new Dictionary<int, CostumeSetInfo>();
    PlayerBuyItemTable m_dicBuyCostume;

    public override void InitializeManager()
    {
        ItemInfoTable info = new ItemInfoTable();
        info.insertItem(new ItemInfo(1001, "BlueCaptain", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_01_captin", E_AVATA_TYPE.TYPE1, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1002, "FeamelCoat", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_02_feamel_coat", E_AVATA_TYPE.TYPE2, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1003, "KnightArmor", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_03_knight", E_AVATA_TYPE.TYPE3, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1004, "Cowgirl", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_04_cowgirl", E_AVATA_TYPE.TYPE2, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1005, "Sheriff", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_05_sheriff", E_AVATA_TYPE.TYPE1, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1006, "WesternWoman", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_06_western_woman", E_AVATA_TYPE.TYPE1, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1007, "WesternTownman", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_07_western_townman", E_AVATA_TYPE.TYPE2, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1008, "King", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_08_king", E_AVATA_TYPE.TYPE1, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1009, "Queen", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_09_queen", E_AVATA_TYPE.TYPE2, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1010, "Samurai", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_10_samurai", E_AVATA_TYPE.TYPE3, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1011, "Viking", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_11_viking_01", E_AVATA_TYPE.TYPE1, -1, E_ITEMRANK.NORMAL));
        info.insertItem(new ItemInfo(1012, "SpaceSuit", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_12_spacesuit", E_AVATA_TYPE.TYPE1, -1));
        info.insertItem(new ItemInfo(1013, "Diving Dress", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_13_scuba", E_AVATA_TYPE.TYPE3, -1));
        info.insertItem(new ItemInfo(1014, "BoxingGuy1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_14_boxing_A", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1015, "BoxingGuy2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_14_boxing_B", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1016, "Basket Uniform1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_15_basketball_A", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1017, "Basket Uniform2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_15_basketball_B", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1018, "Soccer Uniform1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_16_soccer_A", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1019, "Soccer Uniform2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_16_soccer_B", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1020, "Pilot Uniform", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_17_pilot", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1021, "Stewardess Uniform", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_18_stewardess", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1022, "Soldier Uniform1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_19_soldierbag", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1023, "Soldier Uniform2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_20_soldiertights", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1024, "Soldier Uniform3", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_22_soldiershort", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1025, "Soldier Uniform4", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_23_soldierUniform", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1026, "Soldier Uniform5", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_24_bulletproof", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1027, "Helmet Man", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_21_soldierhelmetman", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1028, "Man Suit", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_25_mansuit", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1029, "Woman Suit", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_26_womansuit", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1030, "Tail Coat1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_27_joker", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1031, "Tail Coat2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_28_jacketcoat", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1032, "Tail Coat3", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_29_dresscoat", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1033, "Tail Coat4", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_30_tailcoat", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1034, "Rafting Uniform1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_31_rafting_A", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1035, "Rafting Uniform2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_31_rafting_B", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1036, "Warrior1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_30_tailcoat", E_AVATA_TYPE.TYPE2, -1));
        info.insertItem(new ItemInfo(1037, "Warrior2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_33_femalewarrior", E_AVATA_TYPE.TYPE2, -1));
        info.insertItem(new ItemInfo(1038, "Paladin", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_34_paladin", E_AVATA_TYPE.TYPE2, -1));
        info.insertItem(new ItemInfo(1039, "School Uniform1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_35_schoolgirl", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1040, "School Uniform2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_36_schoolboy", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1041, "School Uniform3", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_37_schoolcoat", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1042, "School Uniform4", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_38_schoolcoatfemale", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1043, "Children Suit1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_39_childsuit", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1044, "Children Suit2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_40_childdress", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1045, "Active wear1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_41_croptop", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1046, "Active wear2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_43_croptopvest", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1047, "Fighter", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_42_fightingdog", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1048, "Security Uniform1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_44_security", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1049, "Security Uniform2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_45_securityvest", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1050, "Nurse Uniform", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_46_nurse", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1051, "Doctor Uniform", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_47_doctor", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1052, "Witch", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_48_witch", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1053, "Police Uniform1", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_49_police", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1054, "Police Uniform2", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_50_policefemale", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1055, "Teacher Uniform", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_51_teacher", E_AVATA_TYPE.TYPE4, -1));
        info.insertItem(new ItemInfo(1056, "Muaythai", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_52_muaythai", E_AVATA_TYPE.TYPE1, -1));
        info.insertItem(new ItemInfo(1057, "Police Robot", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_53_policemechanic", E_AVATA_TYPE.TYPE3, -1));
        info.insertItem(new ItemInfo(1058, "Agent Robot", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_54_agentmechanic", E_AVATA_TYPE.TYPE3, -1));
        info.insertItem(new ItemInfo(1059, "Cat wear", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_55_catsleepwear", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1060, "Puppy wear", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_56_puppysleepwaer", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1061, "Panda wear", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_57_pandasleepwaer", E_AVATA_TYPE.TYPE5, -1));
        info.insertItem(new ItemInfo(1062, "Rabbit wear", E_COSTUME_TYPE.COSTUME, "You fuck", "people_costum_58_rabbitsleepwaer", E_AVATA_TYPE.TYPE5, -1));

        info.insertItem(new ItemInfo(2001, "Hand", E_COSTUME_TYPE.HAND, "You fuck", "people_hand_00", E_AVATA_TYPE.NONE, 20156));

        info.insertItem(new ItemInfo(3001, "Shoes", E_COSTUME_TYPE.FOOT, "You fuck", "people_foot_01", E_AVATA_TYPE.NONE, 20152));

        info.insertItem(new ItemInfo(4001, "Fire", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_01", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4002, "Line", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_02", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4003, "Perm", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_03", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4004, "Short", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_04", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4005, "Twintail", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_05", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4006, "Tied Hair", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_06", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4007, "Gentle Hair", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_07", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4008, "Bald Hair", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_08", E_AVATA_TYPE.NONE, -1));
        info.insertItem(new ItemInfo(4009, "PonyTail", E_COSTUME_TYPE.HAIR, "You fuck", "people_hair_09", E_AVATA_TYPE.NONE, -1));

        info.insertItem(new ItemInfo(5001, "RedShirt", E_COSTUME_TYPE.CHEST, "You fuck", "people_chest_01", E_AVATA_TYPE.NONE, 20143));

        info.insertItem(new ItemInfo(6001, "YellowPants", E_COSTUME_TYPE.BOTTOM, "You fuck", "people_leg_01", E_AVATA_TYPE.NONE, 20141));

        //JsonManager.Instance.CreateJson<ItemInfoTable>(info, "CostumeData");

        // 코스튬 정보 입력
        info = JsonManager.Instance.LoadJsonFile<ItemInfoTable>("Data", "CostumeData");

        for(int i = 0; i < info.arrItem.Count; i++)
        {
            InsertItemInfo(info.arrItem[i]);
        }

        CostumeSetItemtable table = new CostumeSetItemtable();
        table.arrItem.Add(new CostumeSetInfo(1001, 5001, 6001, -1, 4001, 3001));

        table.arrItem[0].insertItem(E_COSTUME_TYPE.BOTTOM, 6001);
        table.arrItem[0].insertItem(E_COSTUME_TYPE.CHEST, 5001);
        table.arrItem[0].insertItem(E_COSTUME_TYPE.COSTUME, 1047);
        table.arrItem[0].insertItem(E_COSTUME_TYPE.FOOT, 3001);
        table.arrItem[0].insertItem(E_COSTUME_TYPE.HAIR, 4002);
        table.arrItem[0].insertItem(E_COSTUME_TYPE.HAND, 2001);

#if UNITY_EDITOR
        JsonManager.Instance.CreateJson<CostumeSetItemtable>(table, "CostumeSetData");
#endif

        // 코스튬 세트 아이템 정보 입력
        CostumeSetItemtable setInfo = JsonManager.Instance.LoadJsonFile<CostumeSetItemtable>("Data", "CostumeSetData");

        for (int i = 0; i < setInfo.arrItem.Count; i++)
        {
            if (m_dicBaseCostume.ContainsKey(setInfo.arrItem[i].CostumeSetID) == false)
            {
                m_dicBaseCostume.Add(setInfo.arrItem[i].CostumeSetID, setInfo.arrItem[i]);
            }
        }

        // 유저가 구매한 아바타 리스트
        m_dicBuyCostume = JsonManager.Instance.LoadJsonPlayerPrefs<PlayerBuyItemTable>("buyList");

        if(m_dicBuyCostume == null)
        {
            m_dicBuyCostume = new PlayerBuyItemTable();
            m_dicBuyCostume.insertItem(1001, true);
            JsonManager.Instance.SaveJsonPlayerPrefs("buyList", m_dicBuyCostume);
        }
    }

    public bool GetBuyItem(int id)
    {

        if (m_dicBuyCostume.getBuyItem(id))
        {
            return true;
        }

        return false;
    }

    public void SaveBuyItem(int itemID, bool value)
    {
        if (m_dicBuyCostume != null)
        {
            m_dicBuyCostume.insertItem(itemID, value);
        }
        else
        {
            m_dicBuyCostume = new PlayerBuyItemTable();
            m_dicBuyCostume.insertItem(itemID, value);
        }

        JsonManager.Instance.SaveJsonPlayerPrefs("buyList", m_dicBuyCostume);
    }


    public int GetCostumeCost(E_ITEMRANK itemRank)
    {
        int price = 0;
        switch (itemRank)
        {
            case E_ITEMRANK.NORMAL:
                price = Common.RANDOM_NORMAL_AVATA_PRICE;
                break;
            case E_ITEMRANK.RARE:
                price = Common.RANDOM_RARE_AVATA_PRICE;
                break;
            case E_ITEMRANK.LEGEND:
                price = Common.RANDOM_LEGEND_AVATA_PRICE;
                break;
        }

        return price;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"> 1001번은 Base 코스튬 </param>
    /// <returns></returns>
    public CostumeSetInfo GetCostumeSetInfo(int ID = 1001)
    {
        CostumeSetInfo itemID;

        if (m_dicBaseCostume.TryGetValue(ID, out itemID))
        {
            return itemID;
        }
        return null;
    }

    public CostumeSetInfo GetRandomCostumeSetInfo()
    {
        int randomCostumeID = Random.Range(0, m_dicBaseCostume.Count) + 1001;
        return GetCostumeSetInfo(randomCostumeID);
    }


    public void InsertItemInfo(int itemID, string name, E_COSTUME_TYPE _type, string info, string path, E_AVATA_TYPE avtaType, int materialID)
    {
        ItemInfo newItem = new ItemInfo(itemID, name, _type, info, path, avtaType, materialID);
        Debug.Log(itemID + " , " + name + " , " + _type + " , " + info + " , " + path);
        InsertItemInfo(newItem);
    }

    public void InsertItemInfo(ItemInfo info)
    {
        if (m_dicItemTableByID.ContainsKey(info.ItemID))
        {
            return;
        }

        m_dicItemTableByID.Add(info.ItemID, info);

        info.itemIconResourcePath = info.itemResourcePath.Replace("people", "icon");

        List<ItemInfo> list;
        if (m_dicItemTableByType.TryGetValue(info.cosutumeType, out list))
        {
            list.Add(info);
        }
        else
        {
            list = new List<ItemInfo>();
            list.Add(info);
            m_dicItemTableByType.Add(info.cosutumeType, list);
        }
    }

    public ItemInfo GetItemInfo(int _itemID)
    {
        ItemInfo itemInfo;

        if (m_dicItemTableByID.TryGetValue(_itemID, out itemInfo))
        {
            return itemInfo;
        }

        return null;
    }

    public List<ItemInfo> GetAllCostumeItemByType(E_COSTUME_TYPE costume)
    {
        List<ItemInfo> list;

        if (m_dicItemTableByType.TryGetValue(costume, out list))
        {
            return list;
        }
        return null;
    }

    public void SetPartsData(ItemInfo _Obj)
    {
        if (m_myCostumeData.ContainsKey(_Obj.cosutumeType))
        {
            m_myCostumeData[_Obj.cosutumeType] = _Obj;
        }
        else
        {
            m_myCostumeData.Add(_Obj.cosutumeType, _Obj);
        }
    }

    public ItemInfo GetPartsData(E_COSTUME_TYPE _Type)
    {
        ItemInfo obj;
        if (m_myCostumeData.TryGetValue(_Type, out obj))
        {
            return obj;
        }

        CostumeSetInfo info = GetCostumeSetInfo();

        int itemID;
        if(info.dic.TryGetValue(_Type, out itemID))
        {
            return GetItemInfo(itemID);
        }

        return null;
    }






}