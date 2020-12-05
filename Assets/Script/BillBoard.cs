using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillBoard : MonoBehaviour
{
    TMPro.TextMeshProUGUI m_text;
    Image m_Image;

    private void Awake()
    {
        m_text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        m_Image = GetComponentInChildren<Image>();
    }


    public void InitProfileSprite(string str)
    {
        if (str == null)
        {
            m_Image.gameObject.SetActive(false);
            return;
        }

        if (m_Image != null)
        {
            m_Image.gameObject.SetActive(true);
            m_Image.sprite = ResourceManager.Instance.GetSprite(str);
            return;
        }
    }

    public void SetColor(Color color)
    {
        m_text.color = color;
    }

    public void SetName(string str)
    {
        if(str == null)
        {
            gameObject.SetActive(false);
            return;
        }

        m_text.text = str;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
