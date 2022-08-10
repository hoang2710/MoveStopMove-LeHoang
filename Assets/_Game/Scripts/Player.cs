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

    public Transform AttackPos;
    public Transform AttackTarget;

    private bool isAttackable;
    private bool isAttack;
    private bool isDead;
    private Quaternion weaponRotation = Quaternion.Euler(-90f, 0, 90f);
    protected float attackAnimThrow = ConstValues.VALUE_PLAYER_ATTACK_ANIM_THROW_TIME_POINT;
    private float attackAnimEnd = ConstValues.VALUE_PLAYER_ATTACK_ANIM_END_TIME_POINT;
    private float timer = 0;
    public GameObject WeaponPlaceHolder;
    //TODO: understand why this shit not (90, -90, 180)
    private Quaternion handWeaponRotation = Quaternion.Euler(-90f, -90f, 180f);

    public static bool isShop;

    private void FixedUpdate()
    {
        if (!isDead)
        {
            LogicHandle();
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
                break;
            case GameState.MainMenu:
                if (isShop)
                {
                    SetUpHandWeapon();
                }
                break;
            default:
                break;
        }
    }
    private void LogicHandle()
    {
        if (MoveDir.sqrMagnitude > 0.01f)
        {
            Move();
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
            }

            timer += Time.deltaTime;
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
        }
    }
    private void Attack()
    {
        anim.SetTrigger(ConstValues.ANIM_TRIGGER_ATTACK);

        Quaternion tempRotation = Quaternion.LookRotation(AttackTarget.position - charaterTrans.position);
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
        anim.SetTrigger(ConstValues.ANIM_TRIGGER_DEAD);

        GameManager.Instance.ChangeGameState(GameState.ResultPhase);
    }
    public void OnHit()
    {
        isDead = true;
        Die();
    }
    private void SetCharacterRotation()
    {
        float tmp = Mathf.Atan2(MoveDir.x, MoveDir.z) * Mathf.Rad2Deg;
        charaterTrans.rotation = Quaternion.Lerp(charaterTrans.rotation, Quaternion.Euler(0, tmp, 0), Time.deltaTime * rotateSpeed);
    }
    private void SetUpHandWeapon()
    {
        Transform WeaponPlaceHolderTrans = WeaponPlaceHolder.transform; Debug.Log(WeaponPlaceHolderTrans.forward);
        GameObject obj = Instantiate(ItemStorage.Instance.GetWeaponType(weaponTag), WeaponPlaceHolderTrans.position, Quaternion.LookRotation(WeaponPlaceHolderTrans.forward) * handWeaponRotation, WeaponPlaceHolderTrans);

        Weapon weapon = obj.GetComponent<Weapon>();
        weapon?.DeactiveWeaponScript();

        Renderer objRen = obj.GetComponent<Renderer>();

        if (objRen != null)
        {
            Material material = ItemStorage.Instance.GetWeaponSkin(weaponSkinTag);
            objRen.materials = new Material[] { material, material };
        }
    }
    private void LoadDataFromPlayerPrefs()
    {
        weaponTag = (WeaponType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_TAG);
        weaponSkinTag = (WeaponSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_SKIN_TAG);
        pantSkinTag = (PantSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_PANT_SKIN_TAG);
    }
}
