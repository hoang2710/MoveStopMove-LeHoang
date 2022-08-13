using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    private float timer;
    private bool isAttack;
    public AIStateId GetId()
    {
        return AIStateId.AttackState;
    }
    public void Enter(AIAgent agent)
    {
        timer = 0;
        isAttack = false;
        agent.NavAgent.enabled = false;
    }
    public void Exit(AIAgent agent)
    {
        agent.NavAgent.enabled = true;
    }
    public void Update(AIAgent agent)
    {
        if (timer > agent.enemyRef.AttackRate)
        {
            agent.stateMachine.ChangeState(AIStateId.IdleState);
        }
        else
        {
            timer += Time.deltaTime;
        }
        if (!isAttack)
        {
            Attack(agent);
        }
    }
    private void Attack(AIAgent agent)
    {
        agent.enemyRef.Anim.SetTrigger(ConstValues.ANIM_TRIGGER_ATTACK);

        Vector3 lookDir = agent.enemyRef.AttackTargetTrans.position - agent.enemyRef.CharaterTrans.position;
        lookDir.y = 0;

        Quaternion tempRotation = Quaternion.LookRotation(lookDir);
        agent.enemyRef.CharaterTrans.rotation = tempRotation;

        if (timer > agent.enemyRef.AttackAnimThrow)
        {
            agent.enemyRef.WeaponPlaceHolder.SetActive(false);

            GameObject obj = ItemStorage.Instance.PopWeaponFromPool(agent.enemyRef.WeaponTag,
                                                                    agent.enemyRef.WeaponSkinTag,
                                                                    agent.enemyRef.AttackPos.position,
                                                                    tempRotation * agent.enemyRef.WeaponRotation);
            Weapon weapon = obj.GetComponent<Weapon>();

            weapon.SetFlyDir(agent.enemyRef.AttackPos.forward);

            isAttack = true;
        }
    }
}
