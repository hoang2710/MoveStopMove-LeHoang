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
    private CharacterBase currentChar;
    [HideInInspector]
    public Camera curCam;
    private bool isPlayer;
    private bool isOutScreen;
    private bool enableFlag;
    private float parentCanvasLength;
    private float parentCanvasHeight;
    private float UIOffsetXAxis = 96f;
    private float UIOffsetYAxis = 96f;
    private float xAxisMin;
    private float xAxisMax;
    private float yAxisMin;
    private float yAxisMax;
    private Vector3 center;
    private float boundX;
    private float boundY;

    private void Awake()
    {
        curCam = Camera.main; //NOTE: Temp 
        ParentCanvasTrans = (RectTransform)CharacterUIPooling.Instance.ParentTransform;
        parentCanvasLength = ParentCanvasTrans.sizeDelta.x;
        parentCanvasHeight = ParentCanvasTrans.sizeDelta.y;

        xAxisMin = UIOffsetXAxis;
        xAxisMax = parentCanvasLength - UIOffsetXAxis;
        yAxisMin = UIOffsetYAxis;
        yAxisMax = parentCanvasHeight - UIOffsetYAxis;

        center = new Vector3(parentCanvasLength / 2, parentCanvasHeight / 2, 0);
        boundX = (parentCanvasLength - 2 * UIOffsetXAxis) / 2;
        boundY = (parentCanvasHeight - 2 * UIOffsetYAxis) / 2;

        enableFlag = NameText.enabled;
    }
    private void LateUpdate()
    {
        if (!isPlayer)
        {
            MoveUI();
        }
    }
    private void MoveUI()
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
            }

            ArrowAnchorTrans.rotation = Quaternion.Euler(0, 0, angle*Mathf.Rad2Deg);
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
    public void SetUpUI(string name, Color color, bool isPlayer)
    {
        ScoreText.text = 0.ToString();
        NameText.text = name;
        ScoreImage.color = color;
        ArrowImage.color = color;
        NameText.color = color;
        this.isPlayer = isPlayer;

        MoveUI();
    }
    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString();
        MoveUI(); //NOTE: update player ui when scale up
    }

    public void OnInit(CharacterBase characterBase)
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