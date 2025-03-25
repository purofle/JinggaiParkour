using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordItem : MonoBehaviour
{
    public BeatmapManager.BeatmapResult beatmapResult;
    public TMP_Text maxCombo;
    public TMP_Text achievement;
    public TMP_Text playTime;

    // Start is called before the first frame update
    void Start()
    {
        maxCombo.text += beatmapResult.maxCombo.ToString();
        achievement.text = beatmapResult.achievement.ToString("0.0000") + "%";
        DateTime utcDateTime = new(beatmapResult.achieveTime * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
        DateTime localDateTime = utcDateTime.ToLocalTime();
        playTime.text = localDateTime.ToString("yyyy/MM/dd hh:mm:ss");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
