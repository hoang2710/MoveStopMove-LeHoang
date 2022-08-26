using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase, IHit
{
    public static Vector3 MoveDir;
    [SerializeField]
    private float moveSpeed = 1.5f;
    [SerializeField]
    private float rotateSpeed = 8f;

    private bool isAttackable = true;
    private bool isAttack;
    private bool isDead;

    private float timer = 0;

    public GameObject PlayerObj;
    public GameObject TargetMark;
    public Transform TargetMarkTrans;
    public GameObject AttackRangeDisplay;
    public Transform AttackRangeDisplayTrans;
    private bool TargetMarkSetActiveFlag;
    protected override void Awake()
    {
        base.Awake();
        isPlayer = true;
    }
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
                RemoveCharacterUI();
                break;
            // case GameState.MainMenu:
            //     ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);
            //     if (!PlayerObj.activeInHierarchy)
            //     {
            //         PlayerObj.SetActive(true);
            //     }
            //     break;
            // case GameState.WeaponShop:
            //     PlayerObj.SetActive(false);
            //     break;
            // case GameState.SkinShop:
            //     ChangeAnimation(ConstValues.ANIM_TRIGGER_DANCE_CHAR_SKIN);
            //     break;
            case GameState.Playing:
                StartCoroutine(EnterPlayingState());
                break;
            case GameState.ResultPhase:
                isAttackable = false;
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

        Quaternion curRotation = Quaternion.LookRotation(lookDir);
        CharaterTrans.rotation = curRotation;

        if (timer > AttackAnimThrow)
        {
            WeaponPlaceHolder.SetActive(false);

            ThrowWeapon(curRotation);

            isAttackable = false;
            isAttack = true;
        }
    }
    private void Die(CharacterBase bullOwner)
    {
        isDead = true;
        ChangeAnimation(ConstValues.ANIM_TRIGGER_DEAD);
        CharacterCollider.enabled = false;

        GameManager.Instance.ChangeGameState(GameState.ResultPhase);
        UIResultCanvas resultCanvas = UIManager.Instance.GetUICanvas<UIResultCanvas>(UICanvasID.Result);

        resultCanvas.SetKillerName(bullOwner.CharacterName, bullOwner.CharacterRenderer.material.color);
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
        Die(bulletOwner);
    }
    private void SetUpPlayerLoadLevel()
    {
        isDead = false;
        isAttackable = false;
        isAttack = false;
        timer = 0;
        Score = 0;

        AttackTarget = null;
        AttackTargetTrans = null;

        CharacterCollider.enabled = true;
        CharaterTrans.localScale = Vector3.one;
        AttackRangeDisplay.SetActive(false);
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRangeDisplayTrans.localScale = Vector3.one * AttackRange;
        CharaterTrans.position = Vector3.zero; //NOTE: needed to set twice
        CharaterTrans.rotation = Quaternion.Euler(0, 180f, 0);
    }
    private void SetUpPLayerPlaying()
    {
        CharaterTrans.position = Vector3.zero; //NOTE: needed to set twice
        CharaterTrans.rotation = Quaternion.Euler(0, 180f, 0);
        AttackRangeDisplay.SetActive(true);

        UIMainMenuCanvas mainMenuCanvas = UIManager.Instance.GetUICanvas<UIMainMenuCanvas>(UICanvasID.MainMenu);
        CharacterName = mainMenuCanvas.GetPlayerName();
    }
    private void SetCharacterRotation()
    {
        float tmp = Mathf.Atan2(MoveDir.x, MoveDir.z) * Mathf.Rad2Deg;
        CharaterTrans.rotation = Quaternion.Lerp(CharaterTrans.rotation, Quaternion.Euler(0, tmp, 0), Time.deltaTime * rotateSpeed);
    }
    public void LoadDataFromPlayerPrefs()
    {
        WeaponTag = (WeaponType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_TAG);
        WeaponSkinTag = (WeaponSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_SKIN_TAG);
        PantSkinTag = (PantSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_PANT_SKIN_TAG);
    }

    public IEnumerator EnterPlayingState()
    {
        SetUpPLayerPlaying();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        DisplayCharacterUI();
    }
}
