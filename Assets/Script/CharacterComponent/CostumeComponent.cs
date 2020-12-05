// 2020.04 소승호 파츠 재구성
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CostumeComponent : MonoBehaviour
{
    CharacterBase m_myCharacter;

    Dictionary<E_COSTUME_TYPE, List<GameObject>> m_myCostumeData = new Dictionary<E_COSTUME_TYPE, List<GameObject>>();

    SkinnedMeshRenderer m_HeadRenderer;
    SkinnedMeshRenderer m_MouseRenderer;
    SkinnedMeshRenderer m_FaceRenderer;

    int m_myHairColorID;
    int m_mySkinColorID;

    public void Init()
    {
        m_myCharacter = GetComponent<CharacterBase>();

        m_mySkinColorID = 20156;
        m_myHairColorID = 20071;

        m_HeadRenderer = m_myCharacter.m_Model.transform.Find("people_head_00").GetComponent<SkinnedMeshRenderer>();
        m_MouseRenderer = m_myCharacter.m_Model.transform.Find("people_mouse").GetComponent<SkinnedMeshRenderer>();
        m_FaceRenderer = m_myCharacter.m_Model.transform.Find("people_face_00").GetComponent<SkinnedMeshRenderer>();

       // OnLoadAllParts(1001);
    }

    public void OnLoadAllParts(int ID = 1001)
    {
        CostumeSetInfo setInfo = CostumeDataManager.Instance.GetCostumeSetInfo(ID);
        OnLoadAllParts(setInfo);
    }

    public void OnLoadRandomAllParts()
    {
        CostumeSetInfo setInfo = CostumeDataManager.Instance.GetRandomCostumeSetInfo();
        OnLoadAllParts(setInfo);
    }

    void OnLoadAllParts(CostumeSetInfo setInfo)
    {
        if (setInfo == null)
            return;

        E_COSTUME_TYPE type = E_COSTUME_TYPE.HAIR;

        for (; type < E_COSTUME_TYPE.END; ++type)
        {
            int costumeID;
            ItemInfo info;

            if (setInfo.dic.TryGetValue(type, out costumeID))
            {
                info = CostumeDataManager.Instance.GetItemInfo(costumeID);
            }
            else
            {
                info = CostumeDataManager.Instance.GetPartsData(type);
            }

            if (info == null)
            {
                continue;
            }

            ChangePart(m_myCharacter, info);
        }
    }

     void ChangeMyPart(ItemInfo info)
    {
        if (m_myCharacter == null)
        {
            Debug.Log("Not Have Character");
            return;
        }

        CostumeDataManager.Instance.SetPartsData(info);
        ChangePart(info.ItemID);
      
    }

    void ChangePart(CharacterBase character, ItemInfo info)
    {
        if (character == null)
        {
            Debug.Log("Not Have Character");
            return;
        }

        CostumeDataManager.Instance.SetPartsData(info);
        ChangePart(info.ItemID);
    }

    void ChangePart(int _ItemID)
    {

        ItemInfo info = CostumeDataManager.Instance.GetItemInfo(_ItemID);

        if (info == null)
        {
            Debug.LogError("해당 아이템 정보는 없습니다.");
            return;
        }

        if (string.IsNullOrEmpty(info.itemResourcePath))
        {
            Debug.LogError("잘못된 리소스 경로입니다.");
            return;
        }

        GameObject resource = ResourceManager.Instance.Load<GameObject>("Prefabs/Parts/" + info.itemResourcePath);

         if (resource == null)
         {
            Debug.LogError("Item ID : " + _ItemID + " , " + info.cosutumeType + "Load");
            return;
         }

       
         DeleteCurrentParts(info.cosutumeType);

         GameObject obj = Instantiate(resource);
        GameObject hairObject = FindCostumeHair(obj);
        SkinnedMeshRenderer[] skin = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

         for (int i = 0; i < skin.Length; i++)
         {
             skin[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;

             FindBones(skin[i], m_myCharacter.m_Model.transform);
             skin[i].rootBone = FindRootBone(skin[i].bones, skin[i].rootBone.name);
             skin[i].transform.SetParent(m_myCharacter.m_Model.transform);
             skin[i].gameObject.transform.localScale = Vector3.one;

             SetPartsObject(info.cosutumeType, skin[i].gameObject);
         }

        if (info.materialID != -1)
            ChangeColor(info.cosutumeType, info.materialID);
        if (m_myHairColorID != -1)
            ChangeColor(E_COSTUME_TYPE.HAIRCOLOR, m_myHairColorID);
        if (m_mySkinColorID != -1)
            ChangeColor(E_COSTUME_TYPE.SKINCOLOR, m_mySkinColorID);

        if (info.cosutumeType == E_COSTUME_TYPE.COSTUME)
        {
            ChangeCostume(info, hairObject);
            Destroy(obj);
        }
        else
        {


            Destroy(obj);
        }
    }

    GameObject FindCostumeHair(GameObject costume)
    {
        if(costume == null)
        {
            return null;
        }

        Component[] components = costume.GetComponentsInChildren<Component>();

        for(int i = 0; i < components.Length; i++)
        {
            if( components[i].gameObject.tag == "Head" && !components[i].gameObject.name.Contains("costum_head"))
            {
                return components[i].gameObject;
                
            }
        }

        return null;
    }

    //타입1 = 얼굴,피부 제외한 코스튬,
    //타입2 = 얼굴,헤어,피부 제외한 코스튬
    //타입3 = 전신코스튬 다벗김
    //타입4 = 얼굴,헤어,피부,손 제외한 코스튬
    void ChangeCostume(ItemInfo item, GameObject hairGameObject)
    {
        DeleteCurrentParts(E_COSTUME_TYPE.CHEST);
        DeleteCurrentParts(E_COSTUME_TYPE.BOTTOM);

        switch (item.avataType)
        {
            case E_AVATA_TYPE.TYPE1:
                DeleteCurrentParts(E_COSTUME_TYPE.ARM);
                DeleteCurrentParts(E_COSTUME_TYPE.HAND);
                DeleteCurrentParts(E_COSTUME_TYPE.HAIR);
                DeleteCurrentParts(E_COSTUME_TYPE.FOOT);

                if(hairGameObject == null)
                    hairGameObject = FindCostumeHair(m_myCharacter.m_Model);

                SetPartsObject(E_COSTUME_TYPE.HAIR, hairGameObject);

                FaceModelVisible(true);

                ChangeColor(E_COSTUME_TYPE.HAIRCOLOR, m_myHairColorID);
                ChangeColor(E_COSTUME_TYPE.COSTUME, m_mySkinColorID);
                return;
            case E_AVATA_TYPE.TYPE3:
                DeleteCurrentParts(E_COSTUME_TYPE.ARM);
                DeleteCurrentParts(E_COSTUME_TYPE.HAND);
                DeleteCurrentParts(E_COSTUME_TYPE.HAIR);
                DeleteCurrentParts(E_COSTUME_TYPE.FOOT);

                FaceModelVisible(false);

                ChangeColor(E_COSTUME_TYPE.COSTUME, m_mySkinColorID);
                return;
            case E_AVATA_TYPE.TYPE2:
            case E_AVATA_TYPE.TYPE4:
            case E_AVATA_TYPE.NONE:
                DeleteCurrentParts(E_COSTUME_TYPE.ARM);
                DeleteCurrentParts(E_COSTUME_TYPE.HAND);
                DeleteCurrentParts(E_COSTUME_TYPE.HAIR);
                DeleteCurrentParts(E_COSTUME_TYPE.FOOT);

                ItemInfo hairItem = CostumeDataManager.Instance.GetPartsData(E_COSTUME_TYPE.HAIR);
               ChangeMyPart(hairItem);

                FaceModelVisible(true);

                ChangeColor(E_COSTUME_TYPE.HAIRCOLOR, m_myHairColorID);
                ChangeColor(E_COSTUME_TYPE.COSTUME, m_mySkinColorID);

                return;
            case E_AVATA_TYPE.TYPE5:
                DeleteCurrentParts(E_COSTUME_TYPE.ARM);
                DeleteCurrentParts(E_COSTUME_TYPE.HAND);
                DeleteCurrentParts(E_COSTUME_TYPE.HAIR);
                DeleteCurrentParts(E_COSTUME_TYPE.FOOT);

                ItemInfo handItem = CostumeDataManager.Instance.GetPartsData(E_COSTUME_TYPE.HAND);
                ChangeMyPart(handItem);

                if (hairGameObject == null)
                    hairGameObject = FindCostumeHair(m_myCharacter.m_Model);

                SetPartsObject(E_COSTUME_TYPE.HAIR, hairGameObject);

                FaceModelVisible(true);

                ChangeColor(E_COSTUME_TYPE.HAIRCOLOR, m_myHairColorID);
                ChangeColor(E_COSTUME_TYPE.COSTUME, m_mySkinColorID);
                ChangeColor(E_COSTUME_TYPE.SKINCOLOR, m_mySkinColorID);
                break;
            default:
                break;
        }


    }

    void ChangeColor(E_COSTUME_TYPE _Type, int _TextureID = 0, int _SharedTextureID = 0)
    {
        if (_TextureID == 0 && _SharedTextureID == 0) return;

        Texture ColorTex = ResourceManager.Instance.GetTexture(_TextureID);

        switch (_Type)
        {
            case E_COSTUME_TYPE.HAIR:
            case E_COSTUME_TYPE.HAIRCOLOR:
                {
                    List<GameObject> list = GetPartsObjectList(E_COSTUME_TYPE.HAIR);

                    if (list != null && list.Count > 0)
                    {
                        for(int i = 0; i< list.Count; i++)
                        {
                            if (list[i] == null) continue;

                            SkinnedMeshRenderer hair = list[i].GetComponent<SkinnedMeshRenderer>();

                            if (hair != null)
                            {
                                SetColor(hair, ColorTex, "hair");
                            }
                        }
                    }

                    SetColor(m_FaceRenderer, ColorTex, "hair");
                }
                break;
            case E_COSTUME_TYPE.SKINCOLOR:
            case E_COSTUME_TYPE.COSTUME:
                {
                    for (E_COSTUME_TYPE i = E_COSTUME_TYPE.NONE; i < E_COSTUME_TYPE.END; ++i)
                    {
                        if (i == E_COSTUME_TYPE.HAIRCOLOR || i == E_COSTUME_TYPE.HAIR) continue;

                        List<GameObject> list = GetPartsObjectList(i);

                        if (list != null)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                SetColor(list[j].GetComponent<SkinnedMeshRenderer>(), ColorTex, "body");
                            }
                        }
                    }
                    SetColor(m_HeadRenderer, ColorTex, "body");
                }
                break;
            default:
                {
                    List<GameObject> list = GetPartsObjectList(_Type);

                    for (int i = 0; i < list.Count; i++)
                    {
                        string matname = "";
                        switch (_Type)
                        {
                            case E_COSTUME_TYPE.HAIR:
                                matname = "hair";
                                break;
                            case E_COSTUME_TYPE.CHEST:
                                matname = "chest";
                                break;
                            case E_COSTUME_TYPE.BOTTOM:
                                matname = "leg";
                                break;
                            case E_COSTUME_TYPE.ARM:
                                matname = "arm";
                                break;
                            case E_COSTUME_TYPE.FOOT:
                                matname = "foot";
                                break;
                        }
                        Texture skincolor = ResourceManager.Instance.GetTexture(_SharedTextureID);

                        SetColor(list[i].GetComponent<SkinnedMeshRenderer>(), skincolor, "body");

                        if (ColorTex)
                            SetColor(list[i].GetComponent<SkinnedMeshRenderer>(), ColorTex, matname);
                    }

                    break;
                }
        }
    }

    void SetColor(Renderer skin, Texture tex, string materialname)
    {
        if (tex == null) return;
        foreach (Material m in skin.materials)
        {
            if (m.name.Contains(materialname)) m.mainTexture = tex;
        }
    }

    void FindBones(SkinnedMeshRenderer skin, Transform model)
    {
        Transform[] find_bones = new Transform[skin.bones.Length];

        for (int i = 0; i < skin.bones.Length; i++)
        {
            find_bones[i] = FindModelBone(model, skin.bones[i].name);
        }

        skin.bones = find_bones;
    }

    Transform FindModelBone(Transform model, string bone_name)
    {
        Transform find_bone = null;

        for (int i = 0; i < model.childCount; i++)
        {
            Transform t = model.GetChild(i);
            if (t.name == bone_name) return t;

            t = FindModelBone(t, bone_name);
            if (t != null) find_bone = t;
        }

        return find_bone;
    }

    Transform FindRootBone(Transform[] bones, string bone_name)
    {
        foreach (Transform t in bones)
        {
            if (t.name == bone_name) return t;
        }

        return null;
    }

    public void SetPartsObject(E_COSTUME_TYPE _Type, GameObject _Obj)
    {
        if (m_myCostumeData.ContainsKey(_Type))
        {
            m_myCostumeData[_Type].Add(_Obj);
        }
        else
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(_Obj);
            m_myCostumeData.Add(_Type, list);
        }
    }


    public List<GameObject> GetPartsObjectList(E_COSTUME_TYPE _Type)
    {
        List<GameObject> obj;
        if (m_myCostumeData.TryGetValue(_Type, out obj))
        {
            return obj;
        }

        return null;
    }

    public void DeleteCurrentParts(E_COSTUME_TYPE _Type)
    {
        var go = GetPartsObjectList(_Type);

        if (go != null)
        {
            for(int i = 0; i < go.Count; i++)
            {
                Destroy(go[i]);
            }

            m_myCostumeData.Remove(_Type);
        }
        else
        {



        }


    }

    void FaceModelVisible(bool _Visible)
    {
        m_HeadRenderer.gameObject.SetActive(_Visible);
        m_MouseRenderer.gameObject.SetActive(_Visible);
        m_FaceRenderer.gameObject.SetActive(_Visible);
    }


}

