using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UICanvasID
{
    MainMenu,
    GamePlay,
    Setting,
    Result,
    WeaponShop,
    SkinShop
}
public class UICanvas : MonoBehaviour
{
    public GameObject CanvasObj;
    public bool isDestroyOnClose;
    public void Open()
    {
        CanvasObj.SetActive(true);
        OnOpenCanvas();
    }
    protected virtual void OnOpenCanvas()
    {
        Debug.Log("Open Canvas " + gameObject.name);
    }
    public void Close()
    {
        OnCloseCanvas();
        CanvasObj.SetActive(false);

        if (isDestroyOnClose)
        {
            Destroy(CanvasObj);
        }
    }
    protected virtual void OnCloseCanvas()
    {
        Debug.Log("Close Canvas " + gameObject.name);
    }
}