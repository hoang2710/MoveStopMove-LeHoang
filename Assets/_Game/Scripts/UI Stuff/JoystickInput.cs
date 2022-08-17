using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickInput : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public UIGamePlayCanvas GamePlayCanvas;
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    public RectTransform JoystickBackground;
    public RectTransform JoystickHandle;
    public RectTransform JoystickPanel;
    public GameObject Joystick;
    [SerializeField]
    private float joystickMaxDistance = 150f;
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickBackground, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / JoystickBackground.sizeDelta.x);
            pos.y = (pos.y / JoystickBackground.sizeDelta.y);

            Player.MoveDir = new Vector3(pos.x, 0, pos.y).normalized;

            pos = pos * joystickMaxDistance;

            JoystickHandle.anchoredPosition = Vector3.ClampMagnitude(pos, joystickMaxDistance);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickPanel, eventData.position, eventData.pressEventCamera, out pos))
        {
            JoystickBackground.anchoredPosition = pos;
            Joystick.SetActive(true);
            OnDrag(eventData);
            GamePlayCanvas.SetActiveTipPanel(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetBackState();
    }
    public void SetBackState()
    {
        Player.MoveDir = Vector3.zero;

        JoystickHandle.anchoredPosition = default(Vector3);
        Joystick.SetActive(false);
    }
}
