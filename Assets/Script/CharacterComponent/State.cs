using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State
{
    public CharacterBase m_Character { get; protected set; }
    public E_CHARACTER_STATE state { get; protected set; }

    public System.Action<CharacterBase> StartAction;
    public System.Action UpdateAction;
    public System.Action EndAction;

    protected Ray ray;
    protected RaycastHit hitRay;

    public E_CHARACTER_STATE m_nextState = E_CHARACTER_STATE.NONE;

    public virtual void StartState(CharacterBase character)
    {
        Debug.Log(state.ToString() + " start");
        m_Character = character;
    }
    public virtual void UpdateState()
    {
    }
    public virtual void EndState()
    {
    }
}

#region Idle
public class IdleState : State
{
    public IdleState()
    {
        state = E_CHARACTER_STATE.IDLE;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);
        m_Character.ChangeAniamtion("Idle");
    }
}
#endregion

#region Run
public class RunState : State
{
    public RunState()
    {
        state = E_CHARACTER_STATE.RUN;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);
        m_Character.ChangeAniamtion("Run");
    }
}
#endregion

#region RunToDest
public class RunToDestState : State
{
    public RunToDestState()
    {
        state = E_CHARACTER_STATE.RUN_DEST;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);
        m_Character.ChangeAniamtion("Run");
    }

    public override void EndState()
    {
        m_Character.StopNav();
    }
}
#endregion

#region Dead
public class DeadState : State
{
    public DeadState()
    {
        state = E_CHARACTER_STATE.DEAD;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);
        m_Character.ChangeAniamtion("Dead");
        EffectManager.Instance.PlayEffect(E_EFFECT.DeadEffect, m_Character.transform.position + new Vector3(0,0.15f, 0), Quaternion.identity, true, 2);
    }
    public override void UpdateState()
    {
        if(m_Character.m_isMyCharacter)
            UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.DEAD);
    }

}
#endregion

#region Jump
public class JumpState : State
{
    float currentJumpForce = 0.0f;
    GameObject effect;
    float time = 0.0f;
    RaycastHit hit;

    public JumpState()
    {
        state = E_CHARACTER_STATE.JUMP;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);
        m_Character.ChangeAniamtion("Jump");
        time = 0.0f;
        currentJumpForce = m_Character.m_JumpPower;

        effect = EffectManager.Instance.PlayEffect(E_EFFECT.RunEffect, m_Character.transform.position, Quaternion.identity, false, 1, m_Character.transform);
    }

    public override void UpdateState()
    {
        InputManager.Instance.UpdateInput();
        currentJumpForce -= Time.deltaTime * 8.50f;
        m_Character.transform.position = m_Character.transform.position + (m_Character.transform.forward * m_Character.m_JumpSpeed + m_Character.transform.up * currentJumpForce) * Time.deltaTime;

        time += Time.deltaTime;

        if (m_Character.CheckLayer(LayerMask.GetMask("Ground"), out hitRay, 1.0f))
        {
            m_Character.ChangeState(E_CHARACTER_STATE.RUN);
        }
        else if (m_Character.CheckLayer(LayerMask.GetMask("Dead"), out hitRay))
        {
            m_Character.ChangeState(E_CHARACTER_STATE.DEAD);
        }
        else if (m_Character.CheckLayer(LayerMask.GetMask("GroundItem"), out hitRay))
        {
            m_Character.ChangeState(E_CHARACTER_STATE.SPRINT);
        }
        else if (m_Character.CheckLayer(LayerMask.GetMask("Victory"), out hitRay, 1.0f))
        {
            m_Character.ChangeState(E_CHARACTER_STATE.VICTORY);
        }
        else if(m_Character.CheckForwardLayer(LayerMask.GetMask("GroundItem") | LayerMask.GetMask("Victory") | LayerMask.GetMask("Ground"), out hitRay, 0.26f))
        {
            m_Character.ChangeState(E_CHARACTER_STATE.CLIMB);
        }
    }

    public override void EndState()
    {
        if (m_nextState != E_CHARACTER_STATE.CLIMB)
        {
            Vector3 pos = m_Character.transform.position;
            pos.y = hitRay.point.y;
            m_Character.transform.position = pos;
        }
        else
        {
            Vector3 pos = m_Character.transform.position;
            pos.y = hitRay.point.y - 0.90f;
            m_Character.transform.position = pos + m_Character.transform.forward * 0.15f;
        }

        EffectManager.Instance.StopEffect(effect.GetComponent<EffectObject>());
        //Debug.Log(state.ToString() + " Ended");
    }
}
#endregion

