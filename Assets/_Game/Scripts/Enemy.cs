using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase, IPoolCharacter, IHit
{
    public AIAgent agent;
    public bool IsMovable { get; private set; } 
    private bool isUIEnable;
    public GameObject BotGameObject;

    public void OnInit()
    {
        agent.stateMachine.ChangeState(AIStateId.IdleState);
        IsAlive = true;
        CharacterCollider.enabled = true;
        CharaterTrans.localScale = Vector3.one;
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;

        SetRandomEnumData();
        ResetScore();
        SetUpHandWeapon();
        SetUpPantSkin();

        if (isUIEnable)
        {
            DisplayCharacterUI();
        }
    }
    public void OnDespawn()
    {
        AttackTarget = null;
        AttackTargetTrans = null;

        RemoveCharacterUI();
    }
    protected override void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            // case GameState.MainMenu:
            //     agent.stateMachine.ChangeState(AIStateId.IdleState);
            //     break;
            case GameState.Playing:
                DisplayCharacterUI();
                IsMovable = true;
                isUIEnable = true;
                break;
            case GameState.Pause:
                IsMovable = false;
                isUIEnable = false;
                break;
            case GameState.LoadLevel:
                BotPooling.Instance.PushBotToPool(BotGameObject);
                break;
            case GameState.ResultPhase:
                IsMovable = false;
                isUIEnable = false;
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

    public void OnHit(CharacterBase bulletOwner)
    {
        agent.stateMachine.ChangeState(AIStateId.DeathState);
        IsAlive = false;
        CharacterCollider.enabled = false;

        RemoveCharacterUI();

        bulletOwner?.OnKillEnemy();
    }
}
