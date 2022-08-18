using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    private float deadTime = ConstValues.VALUE_BOT_DEAD_TIME;
    private float timer;
    public AIStateId GetId()
    {
        return AIStateId.DeathState;
    }
    public void Enter(AIAgent agent)
    {
        agent.NavAgent.enabled = false;
        agent.enemyRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_DEAD);

        timer = 0;
    }
    public void Exit(AIAgent agent)
    {
        agent.NavAgent.enabled = true;
    }
    public void Update(AIAgent agent)
    {
        if (timer >= deadTime)
        {
            BotPooling.Instance.PushBotToPool(agent.enemyRef.BotGameObject);
            LevelManager.Instance.SpawnBotRandomPos();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
