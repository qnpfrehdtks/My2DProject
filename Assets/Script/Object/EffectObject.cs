using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour, IPoolingObject
{
    public int StartCount { get; set; } = 20;

    ParticleSystem m_ps;
    Coroutine m_currentCoroutine;
    Transform m_attachedTransform;
    Vector3 m_Offset;

    private void Awake()
    {
        m_ps = GetComponent<ParticleSystem>();
    }

    public void Initialize(GameObject factory)
    {
        transform.SetParent(factory.transform);
    }
    public void OnPushToQueue()
    {
        m_ps.Stop();

        if (m_currentCoroutine != null)
        {
            StopCoroutine(m_currentCoroutine);
            m_currentCoroutine = null;
        }

        m_attachedTransform = null;
        gameObject.SetActive(false);
    }

    public void OnPopFromQueue()
    {
      //  gameObject.SetActive(true);
      //  m_ps.Play();
    }

    public void AttachToTransform(Transform tr, Vector3 offset)
    {
        m_attachedTransform = tr;
        m_Offset = offset;
    }

    void LateUpdate()
    {
        if(m_attachedTransform)
        {
           transform.position = m_attachedTransform.position + m_Offset;
        }
    }

    public void DestroyEffect(float time = 0.0f)
    {
        if (m_currentCoroutine != null)
        {
            StopCoroutine(m_currentCoroutine);
            m_currentCoroutine = null;
        }

        if (time <= 0.0001f)
        {
            PoolingManager.Instance.PushToPool(gameObject);
            return;
        }

        m_currentCoroutine = StartCoroutine(StartDestroy_C(time));
    }

    IEnumerator StartDestroy_C(float time)
    {
        yield return new WaitForSeconds(time);

        PoolingManager.Instance.PushToPool(gameObject);
    }


}
