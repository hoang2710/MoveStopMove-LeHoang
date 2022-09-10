using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>, IDataHandler
{
    public AudioSource AudioSource;

    public List<AudioData> AudioDatas;
    private Dictionary<AudioType, AudioClip> AudioDictionary = new Dictionary<AudioType, AudioClip>();

    public bool IsSoundOn { get; private set; }
    public bool IsVibrateOn { get; private set; }

    private void Start()
    {
        InitAudioData();
        DataManager.Instance.AssignDataHandler(this);
    }

    private void InitAudioData()
    {
        foreach (var item in AudioDatas)
        {
            AudioDictionary.Add(item.AudioType, item.AudioClip);
        }
    }
    public void PlayAudioClip(AudioType audioType)
    {
        AudioSource.PlayOneShot(AudioDictionary[audioType]);
    }
    public void MakeVibration()
    {
        if (IsVibrateOn)
        {
            Handheld.Vibrate();
        }
    }
    public void SetAudioStatus(bool isMute)
    {
        IsSoundOn = !isMute;
        AudioSource.enabled = !isMute;
    }
    public void SetVibrateStatus(bool isOn)
    {
        IsVibrateOn = isOn;
    }

    public void LoadData(GameData data)
    {
        IsSoundOn = data.IsSoundOn;
        IsVibrateOn = data.IsVibrateOn;

        AudioSource.enabled = IsSoundOn;
    }

    public void SaveData(GameData data)
    {
        data.IsSoundOn = IsSoundOn;
        data.IsVibrateOn = IsVibrateOn;
    }
}

[System.Serializable]
public class AudioData
{
    public AudioType AudioType;
    public AudioClip AudioClip;
}
public enum AudioType
{
    ButtonClick,
    Lose,
    Win,
    CountDown,
    Die1,
    Die2,
    Die3,
    Die4,
    DieExplode,
    Hit,
    ThrowWeapon,
    SizeUp
}