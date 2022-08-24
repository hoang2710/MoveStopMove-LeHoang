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

    [HideInInspector]
    public string CharacterName { get; protected set; }
    public int Score { get; protected set; }
    public int KillScore { get; protected set; }
    public float AttackRange { get; protected set; }
    public float AttackRate { get; protected set; }

    public Transform CharaterTrans;
    public Collider CharacterCollider;

    public Animator Anim;
    protected string curAnim = ConstValues.ANIM_TRIGGER_IDLE;

    public Transform AttackPos;
    [HideInInspector]
    public Transform AttackTargetTrans;
    [HideInInspector]
    public CharacterBase AttackTarget;
    private float minorOffset = 1.1f; //NOTE: prevent targetmark blinking due to detect and un-detect at the same time
    private float detectOffSetDistance = 2f;

    public bool IsAlive { get; protected set; }
    protected bool isPlayer; //NOTE: use for ui display. moveUI
    protected bool isWeaponTripleShot;
    protected float weaponTripleShotOffset; //NOTE: y axis in quaternion

    public Quaternion ThrowWeaponRotation { get; protected set; }
    public float AttackAnimThrow { get; protected set; }
    public float AttackAnimEnd { get; protected set; }

    public GameObject WeaponPlaceHolder;
    [HideInInspector]
    public Transform WeaponPlaceHolderTrans;
    protected GameObject handWeapon;
    protected WeaponType currentHandWeaponTag;
    public Renderer CharacterRenderer;
    public Renderer PantRenderer;
    public Transform CharacterUITransRoot;
    [HideInInspector]
    public CharacterInfoDIsplay currentUIDisplay;

    protected virtual void Awake()
    {
        CharacterName = ConstValues.VALUE_CHARACTER_DEFAULT_NAME;
        Score = 0;
        KillScore = 0;
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRate = ConstValues.VALUE_BASE_ATTACK_RATE;

        ThrowWeaponRotation = Quaternion.Euler(-90f, 0, 90f); //NOTE: default value

        AttackAnimThrow = ConstValues.VALUE_PLAYER_ATTACK_ANIM_THROW_TIME_POINT;
        AttackAnimEnd = ConstValues.VALUE_PLAYER_ATTACK_ANIM_END_TIME_POINT;

        WeaponPlaceHolderTrans = WeaponPlaceHolder.transform;
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
            Collider[] objs = Physics.OverlapSphere(CharaterTrans.position, AttackRange, ConstValues.LAYER_MASK_ENEMY);

            if (objs.Length > 1)
            {


                float minDistSqr = float.MaxValue;
                Collider bestMatch = objs[0];
                foreach (var item in objs)
                {
                    float distSqr = (item.transform.position - CharaterTrans.position).sqrMagnitude; //NOTE: use transform once -> no need to cache

                    if (distSqr < minDistSqr && distSqr > detectOffSetDistance)
                    {
                        minDistSqr = distSqr;
                        bestMatch = item;
                    }
                }
                AttackTargetTrans = bestMatch.transform;
                AttackTarget = AttackTargetTrans.GetComponent<CharacterBase>();

                return true;
            }

            return false; //NOTE: objs.Length = 1 --> detect self
        }
        else
        {
            if (AttackTarget.IsAlive)
            {
                float distSqr = (AttackTargetTrans.position - CharaterTrans.position).sqrMagnitude;
                if (distSqr > AttackRange * AttackRange * minorOffset * minorOffset) //NOTE: optimize later or not
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
        if (isWeaponTripleShot)
        {
            Quaternion leftOffset = curRotation * Quaternion.Euler(0, -weaponTripleShotOffset, 0);
            Quaternion rightOffset = curRotation * Quaternion.Euler(0, weaponTripleShotOffset, 0);

            Shoot(leftOffset);
            Shoot(curRotation);
            Shoot(rightOffset);
        }
        else
        {
            Shoot(curRotation);
        }
    }
    private void Shoot(Quaternion curRotation)
    {
        Vector3 moveDir = curRotation * Vector3.forward;
        GameObject obj = ItemStorage.Instance.PopWeaponFromPool(WeaponTag,
                                                                WeaponSkinTag,
                                                                AttackPos.position,
                                                                curRotation * ThrowWeaponRotation);
        Weapon weapon = obj.GetComponent<Weapon>();
        weapon?.SetUpThrowWeapon(moveDir, this);
    }
    public void SetUpHandWeapon()
    {
        if (handWeapon != null)
        {
            ItemStorage.Instance.PushWeaponToPool(currentHandWeaponTag, handWeapon, true);
        }

        currentHandWeaponTag = WeaponTag;
        handWeapon = ItemStorage.Instance.PopWeaponFromPool(WeaponTag,
                                                            WeaponSkinTag,
                                                            WeaponPlaceHolderTrans,
                                                            Vector3.zero,
                                                            Quaternion.identity);

        Weapon weapon = handWeapon.GetComponent<Weapon>();
        weapon?.SetUpHandWeapon(this);

        Renderer objRen = weapon.WeaponRenderer;
        if (objRen != null)
        {
            Material material = ItemStorage.Instance.GetWeaponSkin(WeaponSkinTag);
            switch (WeaponTag)
            {
                case WeaponType.Candy:
                    objRen.materials = new Material[] { material, material, material }; //NOTE: Candy weapon have 3 material
                    break;
                default:
                    objRen.materials = new Material[] { material, material };
                    break;
            }
        }
    }
    public void ChangeAnimation(string anim)
    {
        if (curAnim != anim)
        {
            Anim.SetTrigger(anim);
            curAnim = anim;
        }
    }
    public virtual void OnKillEnemy()
    {
        CharaterTrans.localScale += ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO * Vector3.one;
        AttackRange += ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO * ConstValues.VALUE_BASE_ATTACK_RANGE;

        currentUIDisplay?.UpdateScore(++Score);//temp score system
        currentUIDisplay?.TriggerPopupScore(1);//temp score system
    }
    public void SetUpThrowWeapon(Quaternion rotation, bool isTripleShot, float tripleShotOffset)
    {
        ThrowWeaponRotation = rotation;
        isWeaponTripleShot = isTripleShot;
        weaponTripleShotOffset = tripleShotOffset;
    }
    public void SetUpPantSkin()
    {
        PantRenderer.material = ItemStorage.Instance.GetPantSkin(PantSkinTag);
    }
    public void DisplayCharacterUI()
    {
        CharacterUIPooling.Instance.PopUIFromPool(this);

        currentUIDisplay?.SetUpUI(CharacterName, CharacterRenderer.material.color, isPlayer);
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

}
