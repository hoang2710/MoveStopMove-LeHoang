using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    private float idleTime = ConstValues.VALUE_AI_IDLE_MAX_TIME;
    private float timer;
    public AIStateId GetId()
    {
        return AIStateId.IdleState;
    }
    public void Enter(AIAgent agent)
    {
        agent.NavAgent.enabled = false;

        agent.enemyRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);

        timer = 0;
    }
    public void Exit(AIAgent agent)
    {
        agent.NavAgent.enabled = true;
    }
    public void Update(AIAgent agent)
    {
        if (agent.enemyRef.IsMovable)
        {
            if (timer > idleTime)
            {
                agent.stateMachine.ChangeState(AIStateId.PatrolState);
            }
            else
            {
                timer += Time.deltaTime;
            }

            if (agent.enemyRef.DetectTarget())
            {
                agent.stateMachine.ChangeState(AIStateId.AttackState);
            }
        }
    }
}
