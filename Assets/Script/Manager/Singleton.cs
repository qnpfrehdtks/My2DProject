using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance;

    static public T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = GameObject.Find(typeof(T).Name);

                if(go == null)
                {
                    go = new GameObject(typeof(T).Name);
                }

                T ins = go.GetComponent<T>();

                if(ins == null)
                {
                    ins = go.AddComponent<T>();
                }

                if(ins != null)
                {
                    ins.InitializeManager();
                }

                return instance = ins;
            }
            else
            {
                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public abstract void InitializeManager();


}
