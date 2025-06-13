using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointDisplay : MonoBehaviour
{
    public BeatmapManager beatmapManager;
    public TMP_Text perfect_text;
    public TMP_Text great_text;
    public TMP_Text miss_text;
    public TMP_Text combo_text;
    void Start()
    {
        BeatmapManager.Point_Detail point_detail = beatmapManager.GetPointDetail();
        perfect_text.text = point_detail.perfect.ToString();
        great_text.text = point_detail.great.ToString();
        miss_text.text = point_detail.miss.ToString();
        combo_text.text = beatmapManager.GetMaxCombo().ToString();
    }
}
