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
        LevelManager.Instance.KillHandle();

        agent.NavAgent.enabled = false;
        agent.enemyRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_DEAD);

        timer = 0;

        //NOTE: temp solution for random enum option
        int ran = Random.Range((int)AudioType.Die1, (int)AudioType.Die4 + 1);
        agent.enemyRef.PlayAudioWithCondition((AudioType)ran);
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
            ParticlePooling.Instance.PopParticleFromPool(ParticleType.Death,
                                                        agent.enemyRef.CharaterTrans.position,
                                                        Quaternion.identity,
                                                        agent.enemyRef);

            agent.enemyRef.PlayAudioWithCondition(AudioType.DieExplode);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
