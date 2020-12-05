using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CHARACTER_STATE
{
    NONE,
    IDLE,
    RUN,
    RUN_DEST,
    SPRINT,
    DEAD_JUMP,
    DEAD,
    VICTORY,
    JUMP,
    CLIMB,
    END
}


public class FSMComponent : MonoBehaviour
{
    protected CharacterBase m_myCharacter;
    protected State m_DefaultState;
    protected State m_CurrentState;

    Ray m_ray;
    RaycastHit m_rayCastHit;

    public E_CHARACTER_STATE m_EnumCurrentState { get; set; }

    protected Dictionary<E_CHARACTER_STATE, State> m_dicState = new Dictionary<E_CHARACTER_STATE, State>();
    public System.Action m_StateUpdate;

    protected NavMeshComponent m_NavComponent;
    
        RaycastHit hit;

    public void DestoryComponent()
    {
        ChangeState(m_DefaultState);
        m_dicState.Clear();
        Destroy(this);
    }

    private void Awake()
    {
        m_DefaultState = new IdleState();
        InsertState(m_DefaultState);

        InsertState(new RunState());
        InsertState(new RunToDestState());
        InsertState(new JumpState());
        InsertState(new DeadState());
        InsertState(new SprintState());
        InsertState(new VictoryState());
        InsertState(new ClimbState());

        GetState(E_CHARACTER_STATE.RUN).UpdateAction = UpdateRun;

        GetState(E_CHARACTER_STATE.RUN_DEST).UpdateAction = UpdateRunDest;
        GetState(E_CHARACTER_STATE.RUN_DEST).StartAction = StartRunDest;
    }

    public virtual void Init(CharacterBase myCharacter)
    {
        m_myCharacter = myCharacter;
        m_NavComponent = Common.GetOrAddComponent<NavMeshComponent>(gameObject);
        m_NavComponent.Init(m_myCharacter);

        ChangeState(m_DefaultState);
    }

    public void StartNav(Vector3 pos)
    {
        m_NavComponent.SetDestination(pos);
        ChangeState(E_CHARACTER_STATE.RUN_DEST);
    }

    protected virtual void StartRunDest(CharacterBase character)
    {
        m_NavComponent.StartNav();
    }

    public virtual void UpdateRunDest()
    {
        if (m_NavComponent.IsDest)
        {
            ChangeState(E_CHARACTER_STATE.RUN);
        }
        else
        {
            if (m_myCharacter.CheckLayer(LayerMask.GetMask("Victory"), out m_rayCastHit, 1.5f))
            {
                ChangeState(E_CHARACTER_STATE.VICTORY);
            }
            else if (!m_myCharacter.CheckLayer(LayerMask.GetMask("Ground"), out m_rayCastHit, 1.5f))
            {
                if (m_myCharacter.StackCount <= 0)
                {
                    ChangeState(E_CHARACTER_STATE.JUMP);
                }
                else if (m_myCharacter.StackCount > 0)
                {
                    ChangeState(E_CHARACTER_STATE.SPRINT);
                }
            }
        }
    }

    public virtual void UpdateRun()
    {
        InputManager.Instance.UpdateInput();
        m_myCharacter.transform.position += (m_myCharacter.transform.forward * Time.deltaTime * m_myCharacter.m_RunSpeed);

        if (m_myCharacter.CheckAllLayer(LayerMask.GetMask("Victory"), out m_rayCastHit,1.5f))
        {
            ChangeState(E_CHARACTER_STATE.VICTORY);
        }
        else if (!m_myCharacter.CheckAllLayer(LayerMask.GetMask("Ground"), out m_rayCastHit, 1.5f))
        {
            if (m_myCharacter.StackCount <= 0)
            {
                ChangeState(E_CHARACTER_STATE.JUMP);
            }
            else if (m_myCharacter.StackCount > 0)
            {
                ChangeState(E_CHARACTER_STATE.SPRINT);
            }
        }
    }

    private void Update()
    {
        m_StateUpdate?.Invoke();
    }

    public void StopNav()
    {
        if(m_NavComponent)
            m_NavComponent.StopNav();
    }

    public void InsertState( State state)
    {
        if(!m_dicState.ContainsKey(state.state))
        {
            m_dicState.Add(state.state, state);
        }
    }

    public State GetState(E_CHARACTER_STATE state)
    {
        State st;
        if (m_dicState.TryGetValue(state, out st))
        {
            return st;
        }

        return null;
    }


    void ChangeState(State newState)
    {
        if (newState == null)
            return;

        ChangeState(newState.state);
    }

    public void ChangeState(E_CHARACTER_STATE state)
    {
        if(m_CurrentState != null && m_CurrentState.state == state)
        {
            return;
        }

        if(m_CurrentState != null)
        {
            m_CurrentState.m_nextState = state;
            m_CurrentState.EndState();
            m_CurrentState.EndAction?.Invoke();
        }

        State newState = GetState(state);

        if (newState != null)
        {
            newState.StartState(m_myCharacter);
            newState.StartAction?.Invoke(m_myCharacter);

            m_CurrentState = newState;
            m_StateUpdate = newState.UpdateState;
            m_StateUpdate += newState.UpdateAction;
        }
        else
        {
            m_DefaultState.StartState(m_myCharacter);
            m_DefaultState.StartAction?.Invoke(m_myCharacter);

            m_CurrentState = m_DefaultState;
            m_StateUpdate = m_DefaultState.UpdateState;
            m_StateUpdate += m_DefaultState.UpdateAction;
        }

        m_EnumCurrentState = m_CurrentState.state;
    }

}
