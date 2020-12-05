using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour, IControllerObserver
{
    public TouchPhase keyCode { get; set; }
    private CharacterBase m_chracterBase;

    float m_rotateRate;

    Vector3 prePos;

    public void Init(CharacterBase character)
    {
        m_chracterBase = character;
        m_rotateRate = 1.2f;

        if(m_chracterBase.m_isMyCharacter)
            InputManager.Instance.AddObserver(TouchPhase.Moved, this);
        else
            InputManager.Instance.RemoveObserver(TouchPhase.Moved, this);
    }

    void Update()
    {

#if UNITY_EDITOR_WIN // 디버그용ㅋ
        if (m_chracterBase == null) return;

        if (!m_chracterBase.m_isMyCharacter) return;

        Vector2 touchPos;

        if (Input.GetMouseButton(0))
        {
            touchPos = Input.mousePosition - prePos;
            m_chracterBase.transform.Rotate(new Vector3(0, touchPos.x, 0) * m_rotateRate * 5 * Time.deltaTime);

            prePos = Input.mousePosition;
        }
#endif
    }

    public void DestroyComponent()
    {
        InputManager.Instance.RemoveObserver(TouchPhase.Moved, this);
    }

    public void OnMoved(Touch touch, CharacterBase character)
    {
        if (m_chracterBase == null) return;

        Vector2 touchPos;
        touchPos = touch.deltaPosition;

        m_chracterBase.transform.Rotate(new Vector3(0, touchPos.x, 0) * m_rotateRate * Time.deltaTime);
    }

    public void OnTouchStationary(Touch touch, CharacterBase character)
    {

    }

    public void OnTouchBegan(Touch touch, CharacterBase character)
    {

    }

    public void OnTouchEnded(Touch touch ,CharacterBase character)
    {

    }


}
