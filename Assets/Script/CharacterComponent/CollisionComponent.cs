using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionComponent : MonoBehaviour
{
    CharacterBase m_myCharacter;

    private void Awake()
    {
        m_myCharacter = GetComponent<CharacterBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_myCharacter.gameObject) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactive") )
        {
            IInteractableObject interactObject = other.GetComponent<IInteractableObject>();
            if (interactObject == null)
            {
                return;
            }

            interactObject.OnEnterCollision(m_myCharacter, m_myCharacter.transform.position);
        }
    }



}
