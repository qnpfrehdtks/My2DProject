using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    Dictionary<string, ObjectPool> m_dicPool = new Dictionary<string, ObjectPool>();
    public override void InitializeManager()
    {
    }

    public void CreatePool(GameObject go, int startCount = 6)
    {
        CreatePool($"@{go.name}", go,  startCount);
    }

    public void CreatePool(string name, GameObject go, int startCount = 6)
    {
        if (m_dicPool.ContainsKey(name))
        {
            return;
        }

        GameObject newPoolObject = new GameObject(name);
        newPoolObject.transform.SetParent(transform);

        ObjectPool newPool = Common.GetOrAddComponent<ObjectPool>(newPoolObject);
        newPool.InitializePool(startCount, go);
        m_dicPool.Add(name, newPool);

        Debug.Log(name + "생성");
    }

    public void PushToPool(GameObject go)
    {
        if (go == null) return;

        IPoolingObject poolObject = go.GetComponent<IPoolingObject>();
        if (poolObject == null) return;

        string[] str = go.name.Split('_');
        CreatePool($"@{str[0]}", go, poolObject.StartCount);

        m_dicPool[$"@{str[0]}"].PushToPool(poolObject);
        go.SetActive(false);
    }

    public GameObject PopFromPool(string prefabName, Vector3 pos, Quaternion quat)
    {
        if (string.IsNullOrEmpty(prefabName))
            return null;

        GameObject original = GetOriginalFromPool($"@{prefabName}");
        return PopFromPool(original, pos, quat);
    }


    public GameObject PopFromPool(GameObject go, Vector3 pos, Quaternion quat)
    {
        if (go == null) 
            return null;

        string[] str = go.name.Split('_');

        IPoolingObject poolingObject = go.GetComponent<IPoolingObject>();

        if(poolingObject == null)
        {
            return null;
        }

        CreatePool($"@{str[0]}", go, poolingObject.StartCount);

        if (m_dicPool.ContainsKey($"@{str[0]}") == false)
            return null;

        IPoolingObject obj = m_dicPool[$"@{str[0]}"].PopFromPool();
        
        if (obj == null)
        {
            return null;
        }
        else
        {
            GameObject newGo = (obj as MonoBehaviour).gameObject;
            newGo.transform.position = pos;
            newGo.transform.rotation = quat;

            newGo.SetActive(true);

            return newGo;
        }
    }

    public void PushAllObjectToPool()
    {
        foreach (var p in m_dicPool)
        {
            p.Value.AllPushToPool();
        }
    }

    public void AllClearPool()
    {
        foreach(var p in m_dicPool)
        {
            p.Value.AllClearPool();
            Destroy(p.Value.gameObject);
        }

        m_dicPool.Clear();
    }

    public void PushAllObjectToPool(GameObject go)
    {
        if (go == null)
            return;

        string[] str = go.name.Split('_');
        ObjectPool pool = null;

        if (m_dicPool.TryGetValue($"@{str[0]}", out pool))
        {
            pool.AllPushToPool();
        }
    }

    public GameObject GetOriginalFromPool(string name)
    {
        ObjectPool pool = null;
        if (m_dicPool.TryGetValue(name, out pool))
        {
            return pool.GetOriginal();
        }

        return null;
    }

    // 특정 
    public void ClearPool(GameObject go)
    {
        if (go == null)
            return;

        string[] str = go.name.Split('_');
        ObjectPool pool;

        if(m_dicPool.TryGetValue($"@{str[0]}", out pool))
        {
            pool.AllClearPool();
        }
    }
}
