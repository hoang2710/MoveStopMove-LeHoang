using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateId initState;
    public NavMeshAgent NavAgent;
    public Enemy enemyRef;
    public AIStateId CurState; //NOTE: for debug
    public AIStateId PrevState; //NOTE: for debug

    private void Awake()
    {
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIPatrolState());
        stateMachine.ChangeState(initState); //TODO: understand why this line prevent error auto update in all State Scrpit

        NavMeshAgentSetting();
    }
    private void Update()
    {
        stateMachine.Update();

        UpdateRotation();
    }
    protected virtual void NavMeshAgentSetting()
    {
        NavAgent.autoBraking = false;
        NavAgent.updateRotation = false;
    }
    protected void UpdateRotation()
    {
        Vector3 velocity = NavAgent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            enemyRef.CharaterTrans.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
