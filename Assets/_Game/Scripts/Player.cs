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
    public bool SetActiveFlag;

    private void FixedUpdate()
    {
        LogicHandle();
    }
    protected override void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.LoadGame:
                // LoadDataFromPlayerPrefs(); //NOTE: Disable for debug
                break;
            case GameState.LoadLevel:
                SetUpHandWeapon();
                SetUpPantSkin();
                SetUpPlayer();
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
    private void LogicHandle() //NOTE: optimize later
    {
        if (!isDead)
        {
            DispalyTargetMark();
            if (MoveDir.sqrMagnitude > 0.01f)
            {
                Move();
                SetCharacterRotation();
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
        else
        {
            Anim.SetTrigger(ConstValues.ANIM_TRIGGER_DEAD);
        }
    }

    private void Die()
    {
        isDead = true;
        // Anim.SetTrigger(ConstValues.ANIM_TRIGGER_DEAD);
        CharacterCollider.enabled = false;

        GameManager.Instance.ChangeGameState(GameState.ResultPhase);
    }
    private void DispalyTargetMark()
    {
        //NOTE: temp solution, optimize later, or not
        if (AttackTargetTrans != null && !SetActiveFlag)
        {
            TargetMark.SetActive(true);
            SetActiveFlag = true;
            TargetMarkTrans.position = AttackTargetTrans.position;
            TargetMarkTrans.SetParent(AttackTargetTrans);
        }
        else if (AttackTargetTrans == null && SetActiveFlag)
        {
            TargetMark.SetActive(false);
            SetActiveFlag = false;
        }
    }
    public void OnHit()
    {
        Die();
    }
    private void SetUpPlayer()
    {
        isDead = false;
        CharacterCollider.enabled = true;
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
