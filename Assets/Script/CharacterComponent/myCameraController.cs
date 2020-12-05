using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myCameraController : MonoBehaviour
{

    E_CAMERA_TYPE m_CameraType;

    GameObject m_TargetObject;

    [SerializeField]
    float m_UpSize = 4.0f;

    [SerializeField]
    float m_DistanceSize = 7.5f;

    [SerializeField]
    Vector3 m_shopOffset = Vector3.one;

    private void LateUpdate()
    {
        if (m_TargetObject == null)
            return;

        switch(m_CameraType)
        {
            case E_CAMERA_TYPE.QUATER_VIEW:
                transform.position = Vector3.Lerp(transform.position, m_TargetObject.transform.position + Vector3.up * m_UpSize - m_TargetObject.transform.forward * m_DistanceSize, Time.deltaTime * 3.5f);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_TargetObject.transform.position + m_TargetObject.transform.forward * 2.0f) - transform.position, Vector3.up), Time.deltaTime * 15.0f);
                break;
            case E_CAMERA_TYPE.SHOP_VIEW:
                transform.position = Vector3.Lerp(transform.position, m_TargetObject.transform.position + Vector3.up * m_UpSize - m_TargetObject.transform.forward * m_DistanceSize, Time.deltaTime * 3.5f);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_TargetObject.transform.position - transform.position).normalized, Vector3.up), Time.deltaTime * 15.0f);
                break;
        }
    }

    public void SetCameraMode(E_CAMERA_TYPE CamMode)
    {
        m_CameraType = CamMode;
    }

    public void SetTarget(GameObject target)
    {
        m_TargetObject = target;
    }

    public void PlusDistance(float height, float distance)
    {
        m_UpSize += height;
        m_DistanceSize += distance;
    }

    public void SetDistance(float height, float distance)
    {
        m_UpSize = height;
        m_DistanceSize = distance;
    }

}
