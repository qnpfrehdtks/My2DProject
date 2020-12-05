using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    bool m_isPauseObserve;

    Dictionary<TouchPhase, List<IControllerObserver>> m_DicObservers =
    new Dictionary<TouchPhase, List<IControllerObserver>>();

    Dictionary<TouchPhase, IControllerObserver> m_DicObserverTouchPad =
    new Dictionary<TouchPhase, IControllerObserver>();

    public override void InitializeManager()
    {
    }

    public bool UpdateInput()
    {
        if (m_DicObservers.Count <= 0)
            return false;

        if (Input.touchCount == 0) return false;

        Touch touch = Input.GetTouch(0);
        TouchEvent(touch, CharacterManager.Instance.m_CurrentMyCharacter);

        return false;
    }


    public void RemoveObserver(TouchPhase touchPhase, IControllerObserver observer)
    {
        List<IControllerObserver> list;

        if (m_DicObservers.TryGetValue(touchPhase, out list))
        {
            list.Remove(observer);
        }
    }

    public void AddObserver(TouchPhase touchPhase, IControllerObserver observer)
    {
        List<IControllerObserver> list;

        if (m_DicObservers.TryGetValue(touchPhase, out list))
        {
            list.Add(observer);
        }
        else
        {
            list = new List<IControllerObserver>();
            list.Add(observer);
            m_DicObservers.Add(touchPhase, list);
        }
    }

    private void TouchEvent(Touch touch, CharacterBase character)
    {
        if(!character.m_isMyCharacter)
        {
            return;
        }

        List<IControllerObserver> list;

        if (m_DicObservers.TryGetValue(touch.phase, out list))
        {
            foreach (var obs in list)
            {
                switch(touch.phase)
                {
                    case TouchPhase.Moved:
                        obs.OnMoved(touch, character);
                        break;
                    case TouchPhase.Began:
                        obs.OnTouchBegan(touch, character);
                        break;
                    case TouchPhase.Stationary:
                        obs.OnTouchStationary(touch, character);
                        break;
                    case TouchPhase.Ended:
                        obs.OnTouchEnded(touch, character);
                        break;
                }
            }
        }
    }


}
