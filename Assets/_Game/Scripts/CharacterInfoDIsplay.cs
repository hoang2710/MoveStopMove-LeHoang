using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoDIsplay : MonoBehaviour, IPoolCharacterUI
{
    public GameObject CanvasObject;
    public Transform CanvasTrans;
    public Transform NameUITrans;
    public Transform ScoreUITrans;
    public Image ScoreImage;
    public TMP_Text ScoreText;
    public TMP_Text NameText;
    private CharacterBase currentChar;
    [HideInInspector]
    public Transform CameraTrans;

    private void Awake()
    {
        CameraTrans = Camera.main.transform; //NOTE: TEMP SOLUTION FOR DEV CHARACTER UI !!!!!!!!!!!
    }
    private void Update()
    {
        Vector3 lookDir = CanvasTrans.position - CameraTrans.position;
        CanvasTrans.LookAt(lookDir);
    }
    public void SetUpUI(string name, Color color)
    {
        ScoreText.text = 0.ToString();
        NameText.text = name;
        ScoreImage.color = color;
    }
    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void OnInit(CharacterBase characterBase)
    {
        currentChar = characterBase;
        NameUITrans = currentChar.CharacterNameTrans;
        ScoreUITrans = currentChar.CharacterScoreTrans;
        CanvasTrans.SetParent(characterBase.CharaterTrans);

        currentChar.currentUIDisplay = this;
    }

    public void OnDespawn()
    {
        currentChar.currentUIDisplay = null;
        currentChar = null;
    }
}
