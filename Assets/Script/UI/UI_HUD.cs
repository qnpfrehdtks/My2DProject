using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HUD : MonoBehaviour, IPoolingObject
{
    Animator m_Animator;
    TMPro.TextMeshProUGUI m_Text;

    public int StartCount { get; set; } = 1;

    public void Initialize(GameObject factory)
    {
        m_Text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        m_Animator = GetComponent<Animator>();
    }
    public void OnPushToQueue()
    {

    }

    public void OnPopFromQueue()
    {
        
    }

    public void SetText(string cnt, float speed = 1.0f)
    {
        m_Text.text = cnt;
        m_Animator.SetTrigger("Up");
        m_Animator.speed = speed;
    }

}
