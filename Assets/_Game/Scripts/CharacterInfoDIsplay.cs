using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoDIsplay : MonoBehaviour, IPoolCharacterUI
{
    public GameObject UIObject;
    public RectTransform UITrans;
    public RectTransform ParentCanvasTrans;
    public Image ScoreImage;
    public Image ArrowImage;
    public RectTransform ArrowAnchorTrans;
    public TMP_Text ScoreText;
    public TMP_Text NameText;
    public TMP_Text ScorePopUpText;
    public Animator ScoreTextAnim;
    private CharacterBase currentChar;
    [HideInInspector]
    public Camera curCam;
    private bool isPlayer;
    private bool isOutScreen;
    private bool enableFlag;
    private float parentCanvasLength;
    private float parentCanvasHeight;
    private float targetParentCanvasLength = 1080f;
    private float targetParentCanvasHeight = 1920f;
    private float UIOffsetXAxis = 64f;
    private float UIOffsetYAxis = 64f;
    private Vector3 center;
    private float boundX;
    private float boundY;

    private void Awake()
    {
        curCam = Camera.main; //NOTE: Temp 

        CalculateOffset();

        enableFlag = NameText.enabled; Debug.Log(UITrans.sizeDelta + "  " + parentCanvasLength + "  " + parentCanvasHeight);
    }
    private void Update()
    {
        if (!isPlayer)
        {
            MoveUI();
        }
    }
    private void CalculateOffset()
    {
        ParentCanvasTrans = (RectTransform)CharacterUIPooling.Instance.ParentTransform;
        parentCanvasLength = ParentCanvasTrans.sizeDelta.x * ParentCanvasTrans.localScale.x;
        parentCanvasHeight = ParentCanvasTrans.sizeDelta.y * ParentCanvasTrans.localScale.y;

        UIOffsetXAxis *= parentCanvasLength / targetParentCanvasLength;
        UIOffsetYAxis *= parentCanvasHeight / targetParentCanvasHeight;

        center = new Vector3(parentCanvasLength / 2, parentCanvasHeight / 2, 0);

        boundX = (parentCanvasLength - 2 * UIOffsetXAxis) / 2;
        boundY = (parentCanvasHeight - 2 * UIOffsetYAxis) / 2;

        Debug.Log(parentCanvasHeight + "   " + parentCanvasLength + "   " + UIOffsetXAxis + "   " + UIOffsetYAxis);
    }
    public void MoveUI()
    {
        Vector3 pos = curCam.WorldToScreenPoint(currentChar.CharacterUITransRoot.position);
        if (pos.z <= 0)
        {
            pos *= -1f; //NOTE: if z<0 means UIRootPos is behind camera --> flipped so need to flip back
        }

        isOutScreen = false;
        pos -= center;
        float angle = Mathf.Atan2(pos.y, pos.x);
        float m = Mathf.Tan(angle);

        if (pos.x > boundX)
        {
            pos = new Vector3(boundX, boundX * m);
            isOutScreen = true;
        }
        else if (pos.x < -boundX)
        {
            pos = new Vector3(-boundX, -boundX * m);
            isOutScreen = true;
        }

        if (pos.y > boundY)
        {
            pos = new Vector3(boundY / m, boundY);
            isOutScreen = true;
        }
        else if (pos.y < -boundY)
        {
            pos = new Vector3(-boundY / m, -boundY);
            isOutScreen = true;
        }
        pos += center;

        if (isOutScreen)
        {
            if (enableFlag)
            {
                NameText.enabled = false;
                ArrowImage.enabled = true;
                enableFlag = false;
                currentChar.IsAudioPlayable = false;
            }

            ArrowAnchorTrans.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }
        else
        {
            if (!enableFlag)
            {
                NameText.enabled = true;
                ArrowImage.enabled = false;
                enableFlag = true;
                currentChar.IsAudioPlayable = true;
            }
        }

        UITrans.position = pos;
    }
    public void SetUpUI(string name, Color color, bool isPlayer, int score)
    {
        if (isPlayer)
        {
            currentChar.IsAudioPlayable = true;

            Player playerRef = Player.PlayerGlobalReference;
            if (playerRef.SkinSetTag == SkinSet.Set_4)
            {
                color = ConstValues.VALUE_UI_COLOR_FOR_SKIN_SET_4;
            }
            if (playerRef.SkinSetTag == SkinSet.Set_5)
            {
                color = ConstValues.VALUE_UI_COLOR_FOR_SKIN_SET_5;
            }
        }
        ScoreText.text = score.ToString();
        NameText.text = name;
        ScoreImage.color = color;
        ArrowImage.color = color;
        NameText.color = color;
        enableFlag = CheckOutScreen();
        this.isPlayer = isPlayer;


        MoveUI();
    }
    private bool CheckOutScreen()
    {
        Vector3 pos = curCam.WorldToScreenPoint(currentChar.CharacterUITransRoot.position);
        pos -= center;

        if (pos.x > boundX || pos.x < -boundX || pos.y > boundY || pos.y < -boundY)
        {
            return true;
        }
        return false;
    }
    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString();
        MoveUI(); //NOTE: update player ui when scale up
    }
    public void TriggerPopupScore(int addedScore)
    {
        ScorePopUpText.text = addedScore.ToString();
        ScorePopUpText.enabled = true;

        ScoreTextAnim.SetTrigger(ConstValues.ANIM_TRIGGER_SCORE_POPUP);
        StartCoroutine(DelayDisableScorePopupText());
    }

    public void OnSpawn(CharacterBase characterBase)
    {
        currentChar = characterBase;
        currentChar.currentUIDisplay = this;
        NameText.enabled = true;
        ArrowImage.enabled = false;
        enableFlag = true;
    }
    public void OnDespawn()
    {
        currentChar.currentUIDisplay = null;
        currentChar = null;
        isPlayer = false;
        ScorePopUpText.enabled = false;
    }

    public IEnumerator DelayDisableScorePopupText()
    {
        yield return new WaitForSeconds(ConstValues.VALUE_SCORE_POPUP_TEXT_ANIMATION_TIME);
        ScorePopUpText.enabled = false;
    }
}