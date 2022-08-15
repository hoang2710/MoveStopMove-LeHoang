using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIState
{
    private Vector3 patrolTargetPos;
    private float patrolRange = ConstValues.VALUE_AI_PATROL_RANGE;

    public AIStateId GetId()
    {
        return AIStateId.PatrolState;
    }
    public void Enter(AIAgent agent)
    {
        agent.enemyRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_RUN);
        if (!GetRandomPos(agent.enemyRef.CharaterTrans.position, patrolRange, out patrolTargetPos))
        {
            agent.stateMachine.ChangeState(AIStateId.PatrolState);
        }

        agent.NavAgent.SetDestination(patrolTargetPos);
    }
    public void Exit(AIAgent agent)
    {

    }
    public void Update(AIAgent agent)
    {
        //NOTE: Change to Idle state if reach destination
        float dist = (patrolTargetPos - agent.enemyRef.CharaterTrans.position).sqrMagnitude;
        if (agent.NavAgent.velocity.sqrMagnitude < 0.01f && dist < ConstValues.VALUE_AI_STOP_DIST_THRESHOLD)
        {
            agent.stateMachine.ChangeState(AIStateId.IdleState);
        }

        if (agent.enemyRef.DetectTarget())
        {
            agent.stateMachine.ChangeState(AIStateId.AttackState);
        }
    }

    private bool GetRandomPos(Vector3 center, float range, out Vector3 result)
    {
        int numnerOfTries = 30;
        for (int i = 0; i < numnerOfTries; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position; Debug.DrawRay(hit.position, Vector3.up * 10f, Color.cyan, 5f);
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}


