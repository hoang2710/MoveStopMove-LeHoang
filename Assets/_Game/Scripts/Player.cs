using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : CharacterBase, IHit, IDataHandler
{
    public static Vector3 MoveDir;
    [SerializeField]
    private float moveSpeed = 1.5f;
    private float defaultMoveSpeed;
    [SerializeField]
    private float rotateSpeed = 8f;

    private bool isAttackable = true;
    private bool isAttack;
    private bool isDead;
    private bool isShop;
    private bool isRevivable = true;

    private float timer = 0;

    public GameObject PlayerObj;
    public GameObject TargetMark;
    public Transform TargetMarkTrans;
    public GameObject AttackRangeDisplay;
    public Transform AttackRangeDisplayTrans;

    private bool TargetMarkSetActiveFlag;

    public static Player PlayerGlobalReference;
    public static event System.Action<Player> OnPlayerSizeUp;

    public NavMeshAgent NavMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        isPlayer = true;
        PlayerGlobalReference = this;
        defaultMoveSpeed = moveSpeed;
    }
    protected override void Start()
    {
        base.Start();

        DataManager.Instance.AssignDataHandler(this);
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
                break;
            case GameState.LoadLevel:
                SetUpHandWeapon();
                SetUpPantSkin();
                SetUpHat();
                SetUpPlayerLoadLevel();
                RemoveCharacterUI();
                break;
            case GameState.MainMenu:
                isShop = false;
                break;
            case GameState.SkinShop:
                isShop = true;
                break;
            case GameState.Playing:
                if (GameManager.Instance.PrevGameState == GameState.ReviveOption)
                {
                    SetUpPlayerOnRevive();
                }
                else if (GameManager.Instance.PrevGameState != GameState.Pause)
                {
                    StartCoroutine(EnterPlayingState());
                }
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
        if (!isDead && !isShop)
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
    private void Die(CharacterBase bulletOwner)
    {
        isDead = true;
        ChangeAnimation(ConstValues.ANIM_TRIGGER_DEAD);
        CharacterCollider.enabled = false;
        NavMeshAgent.enabled = false; //NOTE: set false to prevent a frame delay when set back to default position for new level in SetPLaying method 

        StartCoroutine(DelayDieHandle(bulletOwner));
    }
    private void SetCharacterRotation()
    {
        float tmp = Mathf.Atan2(MoveDir.x, MoveDir.z) * Mathf.Rad2Deg;
        CharaterTrans.rotation = Quaternion.Lerp(CharaterTrans.rotation, Quaternion.Euler(0, tmp, 0), Time.deltaTime * rotateSpeed);
    }
    public void OnHit(CharacterBase bulletOwner, Weapon weapon)
    {
        Die(bulletOwner);

        ItemStorage.Instance.PushWeaponToPool(weapon.WeaponTag, weapon.WeaponObject);

        //NOTE: temp solution for random enum option
        int ran = Random.Range((int)AudioType.Die1, (int)AudioType.Die4 + 1);
        PlayAudioWithCondition((AudioType)ran);
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
    private void SetUpPlayerLoadLevel()
    {
        isDead = false;
        isAttackable = false;
        isAttack = false;
        isRevivable = true;
        timer = 0;
        Score = 0;
        KillScore = defaultKillScore;
        moveSpeed = defaultMoveSpeed;
        PlayerLevel = 1;

        AttackTarget = null;
        AttackTargetTrans = null;

        CharacterCollider.enabled = true;
        CharaterTrans.localScale = Vector3.one;
        AttackRangeDisplay.SetActive(false);
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRangeDisplayTrans.localScale = Vector3.one * AttackRange;
        CharaterTrans.position = Vector3.zero;
        CharaterTrans.rotation = Quaternion.Euler(0, 180f, 0);
    }
    private void SetUpPLayerPlaying()
    {
        CharaterTrans.position = Vector3.zero;
        CharaterTrans.rotation = Quaternion.Euler(0, 180f, 0);
        AttackRangeDisplay.SetActive(true);
        NavMeshAgent.enabled = true; //NOTE: set back just that
    }
    private void SetUpPlayerOnRevive()
    {
        isDead = false;
        isAttackable = false;
        isAttack = false;
        timer = 0;

        ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);

        AttackTarget = null;
        AttackTargetTrans = null;

        CharacterCollider.enabled = true;
        NavMeshAgent.enabled = true;

        Vector3 newPos;
        LevelManager.Instance.GetRandomPos(CharaterTrans.position, out newPos);

        CharaterTrans.position = newPos;
    }
    public void SetPlayerName(string name)
    {
        CharacterName = name;
    }
    public override void OnKillEnemy(int gainedScore)
    {
        base.OnKillEnemy(gainedScore);

        if (isSizeUp)
        {
            CameraManager.Instance.ZoomOutCamera();
            AudioManager.Instance.MakeVibration();

            moveSpeed += defaultMoveSpeed * ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO;
            currentUIDisplay?.MoveUI();

            OnPlayerSizeUp?.Invoke(this);
        }
    }
    public IEnumerator EnterPlayingState()
    {
        SetUpPLayerPlaying();
        yield return new WaitForSeconds(0.55f); //NOTE Wait for camera to transit complete (>0.5f)
        DisplayCharacterUI();
    }
    public IEnumerator DelayDieHandle(CharacterBase bulletOwner)
    {
        UIResultCanvas resultCanvas = UIManager.Instance.GetUICanvas<UIResultCanvas>(UICanvasID.Result);
        resultCanvas.CanvasObj.SetActive(false);
        resultCanvas.SetKillerName(bulletOwner.CharacterName, bulletOwner.CharacterRenderer.material.color);

        yield return new WaitForSeconds(1.5f);

        if (isRevivable)
        {
            GameManager.Instance.ChangeGameState(GameState.ReviveOption);
            isRevivable = false;
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.ResultPhase);
        }
    }

    public void LoadData(GameData data)
    {
        WeaponTag = data.WeaponTag;
        WeaponSkinTag = data.WeaponSkinTag;
        PantSkinTag = data.PantSkinTag;
        HatTag = data.HatTag;

        CharacterName = data.PlayerName;
    }

    public void SaveData(GameData data)
    {
        data.WeaponTag = WeaponTag;
        data.WeaponSkinTag = WeaponSkinTag;
        data.PantSkinTag = PantSkinTag;
        data.HatTag = HatTag;

        data.PlayerName = CharacterName;
    }
}
