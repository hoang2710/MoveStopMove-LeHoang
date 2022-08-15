using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.DeathState;
    }
    public void Enter(AIAgent agent)
    {
        agent.NavAgent.enabled = false;
        agent.enemyRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_DEAD);
    }
    public void Exit(AIAgent agent)
    {
        agent.NavAgent.enabled = true;
    }
    public void Update(AIAgent agent)
    {
    }
}
