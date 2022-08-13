using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharacterBase, IPoolCharacter, IHit
{
    public AIAgent agent;

    public void OnInit()
    {
        agent.stateMachine.ChangeState(AIStateId.IdleState);
        IsAlive = true;
        CharacterCollider.enabled = true;

        SetRandomEnumData();
        ResetScore();
        SetUpHandWeapon();
        SetUpPantSkin();
    }
    public void OnDespawn()
    {

    }
    protected override void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                agent.stateMachine.ChangeState(AIStateId.IdleState);
                break;
            case GameState.Playing:
                agent.stateMachine.ChangeState(AIStateId.PatrolState);
                break;
            case GameState.LoadLevel: //NOTE: testing 
                OnInit();
                break;
            default:
                break;
        }
    }
    private void SetRandomEnumData()
    {
        WeaponTag = (WeaponType)Random.Range(0, System.Enum.GetNames(typeof(WeaponType)).Length);

        //NOTE: Optimize later, dont rush
        switch (WeaponTag)
        {
            case WeaponType.Axe:
                WeaponSkinTag = (WeaponSkinType)Random.Range(0, 4);
                break;
            case WeaponType.Knife:
                WeaponSkinTag = (WeaponSkinType)Random.Range(8, 10);
                break;
            case WeaponType.Candy:
                WeaponSkinTag = (WeaponSkinType)Random.Range(6, 8);
                break;
            case WeaponType.Hammer:
                WeaponSkinTag = (WeaponSkinType)Random.Range(4, 6);
                break;
            default:
                break;
        }

        PantSkinTag = (PantSkinType)Random.Range(0, System.Enum.GetNames(typeof(PantSkinType)).Length);
    }

    private void ResetScore()
    {
        Score = 0;
        KillScore = 0;
    }

    public void OnHit()
    {
        agent.stateMachine.ChangeState(AIStateId.DeathState);
        IsAlive = false;
        CharacterCollider.enabled = false;
    }
}
