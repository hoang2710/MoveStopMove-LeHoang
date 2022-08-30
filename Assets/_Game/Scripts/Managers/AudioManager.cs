using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    public AudioSource AudioSource;

    public List<AudioData> AudioDatas;
    private Dictionary<AudioType, AudioClip> AudioDictionary = new Dictionary<AudioType, AudioClip>();

    private void Start()
    {
        InitAudioData();
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
    public void SetAudioStatus(bool isMute)
    {
        AudioSource.enabled = !isMute;

        DataManager.Instance.SaveToggleSetting(DataKeys.SOUND_SETTING, !isMute);
    }
    public void SetVibrateStatus(bool isOn)
    {
        DataManager.Instance.SaveToggleSetting(DataKeys.VIBRATE_SETTING, isOn);
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