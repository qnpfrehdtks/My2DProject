using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    List<IPoolingObject> m_ListMyPoolingObject = new List<IPoolingObject>();
    Queue<IPoolingObject> m_QActiveObject = new Queue<IPoolingObject>();
   
    GameObject m_OriginalPrefab;
    bool m_isInitialize = false;

    public void InitializePool(int count, GameObject _prefab)
    {
        if(m_isInitialize == true)
        {
            return;
        }

        m_OriginalPrefab = _prefab;
        for (int i = 0; i < count; i++)
        {
            CreateAndPushObject(_prefab);
        }

        m_isInitialize = true;
    }

    public GameObject GetOriginal()
    {
        return m_OriginalPrefab;
    }

    IPoolingObject CreateAndPushObject(GameObject _prefab)
    {
        if (!Common.CheckIsNull(_prefab))
            return null;

        GameObject newGameObject = Instantiate(_prefab);

        if (newGameObject != null)
        {
            IPoolingObject NO = newGameObject.GetComponent<IPoolingObject>();

            if (!Common.CheckIsNull(NO))
            {
                return null;
            }

            newGameObject.name = _prefab.name + "_" + transform.childCount.ToString();

            NO.Initialize(this.gameObject);
            PushToPool(NO);
            m_ListMyPoolingObject.Add(NO);

            return NO;
        }

        return null;
    }

    public IPoolingObject PopFromPool() 
    {
        if (m_QActiveObject.Count == 0)
        {
            CreateAndPushObject(m_OriginalPrefab);
        }

        IPoolingObject newObject = m_QActiveObject.Dequeue();
        newObject.OnPopFromQueue();

        return newObject;
    }

    public void PushToPool(IPoolingObject po)
    {
        m_QActiveObject.Enqueue(po);
        (po as MonoBehaviour).transform.SetParent(transform);
        po.OnPushToQueue();
    }

    public void AllClearPool()
    {
        for (int i = 0; i < m_ListMyPoolingObject.Count; i++)
        {
            Destroy((m_ListMyPoolingObject[i] as MonoBehaviour).gameObject);
        }

        m_QActiveObject.Clear();
        m_ListMyPoolingObject.Clear();
    }

    public void AllPushToPool()
    {
        for(int i = 0; i < m_ListMyPoolingObject.Count; i++)
        {
            PushToPool(m_ListMyPoolingObject[i]);
        }
    }

}

