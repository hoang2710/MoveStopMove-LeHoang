using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase, IHit
{
    public static Vector3 MoveDir;
    [SerializeField]
    private float moveSpeed = 1.5f;
    [SerializeField]
    private float rotateSpeed = 5f;

    private bool isAttackable = true;
    private bool isAttack;
    private bool isDead;

    private float timer = 0;

    public static bool isShop;

    private void FixedUpdate()
    {
        LogicHandle();
        if (Input.GetKey(KeyCode.G))
        {
            SetUpHandWeapon();
        }
    }
    protected override void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.LoadGame:
                LoadDataFromPlayerPrefs();
                break;
            case GameState.LoadLevel:
                SetUpHandWeapon();
                SetUpPantSkin();
                break;
            case GameState.MainMenu:
                if (isShop)
                {
                    LoadDataFromPlayerPrefs();
                    SetUpHandWeapon();
                    SetUpPantSkin();
                }
                break;
            default:
                break;
        }
    }
    private void LogicHandle()
    {
        if (!isDead)
        {
            if (MoveDir.sqrMagnitude > 0.01f)
            {
                Move();
                DetectEnemy();
            }
            else
            {
                if (AttackTarget != null && isAttackable)
                {
                    Attack();
                }
                else
                {
                    Idle();
                    DetectEnemy();
                }

                timer += Time.deltaTime;
            }
        }
    }
    private void Move()
    {
        charaterTrans.position = Vector3.MoveTowards(charaterTrans.position, charaterTrans.position + MoveDir, moveSpeed * Time.deltaTime);
        SetCharacterRotation();

        anim.SetTrigger(ConstValues.ANIM_TRIGGER_RUN);

        isAttackable = true;
        timer = 0;
        WeaponPlaceHolder.SetActive(true);
    }
    private void Idle()
    {
        if (isAttack)
        {
            if (timer >= attackAnimEnd)
            {
                isAttack = false;
                WeaponPlaceHolder.SetActive(true);
            }
        }
        else
        {
            anim.SetTrigger(ConstValues.ANIM_TRIGGER_IDLE);
            timer = 0;
        }
    }
    private void Attack()
    {
        anim.SetTrigger(ConstValues.ANIM_TRIGGER_ATTACK);

        Vector3 lookDir = AttackTarget.position - charaterTrans.position;
        lookDir.y = 0;

        Quaternion tempRotation = Quaternion.LookRotation(lookDir);
        charaterTrans.rotation = tempRotation;

        if (timer > attackAnimThrow)
        {
            WeaponPlaceHolder.SetActive(false);

            GameObject obj = ItemStorage.Instance.PopWeaponFromPool(weaponTag, weaponSkinTag, AttackPos.position, tempRotation * weaponRotation);
            Weapon weapon = obj.GetComponent<Weapon>();

            weapon.SetFlyDir(AttackPos.forward);

            isAttackable = false;
            isAttack = true;
        }
    }
    private void Die()
    {
        isDead = true;
        anim.SetTrigger(ConstValues.ANIM_TRIGGER_DEAD);

        GameManager.Instance.ChangeGameState(GameState.ResultPhase);
    }
    private void DetectEnemy()
    {
        if (AttackTarget == null)
        {
            Collider[] objs = Physics.OverlapSphere(charaterTrans.position, AttackRange, ConstValues.LAYER_MASK_ENEMY);

            if (objs.Length > 0)
            {
                float minDistSqr = float.MaxValue;
                foreach (var item in objs)
                {
                    float distSqr = (item.transform.position - charaterTrans.position).sqrMagnitude;

                    if (distSqr < minDistSqr)
                    {
                        minDistSqr = distSqr;
                        AttackTarget = item.transform;
                    }
                }
            }
        }
    }
    public void OnHit()
    {
        Die();
    }
    private void SetCharacterRotation()
    {
        float tmp = Mathf.Atan2(MoveDir.x, MoveDir.z) * Mathf.Rad2Deg;
        charaterTrans.rotation = Quaternion.Lerp(charaterTrans.rotation, Quaternion.Euler(0, tmp, 0), Time.deltaTime * rotateSpeed);
    }
    private void LoadDataFromPlayerPrefs()
    {
        weaponTag = (WeaponType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_TAG);
        weaponSkinTag = (WeaponSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_SKIN_TAG);
        pantSkinTag = (PantSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_PANT_SKIN_TAG);
    }
}
