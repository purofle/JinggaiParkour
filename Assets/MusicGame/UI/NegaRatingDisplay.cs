using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NegaRankingDisplay : MonoBehaviour
{
    public Sprite[] Presents;
    public BeatmapManager beatmapManager;

    void Update()
    {
        gameObject.GetComponent<Image>().sprite = Presents[beatmapManager.GetNegaRating()];
    }
}
