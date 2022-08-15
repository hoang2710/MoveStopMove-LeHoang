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
    }
    public void Exit(AIAgent agent)
    {
        agent.NavAgent.enabled = true;
    }
    public void Update(AIAgent agent)
    {
        agent.enemyRef.ChangeAnimation(ConstValues.ANIM_PLAY_DEAD);
    }
}
