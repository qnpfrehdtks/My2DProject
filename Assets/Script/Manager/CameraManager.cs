using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public myCameraController m_MainCamera;

    public List<ICameraObstacle> m_ListPrevHitObject = new List<ICameraObstacle>();
    public List<ICameraObstacle> m_ListCurrentHitObject = new List<ICameraObstacle>();

    Vector3 m_Offset;

    Ray m_Ray = new Ray();
    RaycastHit[] hit;

    int LayerCast = 0;

    public override void InitializeManager()
    {
        if(m_MainCamera == null)
        {
           // GameObject resource = ResourceManager.Instance.Load<GameObject>("Main Camera");
            GameObject newCam = ResourceManager.Instance.instantiate<GameObject>("MainCamera");
            m_MainCamera = Common.GetOrAddComponent<myCameraController>(newCam);
            m_MainCamera.SetCameraMode(E_CAMERA_TYPE.QUATER_VIEW);
            m_MainCamera.gameObject.tag = "MainCamera";
        }
        LayerCast = LayerMask.GetMask("Character");
    }

    private void Update()
    {
        if (Camera.main == null) return;
        if (CharacterManager.Instance.m_CurrentMyCharacter == null) return;

        m_Ray.origin = Camera.main.transform.position;
        m_Ray.direction = (CharacterManager.Instance.m_CurrentMyCharacter.transform.position + CharacterManager.Instance.m_CurrentMyCharacter.transform.forward * 0.1f - Camera.main.transform.position).normalized;

        foreach (var go in m_ListPrevHitObject)
        {
            go.ExitCollisionToCameraRay();
        }

        hit = Physics.RaycastAll(m_Ray, 15.0f, LayerCast);

        m_ListCurrentHitObject.Clear();

        foreach (var h in hit)
        {
            ICameraObstacle cam =  h.collider.GetComponent<ICameraObstacle>();
            if (cam == null) continue;

            cam.EnterCollisionToCameraRay();
            m_ListCurrentHitObject.Add(cam);
        }

        m_ListPrevHitObject = m_ListCurrentHitObject;
    }

    public void SetCameraMode(E_CAMERA_TYPE type)
    {
        if (m_MainCamera != null)
        {
            m_MainCamera.SetCameraMode(type);
        }
    }


    public void PlusDistance(float addDistance, float addheight)
    {
        m_MainCamera.PlusDistance(addheight, addDistance);
    }

    public void SetDistance(float distance, float height)
    {
        m_MainCamera.SetDistance(height, distance);
    }

    public void SetTarget(GameObject obj)
    {

        m_MainCamera.SetTarget(obj);
    }


}
