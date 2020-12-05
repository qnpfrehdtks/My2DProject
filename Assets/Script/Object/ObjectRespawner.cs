using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PILE_TYPE
{
    ONE,
    THREE,
    SIX,
    TEN
}

public class ObjectRespawner : MonoBehaviour
{
    [SerializeField]
    E_PILE_TYPE m_Type = E_PILE_TYPE.ONE;

    List<IRespawnObject> m_ListInteractObject = new List<IRespawnObject>();
    List<Vector3> m_ListOffset = new List<Vector3>();

  //  float RegenTime = 5.0f;

    GameObject m_Wood;

    public void Init()
    {
        switch(m_Type)
        {
            case E_PILE_TYPE.ONE:
                m_ListOffset.Add(Vector3.zero);
                break;
            case E_PILE_TYPE.THREE:
                m_ListOffset.Add(Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * 0.5f);
                m_ListOffset.Add(Vector3.forward * -0.5f);
                break;
            case E_PILE_TYPE.SIX:
                m_ListOffset.Add(Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * 0.5f);
                m_ListOffset.Add(Vector3.forward * -0.5f);
                m_ListOffset.Add(Vector3.up * 0.45f);
                m_ListOffset.Add(Vector3.forward * 0.5f + Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * -0.5f + Vector3.up * 0.2f);
                break;
            case E_PILE_TYPE.TEN:

                break;
        }

        m_Wood = ResourceManager.Instance.Load<GameObject>("Prefabs/Wood");
        RegenWood();
    }

    public void AllClearObject()
    {
        m_ListOffset.Clear();
        m_ListInteractObject.Clear();
    }

    public void RegenWood()
    {
        for (int i = 0; i < m_ListOffset.Count; i++)
        {   
            GameObject respawnItem = PoolingManager.Instance.PopFromPool(m_Wood, transform.position + m_ListOffset[i], transform.rotation);
            IRespawnObject respawn = respawnItem.GetComponent<IRespawnObject>();

            if (respawn != null)
            {
                respawn.RespawnNumber = m_ListInteractObject.Count;
                respawn.InitializeRespawn(this);

                m_ListInteractObject.Add(respawn);
            }
            else
            {
                Common.LogError("Not have a respawn Item " + respawnItem.name);
            }
        }
    }

    public void RemoveWood(IRespawnObject obj)
    {
        if(m_ListInteractObject.Remove(obj))
        {

        }
    }


}
