using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonManager : Singleton<JsonManager>
{
    public override void InitializeManager()
    {
    }

    void CreateJsonFile(string _createPath, string _fileName, string _jsonData)
    {
        string path = string.Format("{0}/{1}.json", _createPath, _fileName);

        // 파일이 존재하지 않으면 그냥 새로 json을 만들고
        if (!File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(_jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }
    }

    public T LoadJsonPlayerPrefs<T>(string _keyPlayerPrefs) where T : class
    {
        string jsonData;

        if(!PlayerPrefs.HasKey("buyList"))
        {
            return null;
        }

        jsonData = PlayerPrefs.GetString("buyList");
        T data = JsonToOject<T>(jsonData);

        return data;
    }

    public void SaveJsonPlayerPrefs<T>(string _keyPlayerPrefs, T _value) where T : class
    {
        string data = JsonUtility.ToJson(_value);
        PlayerPrefs.SetString(_keyPlayerPrefs, data);
    }

    public T LoadJsonFile<T>(string _loadPath, string _fileName) where T : class
    {
        TextAsset ta = ResourceManager.Instance.Load<TextAsset>(string.Format("{0}/{1}", _loadPath, _fileName));

        if(ta != null && !string.IsNullOrEmpty( ta.ToString()))
            return JsonUtility.FromJson<T>(ta.ToString());
        else
        {
            return null;
        }
    }

    public void CreateJson<T>(T obj, string fileName) where T : class
    {
        string jsonData = ObjectToJson(obj);


        Debug.Log("Create Json : " + jsonData);

        CreateJsonFile(Application.dataPath, fileName, jsonData);
    }


    public string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public T JsonToOject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }



}
