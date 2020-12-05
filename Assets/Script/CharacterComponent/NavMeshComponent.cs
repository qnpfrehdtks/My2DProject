using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshComponent : MonoBehaviour
{
    CharacterBase m_myCharacter;
    NavMeshAgent m_navAgent;

    public Vector3 m_CurrentDest;
    public bool IsStop
    {
        get
        {
           return m_navAgent.isStopped;
        }
    }

    public bool IsDest
    {
        get
        {
            if (Vector3.Distance(m_navAgent.destination, m_myCharacter.transform.position) < m_navAgent.stoppingDistance)
            {
                m_navAgent.isStopped = true;
                m_navAgent.enabled = false;
                return true;
            }

            return false;
        }
    }


    public void Init(CharacterBase character)
    {
        m_myCharacter = character;
        m_navAgent = Common.GetOrAddComponent<NavMeshAgent>(m_myCharacter.gameObject);
        m_navAgent.speed = m_myCharacter.m_RunSpeed;
        m_navAgent.stoppingDistance = 0.1f;

        StopNav();
    }


    public void StopNav()
    {
        if (m_navAgent.enabled)
        {
            m_navAgent.isStopped = true;
        }

        m_navAgent.enabled = false;
        
    }

    public void SetDestination(Vector3 dest)
    {
        m_CurrentDest = dest;
    }

    public void StartNav()
    {
        m_navAgent.enabled = true;
        m_navAgent.isStopped = false;
        m_navAgent.speed = m_myCharacter.m_RunSpeed;
        m_navAgent.SetDestination(m_CurrentDest);
    }



}
