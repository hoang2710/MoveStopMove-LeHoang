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
    private float UIOffsetXAxis = 96f;
    private float UIOffsetYAxis = 96f;
    private Vector3 center;
    private float boundX;
    private float boundY;

    private void Awake()
    {
        curCam = Camera.main; //NOTE: Temp 
        ParentCanvasTrans = (RectTransform)CharacterUIPooling.Instance.ParentTransform;
        parentCanvasLength = ParentCanvasTrans.sizeDelta.x;
        parentCanvasHeight = ParentCanvasTrans.sizeDelta.y;

        UIOffsetXAxis *= parentCanvasLength / targetParentCanvasLength;
        UIOffsetYAxis *= parentCanvasHeight / targetParentCanvasHeight;

        center = new Vector3(parentCanvasLength / 2, parentCanvasHeight / 2, 0);

        boundX = (parentCanvasLength - 2 * UIOffsetXAxis) / 2;
        boundY = (parentCanvasHeight - 2 * UIOffsetYAxis) / 2;

        enableFlag = NameText.enabled; Debug.Log(UITrans.sizeDelta + "  " + parentCanvasLength + "  " + parentCanvasHeight);
    }
    private void LateUpdate()
    {
        if (!isPlayer)
        {
            MoveUI();
        }
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
    public void SetUpUI(string name, Color color, bool isPlayer)
    {
        ScoreText.text = 0.ToString();
        NameText.text = name;
        ScoreImage.color = color;
        ArrowImage.color = color;
        NameText.color = color;
        this.isPlayer = isPlayer;

        if (isPlayer)
        {
            currentChar.IsAudioPlayable = true;
        }

        MoveUI();
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

#region old moveUI
/*
private void MoveUI()
{
    Vector3 pos = curCam.WorldToScreenPoint(currentChar.CharacterUITransRoot.position);
    if (pos.z <= 0)
    {
        pos *= -1f; //NOTE: if z<0 means UIRootPos is behind camera --> flipped so need to flip back
    }

    if (pos.x > xAxisMax || pos.x < xAxisMin)
    {
        isOutScreen = true;
    }
    else if (pos.y > yAxisMax || pos.y < yAxisMin)
    {
        isOutScreen = true;
    }
    else
    {
        isOutScreen = false;
    }

    if (isOutScreen)
    {
        Vector3 truePos = pos;

        pos.x = Mathf.Clamp(pos.x, xAxisMin, xAxisMax);
        pos.y = Mathf.Clamp(pos.y, yAxisMin, yAxisMax);

        if (enableFlag)
        {
            NameText.enabled = false;
            ArrowImage.enabled = true;
            enableFlag = false;
        }

        Vector3 arrowDir = truePos - pos;
        float angle = Mathf.Atan2(arrowDir.y, arrowDir.x) * Mathf.Rad2Deg;
        ArrowAnchorTrans.rotation = Quaternion.Euler(0, 0, angle);
    }
    else
    {
        if (!enableFlag)
        {
            NameText.enabled = true;
            ArrowImage.enabled = false;
            enableFlag = true;
        }
    }

    UITrans.position = pos;
}
*/
#endregion