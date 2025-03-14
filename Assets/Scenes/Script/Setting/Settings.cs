using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GlobalSettings globalSettings;
    public Slider MusicVolume;
    public Slider SoundVolume;
    public Toggle hasMotionBlur;
    public Text MaxLife;
    public InputField CustomMaxLife;
    public Toggle notShake;
    public InputField MusicGameSpeed;
    public Toggle isAutoPlay;
    public Toggle notVibrate;
    void Awake(){
        MusicVolume.value = DataStorager.settings.MusicVolume;
        SoundVolume.value = DataStorager.settings.SoundVolume;
        hasMotionBlur.isOn = DataStorager.settings.hasMotionBlur;
        notShake.isOn = DataStorager.settings.notShake;
        isAutoPlay.isOn = DataStorager.settings.isAutoPlay;
        notVibrate.isOn = DataStorager.settings.notVibrate;
        MaxLife.text = DataStorager.maxLife.count.ToString();
        if(DataStorager.settings.CustomMaxLife > 0){
            CustomMaxLife.text = DataStorager.settings.CustomMaxLife.ToString();
        } else {
            CustomMaxLife.text = DataStorager.maxLife.count.ToString();
        }
        if(DataStorager.settings.MusicGameSpeed > 0){
            MusicGameSpeed.text = DataStorager.settings.MusicGameSpeed.ToString();
        } else {
            MusicGameSpeed.text = "1";
        }
    }

    public void SaveAndExit(){
        DataStorager.settings.MusicVolume = MusicVolume.value;
        DataStorager.settings.SoundVolume = SoundVolume.value;
        DataStorager.settings.hasMotionBlur = hasMotionBlur.isOn;
        DataStorager.settings.notShake = notShake.isOn;
        DataStorager.settings.notVibrate = notVibrate.isOn;
        DataStorager.settings.isAutoPlay = isAutoPlay.isOn;
        if (!int.TryParse(CustomMaxLife.text, out int clife))
        {
            DataStorager.settings.CustomMaxLife = DataStorager.maxLife.count;
        }
        else
        {
            if (clife > 0 && clife <= DataStorager.maxLife.count)
            {
                DataStorager.settings.CustomMaxLife = clife;
            }
            else
            {
                DataStorager.settings.CustomMaxLife = DataStorager.maxLife.count;
            }
        }
        if (!float.TryParse(MusicGameSpeed.text, out float cspeed))
        {
            DataStorager.settings.MusicGameSpeed = 1;
        }
        else
        {
            if (cspeed > 0)
            {
                DataStorager.settings.MusicGameSpeed = cspeed;
            }
            else
            {
                DataStorager.settings.MusicGameSpeed = 1;
            }
        }
        // 保存
        DataStorager.SaveSettings();
        globalSettings.handleSettings();
        SceneManager.LoadScene("Initalize");
    }
}
