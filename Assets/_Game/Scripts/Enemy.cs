using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase, IPoolCharacter, IHit
{
    public AIAgent agent;
    public bool IsMovable { get; private set; }
    public GameObject BotGameObject;

    public void OnSpawn()
    {
        agent.stateMachine.ChangeState(AIStateId.IdleState);

        IsAlive = true;
        CharacterCollider.enabled = true;
        CharaterTrans.localScale = Vector3.one;
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;

        AttackTarget = null;
        AttackTargetTrans = null;

        SetRandomEnumData();
        ResetScore();
        SetUpHandWeapon();
        SetUpPantSkin();
        SetRandomBodySkin();
        SetRandomName();

        if (GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            DisplayCharacterUI();
        }
        else
        {
            RemoveCharacterUI();
        }
    }
    public void OnDespawn()
    {
        RemoveCharacterUI();
    }
    protected override void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                if (BotGameObject.activeInHierarchy)
                {
                    DisplayCharacterUI();
                }
                IsMovable = true;
                break;
            case GameState.Pause:
                IsMovable = false;
                break;
            case GameState.LoadLevel:
                BotPooling.Instance.PushBotToPool(BotGameObject);
                break;
            case GameState.ResultPhase:
                IsMovable = false;
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
    public void SetRandomBodySkin()
    {
        CharacterRenderer.material = ItemStorage.Instance.GetRandomBotMaterial();
    }
    public void SetRandomName()
    {
        CharacterName = ItemStorage.Instance.GetRandomBotName();
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
