
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    public E_SCENE_TYPE m_sceneType { get; private set; } = E_SCENE_TYPE.NONE;
    SceneMain m_currentMain { get; set; }

    E_SCENE_TYPE m_NextRoomType;
    public string m_NextRoomName;

    Coroutine m_LoadingCoroutine;

    public override void InitializeManager()
    {

        Debug.Log(gameObject.name + "Initialize Success!!");
        return;
    }


    public void LoadScene(E_SCENE_TYPE _sceneType, string _roomName)
    {
        if (m_currentMain != null)
        {
            m_currentMain.ExitSceneInit();
        }

        m_NextRoomName = _roomName;
        m_NextRoomType = _sceneType;
        SceneManager.LoadScene("scene_Loading", LoadSceneMode.Single);

        // StartCoroutine(LoadScene_C(_sceneType));
    }

    public void LoadReservedScene()
    {
        if(m_LoadingCoroutine != null)
        {
            StopCoroutine(m_LoadingCoroutine);
        }

        m_LoadingCoroutine = StartCoroutine(LoadScene_C(m_NextRoomType));
    }
    IEnumerator LoadScene_C(E_SCENE_TYPE _sceneType)
    {
        yield return new WaitForSeconds(4.0f);

        Debug.Log(_sceneType.ToString() + " Try to loading Scene");

        string sceneName = Common.GetSceneName(_sceneType);
        AsyncOperation Op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        Op.allowSceneActivation = false;

        while (!Op.isDone)
        {
            yield return null;
            Debug.Log(sceneName.ToString() + " Loading...");
            if (Op.progress >= 0.9f)
            {
                m_sceneType = _sceneType;

                Op.allowSceneActivation = true;
            }
        } 
    }

}