#region Sprint
public class SprintState : State
{
    GameObject effect;
    float time = 0.0f;
    RaycastHit hit;

    public SprintState()
    {
        state = E_CHARACTER_STATE.SPRINT;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);
        m_Character.ChangeAniamtion("Run");
       
        effect = EffectManager.Instance.PlayEffect(E_EFFECT.RunEffect, m_Character.transform.position, Quaternion.identity, false, 1, m_Character.transform);
        time = 0.1f;
    }

    public override void UpdateState()
    {
        InputManager.Instance.UpdateInput();
        m_Character.transform.position += (m_Character.transform.forward * Time.deltaTime * m_Character.m_RunSpeed);

        time += Time.deltaTime;

        if (m_Character.CheckAllLayer(LayerMask.GetMask("Victory"), out hitRay, 1.5f))
        {
            m_Character.ChangeState(E_CHARACTER_STATE.VICTORY);
        }
        else if (m_Character.CheckAllLayer(LayerMask.GetMask("Ground"), out hitRay, 1.5f))
        {
            m_Character.ChangeState(E_CHARACTER_STATE.RUN);
        }
        else if (time > (0.10f))
        {
            if (!m_Character.CheckLayer(LayerMask.GetMask("GroundItem"), out hitRay))
            {
                IInteractableObject ob = m_Character.PopFromStackObject();

                if (ob == null)
                {
                    m_Character.ChangeState(E_CHARACTER_STATE.JUMP);
                }
            }
            else
            {
                Vector3 pos = m_Character.transform.position;
                pos.y = hitRay.point.y + 0.03f;
                m_Character.transform.position = pos;
            }

            time = 0.0f;
        }
    }

    public override void EndState()
    {
        EffectManager.Instance.StopEffect(effect.GetComponent<EffectObject>());
        time = 0.00f;
        Debug.Log(state.ToString() + " Ended");
    }
}
#endregion

#region Dancing
public class VictoryState : State
{
    Vector3 dest;
    public VictoryState()
    {
        state = E_CHARACTER_STATE.VICTORY;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);

        CharacterManager.Instance.GoalCharacter(m_Character);

        GameObject rank = GameObject.Find($"Rank_{CharacterManager.Instance.m_ListGoalCharacter.Count}");
        dest = rank.transform.position;

        m_Character.StartNav(dest);
    }

    public override void UpdateState()
    {
        Quaternion quat = Quaternion.identity;


        if ((m_Character.transform.position - dest).sqrMagnitude <= 0.11f)
        {
            if (m_Character.m_isMyCharacter && m_Character.OnVictory != null)
            { 
                GameMaster.Instance.OnGameGoal();
            }

            m_Character.OnVictory?.Invoke();
            m_Character.OnVictory = null;

            if (m_Character.Rank == 0)
            {
                m_Character.ChangeAniamtion("Dancing");
            }
            else
            {
                m_Character.ChangeAniamtion("Defeat");
            }

            Vector3 camPos = CameraManager.Instance.m_MainCamera.transform.position;
            camPos.y = m_Character.transform.position.y;
            quat = Quaternion.LookRotation((camPos - m_Character.transform.position).normalized, Vector3.up);
            m_Character.transform.rotation = Quaternion.Slerp(m_Character.transform.rotation, quat, Time.deltaTime * 2.0f);
        }
    }

    public override void EndState()
    {
        Debug.Log(state.ToString() + " Ended");
    }
}
#endregion


#region Climb
public class ClimbState : State
{
    public ClimbState()
    {
        state = E_CHARACTER_STATE.CLIMB;
    }

    public override void StartState(CharacterBase character)
    {
        base.StartState(character);
        m_Character.ChangeAniamtion("Climbing");
    }

    public override void UpdateState()
    {
       if( m_Character.GetAnimationPlayRatio() > 0.985f)
       {
            m_Character.ChangeState(E_CHARACTER_STATE.RUN);
       }
    }

    public override void EndState()
    {
        Vector3 pos = m_Character.transform.position;
        pos.y += 0.90f;
        m_Character.transform.position = pos + m_Character.transform.forward * 0.04f;

        Debug.Log(state.ToString() + " Ended");
    }
}
#endregion


