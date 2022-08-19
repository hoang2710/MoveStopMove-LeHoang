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
    [HideInInspector]
    public Color CharacterColor { get; protected set; }
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

    public Quaternion WeaponRotation { get; protected set; }
    public float AttackAnimThrow { get; protected set; }
    public float AttackAnimEnd { get; protected set; }

    public GameObject WeaponPlaceHolder;
    protected GameObject handWeapon;
    protected Quaternion handWeaponRotation = Quaternion.Euler(90f, -90f, 180f);

    public Renderer CharacterRenderer;
    public Renderer PantRenderer;

    public Transform CharacterNameTrans;
    public Transform CharacterScoreTrans;
    [HideInInspector]
    public CharacterInfoDIsplay currentUIDisplay;

    private void Awake()
    {
        CharacterName = ConstValues.VALUE_CHARACTER_DEFAULT_NAME;
        CharacterColor = ConstValues.VALUE_CHARACTER_DEFAULT_COLOR;
        Score = 0;
        KillScore = 0;
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRate = ConstValues.VALUE_BASE_ATTACK_RATE;

        WeaponRotation = Quaternion.Euler(-90f, 0, 90f);

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
    public void SetUpHandWeapon()
    {
        if (handWeapon != null)
        {
            Destroy(handWeapon);
        }

        Anim.Play(ConstValues.ANIM_PLAY_DEFAULT_IDLE); //NOTE: make sure character model is in right position for assign hand weapon

        Transform WeaponPlaceHolderTrans = WeaponPlaceHolder.transform;
        handWeapon = Instantiate(ItemStorage.Instance.GetWeaponType(WeaponTag),
                                WeaponPlaceHolderTrans.position,
                                Quaternion.identity,
                                WeaponPlaceHolderTrans);

        Transform handWeaponTrans = handWeapon.transform;
        handWeaponTrans.localRotation = handWeaponRotation;

        Weapon weapon = handWeapon.GetComponent<Weapon>();
        weapon?.DeactiveWeaponScript();

        Renderer objRen = handWeapon.GetComponent<Renderer>();

        if (objRen != null)
        {
            switch (WeaponTag)
            {
                case WeaponType.Candy:
                    Material materialCandy = ItemStorage.Instance.GetWeaponSkin(WeaponSkinTag);
                    objRen.materials = new Material[] { materialCandy, materialCandy, materialCandy }; //NOTE: Candy weapon have 3 material
                    break;
                default:
                    Material material = ItemStorage.Instance.GetWeaponSkin(WeaponSkinTag);
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
        CharaterTrans.localScale *= ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO;
        AttackRange *= ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO;
    }
    public void SetUpPantSkin()
    {
        PantRenderer.material = ItemStorage.Instance.GetPantSkin(PantSkinTag);
    }
    public void DisplayCharacterUI()
    {
        CharacterUIPooling.Instance.PopUIFromPool(this);

        currentUIDisplay?.SetUpUI(CharacterName, CharacterColor);
    }
    public void RemoveCharacterUI()
    {
        if (currentUIDisplay != null)
        {
            CharacterUIPooling.Instance.PushUIToPool(currentUIDisplay.CanvasObject);
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
