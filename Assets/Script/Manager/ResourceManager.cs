using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    Dictionary<int, Texture> m_dicTexture = new Dictionary<int, Texture>();
    
    public override void InitializeManager()
    {
        LoadAllTexture();
        LoadAllSprite("Image/Item");
        LoadAllSprite("Image/RegionSprite");
    }

    public T Load<T>(string path) where T : Object
    {
        string[] sp = path.Split('/');
        GameObject obj = PoolingManager.Instance.GetOriginalFromPool(sp[sp.Length - 1]);

        if (obj == null)
        {
            T tobj = Resources.Load<T>(path);
            if (tobj == null)
            {
                Debug.LogError("Fail Load Resource " + path);
                return null;
            }
            return tobj;
        }

        return obj as T;
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        T[] obj = Resources.LoadAll<T>(path);

        if (obj == null)
        {
            Debug.LogError("Fail Load Resource " + path);
            return null;
        }
        return obj;
    }

    public T instantiate<T>(string path, Transform parent = null) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Fail Instantiate Resource");
            return null;
        }

        T Prefabs = Load<T>(path);

        if (Prefabs == null)
            return null;

        return instantiate(Prefabs, parent);
    }

    public T instantiate<T>(T newResouces, Transform parent = null) where T : UnityEngine.Object
    {
        if (newResouces == null)
        {
            Debug.LogError("Fail Instantiate Resource");
            return null;
        }

        IPoolingObject poolingObj = (newResouces as GameObject).GetComponent<IPoolingObject>();
        if(poolingObj == null)
        {
          return Object.Instantiate(newResouces, parent);
        }
        else
        {
            return PoolingManager.Instance.PopFromPool(newResouces as GameObject, Vector3.zero,Quaternion.identity) as T;
        }

    }

    public void DestroyObject(GameObject go)
    {
        IPoolingObject poolingObject =  go.GetComponent<IPoolingObject>();

        if(poolingObject == null)
        {
            Destroy(go);
            return;
        }

        PoolingManager.Instance.PushToPool(go);        
    }

    void LoadAllTexture()
    {
        Texture[] texture = LoadAll<Texture>("Textures");

        for (int i = 0; i < texture.Length; i++)
        {
            int texID = -1;
            if (int.TryParse(texture[i].name, out texID))
            {
                if (!m_dicTexture.ContainsKey(texID))
                {
                    m_dicTexture.Add(texID, texture[i]);
                }
            }
        }
    }

    public Texture GetTexture(int textureID)
    {
        Texture tex;

        if(m_dicTexture.TryGetValue(textureID, out tex))
        {
            return tex;
        }

        return null;

    }

    Dictionary<string, Sprite> m_dicSprite = new Dictionary<string, Sprite>();

    public void LoadAllSprite(string path)
    {//"Image/Item"
        Sprite[] sprite = LoadAll<Sprite>(path);

        for (int i = 0; i < sprite.Length; i++)
        {
            if (!m_dicSprite.ContainsKey(sprite[i].name))
            {
                m_dicSprite.Add(sprite[i].name, sprite[i]);
            }
        }
    }

    /// <summary>
    ///  SpriteID 해당 스프라이트 name 
    /// </summary>
    /// <param name="spriteID"> sprite name = sprite ID </param>
    /// <returns></returns>
    public Sprite GetSprite(string spriteID)
    {
        Sprite tex;
        if (m_dicSprite.TryGetValue(spriteID, out tex))
        {
            return tex;
        }

        Debug.LogError(spriteID);
        return null;

    }


}
