using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : SingletonMono<CameraManager>
{
    public Animator Anim;
    private string curCamera = ConstValues.CINEMACHINE_ANIM_MAIN_MENU;
    public CinemachineVirtualCamera PlayingCamera;
    private CinemachineTransposer transposer;
    private Vector3 defaultCameraOffset;
    private Camera mainCam;
    private int defaultCullingMask;

    private void Start()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;

        mainCam = Camera.main;
        defaultCullingMask = mainCam.cullingMask;

        transposer = PlayingCamera.GetCinemachineComponent<CinemachineTransposer>();
        defaultCameraOffset = transposer.m_FollowOffset;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }
    private void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                ChangeCamera(ConstValues.CINEMACHINE_ANIM_MAIN_MENU);
                break;
            case GameState.Playing:
                ChangeCamera(ConstValues.CINEMACHINE_ANIM_PLAYING);
                break;
            case GameState.SkinShop:
                ChangeCamera(ConstValues.CINEMACHINE_ANIM_SHOPPING);
                break;
            case GameState.LoadLevel:
                ResetCameraPosition();
                break;
            default:
                break;
        }
    }
    private void ChangeCamera(string camera)
    {
        if (curCamera != camera)
        {
            Anim.Play(camera);
            curCamera = camera;
        }
    }
    public void ZoomOutCamera()
    {
        transposer.m_FollowOffset += ConstValues.VALUE_CHARACTER_UP_SIZE_RATIO * defaultCameraOffset;
    }
    public void ResetCameraPosition()
    {
        transposer.m_FollowOffset = defaultCameraOffset;
    }
}
