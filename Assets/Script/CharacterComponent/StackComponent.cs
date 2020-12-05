using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackComponent : MonoBehaviour
{
    Stack<IInteractableObject> m_stackItem = new Stack<IInteractableObject>();
    CharacterBase m_myCharacter;
    
    public int StackCount
    {
        get
        {
            return m_stackItem.Count;
        }
    }

    public IInteractableObject Top
    {
        get
        {
            if (m_stackItem.Count == 0)
                return null;

            return m_stackItem.Peek();
        }
    }

    public void Init(CharacterBase character)
    {
        AllStackPushToPool();
        m_myCharacter = character;
    }

    public void AllStackPushToPool()
    {
        while (m_stackItem.Count > 0)
        {
            MonoBehaviour mono = m_stackItem.Pop() as MonoBehaviour;

            if(mono == null)
            {
                continue;
            }

            GameObject go = mono.gameObject;
            PoolingManager.Instance.PushToPool(go);
        }
    }

    public void OffFade()
    {
        IInteractableObject[] arr = m_stackItem.ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            InteractObject obj = arr[i] as InteractObject;

            if (obj == null)
                continue;

            obj.ExitCollisionToCameraRay();
        }
    }

    public void OnFade()
    {
        IInteractableObject[] arr = m_stackItem.ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            InteractObject obj = arr[i] as InteractObject;

            if (obj == null)
                continue;

            obj.EnterCollisionToCameraRay();
        }
    }


    public void OnVictory()
    {
        IInteractableObject[] arr = m_stackItem.ToArray();

        for(int i = 0; i < arr.Length; i++)
        {
            InteractObject obj = arr[i] as InteractObject;

            if (obj == null)
                continue;

            obj.FlyAway();
        }
    }

    public void PushToStackObject(IInteractableObject interactItem)
    {
        m_stackItem.Push(interactItem);
    }

    public IInteractableObject PopFromStackObject()
    {
        if (m_stackItem.Count > 0)
        {
            IInteractableObject interactObject = m_stackItem.Pop();

            InteractObject obj = interactObject as InteractObject;
            obj.OnPopFromStack();

            return interactObject;
        }

        return null;
    }

}
