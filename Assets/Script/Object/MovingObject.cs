using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour, IPoolingObject
{
    [SerializeField]
    public int Gold;

    public int StartCount { get; set; } = 30;
    public void Initialize(GameObject factory)
    {

    }
    public void OnPushToQueue()
    {

    }
    public void OnPopFromQueue()
    { 
    }

    void Update()
    {
        if (UIManager.Instance.m_Target == null)
        {
            return;
        }

       // transform.position = Vector3.Slerp(transform.position, UIManager.Instance.m_Target.transform.position, Time.deltaTime * 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            UIManager.Instance.OnChangeGoldUI(GameMaster.ONE_GOLD_VALUE);
            PoolingManager.Instance.PushToPool(gameObject);
            
        }
    }
}
