using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class RecordItem : MonoBehaviour
{
    public BeatmapManager.BeatmapResult beatmapResult;
    public TMP_Text maxCombo;
    public TMP_Text achievement;
    public TMP_Text playTime;
    public Image Rating;
    public Sprite[] Presents;
    // Start is called before the first frame update
    void Start()
    {
        maxCombo.text += beatmapResult.maxCombo.ToString();
        achievement.text = beatmapResult.achievement.ToString("0.0000") + "%";
        DateTime utcDateTime = new(beatmapResult.achieveTime * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
        DateTime localDateTime = utcDateTime.ToLocalTime();
        playTime.text = localDateTime.ToString("yyyy/MM/dd HH:mm:ss");
        int max_rating = beatmapResult.rating;
        if(max_rating < 15){
            Rating.sprite = Presents[max_rating];
        } else {
            Rating.sprite = null;
            Rating.color = new Color(0,0,0,0);
        }
    }
}
