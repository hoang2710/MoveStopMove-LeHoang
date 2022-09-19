using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : CharacterBase, IHit, IDataHandler
{
    public static Vector3 MoveDir;
    private Quaternion curRotation;
    [SerializeField] private float moveSpeed = 1.5f;
    private float defaultMoveSpeed;
    [SerializeField] private float rotateSpeed = 8f;

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

    private bool moveFlag; //NOTE: use for prevent some code execute every frame
    private bool attackFlag; //NOTE: use for prevent some code execute every frame

    public SkinSet SkinSetTag { get; protected set; }

    public EquipItem BackRef { get; set; }
    public EquipItem TailRef { get; set; }

    [SerializeField] private Material defaultCharSkin;

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
    private void Update()
    {
        LogicHandle();
    }
    protected override void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.LoadGame:
                SetUpHandWeapon();
                if (SkinSetTag == SkinSet.None)
                {
                    SetUpPantSkin();
                    SetUpHat();
                    SetUpShield();
                }
                else
                {
                    SetupSkinSet();
                }
                break;
            case GameState.LoadLevel:
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
                NavMeshAgent.enabled = false;
                CharacterCollider.enabled = false;
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

        if (!moveFlag)
        {
            moveFlag = true;
            attackFlag = false;

            isAttackable = true;
            isAttack = false;
            WeaponPlaceHolder.SetActive(true);
        }

        timer = 0;
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

        if (!attackFlag)
        {
            attackFlag = true;
            moveFlag = false;

            Vector3 lookDir = AttackTargetTrans.position - CharaterTrans.position;
            lookDir.y = 0;

            curRotation = Quaternion.LookRotation(lookDir);
            CharaterTrans.rotation = curRotation;
        }

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
        moveFlag = false;
        attackFlag = false;

        isDead = true;
        ChangeAnimation(ConstValues.ANIM_TRIGGER_DEAD);
        CharacterCollider.enabled = false;

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

        ItemStorage.Instance.PushWeaponToPool(weapon);

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
            TargetMarkTrans.SetParent(AttackTargetTrans, false);
            TargetMarkTrans.position = AttackTargetTrans.position;
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
        CharacterLevel = 1;

        AttackTarget = null;
        AttackTargetTrans = null;

        CharacterCollider.enabled = true;
        CharaterTrans.localScale = Vector3.one;
        AttackRangeDisplay.SetActive(false);
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRangeDisplayTrans.localScale = Vector3.one * AttackRange;
        AttackPosOffset = ConstValues.VALUE_DEFAULT_ATTACK_POS_OFFSET;
        CharaterTrans.position = Vector3.zero;
        CharaterTrans.rotation = Quaternion.Euler(0, 180f, 0);
    }
    private void SetUpPLayerPlaying()
    {
        moveFlag = false;
        attackFlag = false;

        AttackRangeDisplay.SetActive(true);
        NavMeshAgent.enabled = true; //NOTE: set back just that
    }
    private void SetUpPlayerOnRevive()
    {
        moveFlag = false;
        attackFlag = false;

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

            moveSpeed = (1 + (CharacterLevel - 1) * ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO) * defaultMoveSpeed;
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

    public void SetSkinSet(SkinSet tag)
    {
        SkinSetTag = tag;
    }
    public void SetupSkinSet()
    {
        if (SkinSetTag == SkinSet.None)
        {
            UnequipSkinSet();
            return;
        }
        else
        {
            DestroyEquipedBackAndTail();
        }

        SkinSetDataSO skinSet = ItemStorage.Instance.GetSkinSet(SkinSetTag);

        CharacterRenderer.material = skinSet.CharSkinMat;

        SetHat(skinSet.HatTag);
        SetUpHat();

        SetShield(skinSet.ShieldTag);
        SetUpShield();

        SetPantSkin(skinSet.PantSkinTag);
        SetUpPantSkin();

        BackRef = ItemStorage.Instance.GetBackItem(skinSet.BackItemTag);
        BackRef.SetupItem(BackPlaceHolderTrans);

        TailRef = ItemStorage.Instance.GetTailItem(skinSet.TailTag);
        TailRef.SetupItem(TailPlaceHolderTrans);
    }
    public void UnequipSkinSet()
    {
        CharacterRenderer.material = defaultCharSkin;

        SetHat(HatType.None);
        SetUpHat();

        SetShield(ShieldType.None);
        SetUpShield();

        SetPantSkin(PantSkinType.Invisible);
        SetUpPantSkin();

        DestroyEquipedBackAndTail();
    }
    private void DestroyEquipedBackAndTail()
    {
        if (BackRef != null)
        {
            Destroy(BackRef.Object);
        }
        if (TailRef != null)
        {
            Destroy(TailRef.Object);
        }
    }

    public void LoadData(GameData data)
    {
        WeaponTag = data.WeaponTag;
        WeaponSkinTag = data.WeaponSkinTag;
        PantSkinTag = data.PantSkinTag;
        HatTag = data.HatTag;
        ShieldTag = data.ShieldTag;
        SkinSetTag = data.SkinSetTag;

        CharacterName = data.PlayerName;
    }

    public void SaveData(GameData data)
    {
        if (UIManager.Instance.IsUICanvasOpened(UICanvasID.SkinShop))
        {
            UISkinShopCanvas skinShopCanvas = UIManager.Instance.GetUICanvas<UISkinShopCanvas>(UICanvasID.SkinShop);
            skinShopCanvas.SetBackData();
        }

        data.WeaponTag = WeaponTag;
        data.WeaponSkinTag = WeaponSkinTag;
        data.PantSkinTag = PantSkinTag;
        data.HatTag = HatTag;
        data.ShieldTag = ShieldTag;
        data.SkinSetTag = SkinSetTag;

        data.PlayerName = CharacterName;
    }
}
