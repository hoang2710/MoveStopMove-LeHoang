using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBase : MonoBehaviour
{
    public WeaponType WeaponTag { get; protected set; }
    public WeaponSkinType WeaponSkinTag { get; protected set; }
    public PantSkinType PantSkinTag { get; protected set; }
    public HatType HatTag { get; protected set; }

    public string CharacterName { get; protected set; }
    public int Score { get; protected set; }
    public int KillScore { get; protected set; }
    protected int PlayerLevel = 1;
    protected int defaultKillScore = 5;
    protected int scoreStep = 8;
    protected float scoreStepMultipler = 1.1f;
    protected bool isSizeUp;

    public float AttackRange { get; protected set; }
    public float AttackRate { get; protected set; }

    public Transform CharaterTrans;
    public Collider CharacterCollider;

    public Animator Anim;
    protected string curAnim = ConstValues.ANIM_TRIGGER_IDLE;

    public Transform AttackPos;
    [HideInInspector] public Transform AttackTargetTrans;
    [HideInInspector] public CharacterBase AttackTarget;
    [HideInInspector] public float AttackPosOffset = 1f; //NOTE: distance from attack pos to character center, use for calculate weapon life time
    private float minorOffset = 1.1f; //NOTE: prevent targetmark blinking due to detect and un-detect at the same time
    private float detectOffSetDistance = 2f;

    public bool IsAlive { get; protected set; }
    protected bool isPlayer; //NOTE: use for ui display. moveUI

    public Quaternion ThrowWeaponRotation { get; protected set; }
    public float AttackAnimThrow { get; protected set; }
    public float AttackAnimEnd { get; protected set; }

    public GameObject WeaponPlaceHolder;
    public Transform WeaponPlaceHolderTrans;
    public GameObject HatPlaceHolder;
    public Transform HatPLaceHolderTrans;
    protected GameObject handWeapon;
    protected Transform handWeaponTrans;
    protected WeaponType currentHandWeaponTag;
    protected GameObject hatObject;
    protected HatType currentHatTag;
    public Renderer CharacterRenderer;
    public Renderer PantRenderer;
    public Transform CharacterUITransRoot;
    [HideInInspector]
    public CharacterInfoDIsplay currentUIDisplay;
    public bool IsAudioPlayable { get; set; } //NOTE: If character is out screen --> cant play audio, set value in character info display script (temp maybe)

    //NOTE: use for detect enemy
    private Collider[] colliders = new Collider[10];
    private Collider bestMatch;
    private int numofColliderFound;
    private float minDistSqr;
    private float distSqr;

    protected virtual void Awake()
    {
        CharacterName = ConstValues.VALUE_CHARACTER_DEFAULT_NAME;
        Score = 0;
        KillScore = 5;
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRate = ConstValues.VALUE_BASE_ATTACK_RATE;

        ThrowWeaponRotation = Quaternion.Euler(-90f, 0, 90f); //NOTE: default value

        AttackAnimThrow = ConstValues.VALUE_PLAYER_ATTACK_ANIM_THROW_TIME_POINT;
        AttackAnimEnd = ConstValues.VALUE_PLAYER_ATTACK_ANIM_END_TIME_POINT;
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
    public bool DetectTarget()
    {
        if (AttackTargetTrans == null)
        {
            numofColliderFound = Physics.OverlapSphereNonAlloc(CharaterTrans.position, AttackRange, colliders, ConstValues.LAYER_MASK_ENEMY, QueryTriggerInteraction.Ignore);

            if (numofColliderFound > 1)
            {
                minDistSqr = float.MaxValue;
                bestMatch = colliders[0];
                for (int i = 0; i < numofColliderFound; i++)
                {
                    distSqr = (CacheCharTrans.Get(colliders[i]).position - CharaterTrans.position).sqrMagnitude;

                    if (distSqr < minDistSqr && distSqr > detectOffSetDistance)
                    {
                        minDistSqr = distSqr;
                        bestMatch = colliders[i];
                    }
                }
                AttackTarget = CacheCharBAse.Get(bestMatch);
                AttackTargetTrans = AttackTarget.CharaterTrans;

                return true;
            }

            return false; //NOTE: numOfColliderFound = 1 --> detect self
        }
        else
        {
            if (AttackTarget.IsAlive)
            {
                float distSqr = (AttackTargetTrans.position - CharaterTrans.position).sqrMagnitude;
                if (distSqr > AttackRange * AttackRange * minorOffset) //NOTE: optimize later or not
                {
                    AttackTargetTrans = null;
                    return false;
                }
            }
            else
            {
                AttackTargetTrans = null;
                return false;
            }

            return true;
        }
    }
    public void ThrowWeapon(Quaternion curRotation)
    {
        PlayAudioWithCondition(AudioType.ThrowWeapon);

        Vector3 moveDir = curRotation * Vector3.forward;
        Weapon weapon;
        ItemStorage.Instance.PopWeaponFromPool(WeaponTag,
                                               WeaponSkinTag,
                                               AttackPos.position,
                                               curRotation * ThrowWeaponRotation,
                                               out weapon);
        weapon?.SetUpThrowWeapon(moveDir, this);
    }
    public void SetUpHandWeapon()
    {
        if (handWeapon != null)
        {
            ItemStorage.Instance.PushWeaponToPool(currentHandWeaponTag, handWeapon, handWeaponTrans);
        }

        currentHandWeaponTag = WeaponTag;
        Weapon weapon;
        handWeapon = ItemStorage.Instance.PopWeaponFromPool(WeaponTag,
                                                            WeaponSkinTag,
                                                            WeaponPlaceHolderTrans,
                                                            Vector3.zero,
                                                            Quaternion.identity,
                                                            out weapon);
        weapon?.SetUpHandWeapon(this);
        handWeaponTrans = weapon?.WeaponTrans;
    }
    public void SetUpHat()
    {
        if (hatObject != null)
        {
            ItemStorage.Instance.PushHatToPool(currentHatTag, hatObject);
        }

        currentHatTag = HatTag;
        hatObject = ItemStorage.Instance.PopHatFromPool(HatTag, HatPLaceHolderTrans);
    }
    public void ChangeAnimation(string anim)
    {
        if (curAnim != anim)
        {
            Anim.ResetTrigger(curAnim);
            Anim.SetTrigger(anim);
            curAnim = anim;
        }
    }
    public void PlayAudioWithCondition(AudioType audioType)
    {
        if (IsAudioPlayable)
        {
            AudioManager.Instance.PlayAudioClip(audioType);
        }
    }
    public virtual void OnKillEnemy(int gainedScore)
    {
        isSizeUp = CalculateScore(gainedScore);
        if (isSizeUp)
        {
            CharaterTrans.localScale += ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO * Vector3.one;
            AttackRange += ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO * ConstValues.VALUE_BASE_ATTACK_RANGE;
            AttackPosOffset += ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO;

            ParticlePooling.Instance.PopParticleFromPool(ParticleType.Upgrade,
                                                        CharaterTrans.position,
                                                        ConstValues.VALUE_PARTICLE_UPGRADE_DEFAULT_ROTATION,
                                                        this);

            PlayAudioWithCondition(AudioType.SizeUp);
        }
    }
    private bool CalculateScore(int gainedScore)
    {
        Score += gainedScore;
        KillScore = Score / 2;

        currentUIDisplay?.UpdateScore(Score);
        currentUIDisplay?.TriggerPopupScore(gainedScore);

        if (((float)Score / (scoreStep * Mathf.Pow(scoreStepMultipler, (Score / scoreStep)))) > PlayerLevel)
        {
            PlayerLevel++;
            return true;
        }

        return false;
    }
    public void SetUpThrowWeapon(Quaternion rotation)
    {
        ThrowWeaponRotation = rotation;
    }
    public void SetUpPantSkin()
    {
        PantRenderer.material = ItemStorage.Instance.GetPantSkin(PantSkinTag);
    }
    public void DisplayCharacterUI()
    {
        if (currentUIDisplay == null)
        {
            CharacterUIPooling.Instance.PopUIFromPool(this);

            currentUIDisplay?.SetUpUI(CharacterName, CharacterRenderer.material.color, isPlayer);
        }
    }
    public void RemoveCharacterUI()
    {
        if (currentUIDisplay != null)
        {
            CharacterUIPooling.Instance.PushUIToPool(currentUIDisplay.UIObject);
        }
    }
    public void SetWeaponType(WeaponType tag)
    {
        WeaponTag = tag;
    }
    public void SetWeaponSkin(WeaponSkinType tag)
    {
        WeaponSkinTag = tag;
    }
    public void SetPantSkin(PantSkinType tag)
    {
        PantSkinTag = tag;
    }
    public void SetHat(HatType tag)
    {
        HatTag = tag;
    }

}
