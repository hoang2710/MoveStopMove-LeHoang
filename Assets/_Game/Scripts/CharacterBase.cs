using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    protected WeaponType weaponTag;
    [SerializeField]
    protected WeaponSkinType weaponSkinTag;
    [SerializeField]
    protected PantSkinType pantSkinTag;

    public int Score { get; protected set; }
    public int KillScore { get; protected set; }
    public float AttackRange { get; protected set; }
    public float AttackRate { get; protected set; }

    public Transform charaterTrans;
    public Animator anim;

    public Transform AttackPos;
    public Transform AttackTarget;

    private void Awake()
    {
        Score = 0;
        KillScore = 0;
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRate = ConstValues.VALUE_BASE_ATTACK_RATE;
    }
    protected virtual void Start()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }
    protected virtual void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }
    protected virtual void GameManagerOnGameStateChange(GameState state)
    {

    }

    public void SetWeaponType(WeaponType tag)
    {
        weaponTag = tag;
    }
    public void SetWeaponSkin(WeaponSkinType tag)
    {
        weaponSkinTag = tag;
    }
    public void SetPantSkin(PantSkinType tag)
    {
        pantSkinTag = tag;
    }

}
