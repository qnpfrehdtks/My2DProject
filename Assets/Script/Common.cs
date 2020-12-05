using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




public class Common
{
    public static string[] strName = { "Ogrima", "Auther", "Yiruma", "Grom", "Dijkstra", "Grunt", "Jain", "Issac", "Mumu", "Thrall", "DogKing", "Arthas", "May", "July", "XieXie" };
     public static string[] regionName = { "ao", "ad", "at", "ar", "au", "be", "bd", "bh", "bn", "bo", "br", "bs", "ca", "ch", "cl","cm","cn","cr", "de", "es", "fr", "gr", "id", "in", "it","jp", "kh", "kr", "vn", "ru", "us" };
    public const string RANDOM_AVATA_ICON = "RandomAvataIcon";

    // 기획자가 있다면, XML, JSON 과 같은 자료를 불러와서 
    // 이러한 값들을 셋팅을 진행한다.
    public const int RANDOM_NORMAL_AVATA_PRICE = 1500;
    public const int RANDOM_RARE_AVATA_PRICE = 4000;
    public const int RANDOM_LEGEND_AVATA_PRICE = 5500;

    static public void LogError(string str)
    {
        Debug.LogError("[Log]" + str);
    }

    static public string GetSceneName(E_SCENE_TYPE _sceneType)
    {
        string sceneName = "scene_" + _sceneType.ToString();
        return sceneName;
    }



    static public bool CheckIsNull(object go)
    {
        if (go == null)
        {
            Debug.LogError("[Log]" + "Is Null");
            return false;
        }

        return true;
    }

    static public T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T obj = gameObject.GetComponent<T>();

        if (obj)
        {
            return obj;
        }

        return gameObject.AddComponent<T>();
    }

    static public T FindChild<T>(GameObject gameObject, string childName = null ) where T : UnityEngine.Object
    {
        T[] children = gameObject.GetComponentsInChildren<T>();

        if(children != null )
        {
            foreach(T t in children)
            {
                if(string.IsNullOrEmpty(childName) || t.name == childName)
                {
                    return t;
                }
            }
        }

        return null;

    }


    static public bool CheckAllRay(Ray ray, int layer, float distance, out RaycastHit hit)
    {
        RaycastHit[] arrHit = Physics.RaycastAll(ray, distance, layer);

        if (arrHit != null && arrHit.Length > 0)
        {
            hit = arrHit[0];
#if UNITY_EDITOR
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);
#endif
            return true;
        }

#if UNITY_EDITOR
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);
#endif
        hit = new RaycastHit();

        return false;
    }

    static public bool CheckRay(Ray ray, int layer, float distance, out RaycastHit hit)
    {
        if(Physics.Raycast(ray, out hit, distance, layer))
        {
#if UNITY_EDITOR
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);
#endif
            return true;
        }

#if UNITY_EDITOR
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);
#endif

        return false;
    }
    




}