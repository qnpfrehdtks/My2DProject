using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : Singleton<CharacterManager>
{
    public int m_CurrentWearCostume;

    GameObject m_characterBase;

    

    [HideInInspector]
    public CharacterBase m_CurrentMyCharacter { get; set; }
    public List<CharacterBase> m_ListCurrentCharacter { get; set; } = new List<CharacterBase>();
    public List<CharacterBase> m_ListGoalCharacter { get; set; } = new List<CharacterBase>();


    public override void InitializeManager()
    {
        m_characterBase = ResourceManager.Instance.Load<GameObject>("Prefabs/CharacterBase");
        m_ListGoalCharacter.Clear();
        InitMyCostumeID();

        PoolingManager.Instance.CreatePool(m_characterBase, 8);
    }

    void InitMyCostumeID()
    {
        if (PlayerPrefs.HasKey("myCostumeID"))
        {
            m_CurrentWearCostume = PlayerPrefs.GetInt("myCostumeID");
        }
        else
        {
            m_CurrentWearCostume = 1001;
            PlayerPrefs.SetInt("myCostumeID", m_CurrentWearCostume);
        }
    }

    public void CreateMyCharacter(Vector3 _pos, Quaternion _rot)
    {
        GameObject newCharacter = PoolingManager.Instance.PopFromPool(m_characterBase, _pos, _rot);
        m_CurrentMyCharacter = newCharacter.GetComponent<CharacterBase>();
        
        if (m_CurrentMyCharacter != null)
        {
            m_CurrentMyCharacter.Init(true, 0);
            CameraManager.Instance.SetTarget(m_CurrentMyCharacter.gameObject);
            CameraManager.Instance.SetDistance(4f, 4f);
            m_CurrentMyCharacter.SetCostumeByID(m_CurrentWearCostume);
            newCharacter.transform.position = _pos;
            newCharacter.transform.rotation = _rot;
        }
        else
        {
            Common.LogError("Error! myCurrentCharacter = null");
        }

        m_ListCurrentCharacter.Add(m_CurrentMyCharacter);
    }

    public void CreateOtherCharacter(Vector3 _pos, Quaternion _rot)
    {
        
        GameObject newCharacter = PoolingManager.Instance.PopFromPool(m_characterBase, _pos, _rot);
        CharacterBase otherCharacter = newCharacter.GetComponent<CharacterBase>();

        if (otherCharacter != null)
        {
            otherCharacter.Init(false, m_ListCurrentCharacter.Count);
            otherCharacter.SetCostumeByRandom();
            otherCharacter.transform.position = _pos;
            otherCharacter.transform.rotation = _rot;
        }
        else
        {
            Common.LogError("can not create newCharacter");
        }

        m_ListCurrentCharacter.Add(otherCharacter);

    }

    public void SetCostumeMyCharacter(int CosumeID)
    {
        PlayerPrefs.SetInt("myCostumeID", CosumeID);
        SetCostumeCharacter(m_CurrentMyCharacter,  CosumeID);
    }


    public void SetCostumeCharacter(CharacterBase character, int CosumeID)
    {
        if (character != null)
        {
            character.SetCostumeByID(CosumeID);
        }
        else
        {
            Common.LogError("MyCharacter = null!!");
        }
    }

    public void SetPositionMyCharacter(Vector3 _pos, Quaternion _rot)
    {
        if (m_CurrentMyCharacter != null)
        {
            m_CurrentMyCharacter.transform.position = _pos;
            m_CurrentMyCharacter.transform.rotation = _rot;
        }
        else
        {
            Common.LogError("MyCharacter = null!!");
        }
    }

    public void AllPushOtherCharacter()
    {
        foreach(var character in m_ListCurrentCharacter)
        {
            character.PushToPool();
        }

        m_ListCurrentCharacter.Clear();
        m_ListGoalCharacter.Clear();
    }

    public void SetShowOtherCharacter(bool active)
    {
        for(int i = 0; i < m_ListCurrentCharacter.Count; i++)
        {
            m_ListCurrentCharacter[i].gameObject.SetActive(active);
        }
    }
    public void GoalCharacter(CharacterBase winCharacter)
    {
        winCharacter.Rank = m_ListGoalCharacter.Count;
        m_ListGoalCharacter.Add(winCharacter);
      
    }

    public void OnStartGame()
    {
        if(m_CurrentMyCharacter != null)
            m_CurrentMyCharacter.m_BillBoard.gameObject.SetActive(false);

        for (int i = 0; i < m_ListCurrentCharacter.Count; i++)
        {
            m_ListCurrentCharacter[i].ChangeState(E_CHARACTER_STATE.RUN);
        }
    }
}
