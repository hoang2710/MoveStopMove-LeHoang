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

    public GameObject TargetMark;
    public Transform TargetMarkTrans;
    public GameObject AttackRangeDisplay;
    public Transform AttackRangeDisplayTrans;
    private bool TargetMarkSetActiveFlag;

    private void FixedUpdate()
    {
        LogicHandle();
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
                SetUpPlayerLoadLevel();
                break;
            case GameState.MainMenu:
                if (isShop)
                {
                    LoadDataFromPlayerPrefs();
                    SetUpHandWeapon();
                    SetUpPantSkin();
                }
                break;
            case GameState.Playing:
                SetUpPLayerPlaying();
                break;
            default:
                break;
        }
    }
    private void LogicHandle() //NOTE: optimize later or not
    {
        if (!isDead)
        {
            DispalyTargetMark();
            if (MoveDir.sqrMagnitude > 0.01f)
            {
                Move();
                DetectTarget();
            }
            else
            {
                if (AttackTargetTrans != null && isAttackable)
                {
                    Attack();
                }
                else
                {
                    Idle();
                    DetectTarget();
                }

                timer += Time.deltaTime;
            }
        }
    }
    private void Move() //NOTE: optimize later
    {
        CharaterTrans.position = Vector3.MoveTowards(CharaterTrans.position, CharaterTrans.position + MoveDir, moveSpeed * Time.deltaTime);
        SetCharacterRotation();

        ChangeAnimation(ConstValues.ANIM_TRIGGER_RUN);

        isAttackable = true;
        isAttack = false;
        timer = 0;
        WeaponPlaceHolder.SetActive(true);
    }
    private void Idle() //Optimize later
    {
        if (isAttack)
        {
            if (timer >= AttackAnimEnd)
            {
                isAttack = false;
                WeaponPlaceHolder.SetActive(true);
            }
        }
        else
        {
            ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);
            timer = 0;
        }
    }
    private void Attack() //NOTE: optimize later
    {
        ChangeAnimation(ConstValues.ANIM_TRIGGER_ATTACK);

        Vector3 lookDir = AttackTargetTrans.position - CharaterTrans.position;
        lookDir.y = 0;

        Quaternion tempRotation = Quaternion.LookRotation(lookDir);
        CharaterTrans.rotation = tempRotation;

        if (timer > AttackAnimThrow)
        {
            WeaponPlaceHolder.SetActive(false);

            GameObject obj = ItemStorage.Instance.PopWeaponFromPool(WeaponTag, WeaponSkinTag, AttackPos.position, tempRotation * WeaponRotation);
            Weapon weapon = obj.GetComponent<Weapon>();

            weapon.SetFlyDir(AttackPos.forward);
            weapon.SetBulletOwner(this);
            weapon.CalculateLifeTime();

            isAttackable = false;
            isAttack = true;
        }
    }
    private void Die()
    {
        isDead = true;
        ChangeAnimation(ConstValues.ANIM_TRIGGER_DEAD);
        CharacterCollider.enabled = false;

        GameManager.Instance.ChangeGameState(GameState.ResultPhase);
    }
    private void DispalyTargetMark()
    {
        //NOTE: temp solution, optimize later, or not
        if (AttackTargetTrans != null && !TargetMarkSetActiveFlag)
        {
            TargetMark.SetActive(true);
            TargetMarkSetActiveFlag = true;
            TargetMarkTrans.position = AttackTargetTrans.position;
            TargetMarkTrans.SetParent(AttackTargetTrans);
        }
        else if (AttackTargetTrans == null && TargetMarkSetActiveFlag)
        {
            TargetMark.SetActive(false);
            TargetMarkSetActiveFlag = false;
        }
    }
    public void OnHit(CharacterBase bulletOwner)
    {
        Die();
    }
    private void SetUpPlayerLoadLevel()
    {
        isDead = false;
        CharacterCollider.enabled = true;
        CharaterTrans.localScale = Vector3.one;
        AttackRangeDisplay.SetActive(false);
        AttackRangeDisplayTrans.localScale = Vector3.one * ConstValues.VALUE_BASE_ATTACK_RANGE;
    }
    private void SetUpPLayerPlaying()
    {
        AttackRangeDisplay.SetActive(true);
    }
    private void SetCharacterRotation()
    {
        float tmp = Mathf.Atan2(MoveDir.x, MoveDir.z) * Mathf.Rad2Deg;
        CharaterTrans.rotation = Quaternion.Lerp(CharaterTrans.rotation, Quaternion.Euler(0, tmp, 0), Time.deltaTime * rotateSpeed);
    }
    private void LoadDataFromPlayerPrefs()
    {
        WeaponTag = (WeaponType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_TAG);
        WeaponSkinTag = (WeaponSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_SKIN_TAG);
        PantSkinTag = (PantSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_PANT_SKIN_TAG);
    }
}
