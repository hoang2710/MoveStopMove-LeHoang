using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    private float timer;
    private bool isAttack;

    Quaternion tempRotation;
    public AIStateId GetId()
    {
        return AIStateId.AttackState;
    }
    public void Enter(AIAgent agent)
    {
        timer = 0;
        isAttack = false;
        agent.NavAgent.enabled = false;

        StartAttack(agent);
    }
    public void Exit(AIAgent agent)
    {
        agent.NavAgent.enabled = true;
        agent.enemyRef.WeaponPlaceHolder.SetActive(true);
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

        AttackHandler(agent);
    }
    private void AttackHandler(AIAgent agent)
    {
        if (timer > agent.enemyRef.AttackAnimEnd)
        {
            agent.enemyRef.WeaponPlaceHolder.SetActive(true);
        }
        else if (timer > agent.enemyRef.AttackAnimThrow && !isAttack)
        {
            agent.enemyRef.WeaponPlaceHolder.SetActive(false);

            GameObject obj = ItemStorage.Instance.PopWeaponFromPool(agent.enemyRef.WeaponTag,
                                                                    agent.enemyRef.WeaponSkinTag,
                                                                    agent.enemyRef.AttackPos.position,
                                                                    tempRotation * agent.enemyRef.WeaponRotation);
            Weapon weapon = obj.GetComponent<Weapon>();

            weapon.SetFlyDir(agent.enemyRef.AttackPos.forward);
            weapon.SetBulletOwner(agent.enemyRef);
            weapon.CalculateLifeTime();

            isAttack = true;
        }
    }
    private void StartAttack(AIAgent agent)
    {
        agent.enemyRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_ATTACK);

        Vector3 lookDir = agent.enemyRef.AttackTargetTrans.position - agent.enemyRef.CharaterTrans.position;
        lookDir.y = 0;

        tempRotation = Quaternion.LookRotation(lookDir);
        agent.enemyRef.CharaterTrans.rotation = tempRotation;
    }
}
