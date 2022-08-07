using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : CharacterBase, IPoolCharacter
{
    public NavMeshAgent agent;

    private void Awake()
    {
        NavMeshAgentSetting();
    }
    public void OnInit()
    {
        SetRandomEnumData();
        ResetScore();
    }
    public void OnDespawn()
    {
        throw new System.NotImplementedException();
    }
    private void SetRandomEnumData()
    {
        weaponTag = (WeaponType)Random.Range(0, System.Enum.GetNames(typeof(WeaponType)).Length);
        weaponSkinTag = (WeaponSkinType)Random.Range(0, System.Enum.GetNames(typeof(WeaponSkinType)).Length);
        pantSkinTag = (PantSkinType)Random.Range(0, System.Enum.GetNames(typeof(PantSkinType)).Length);
    }
    private void ResetScore()
    {
        score = 0;
        killScore = 0;
    }
    protected void NavMeshAgentSetting()
    {
        agent.autoBraking = false;
        agent.updateRotation = false;
    }
    protected void UpdateRotation()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            charaterTrans.rotation = Quaternion.LookRotation(velocity);
        }
    }

}
