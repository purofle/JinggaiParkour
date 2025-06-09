using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class SyncRanking : MonoBehaviour
{
    public TMP_Text ranking;
    public TMP_Text score_list;

    public TMP_ColorGradient[] gradients;

    float now_score;
    List<float> all_score = new();

    void Update()
    {
        all_score.Clear();
        foreach (PlayerInfo playerInfo in FindObjectsOfType<PlayerInfo>())
        {
            Debug.Log(all_score);
            all_score.Add(playerInfo.achievement * 100);
            if (playerInfo.isLocalPlayer)
            {
                now_score = playerInfo.achievement * 100;
            }
        }
        all_score.Sort();
        all_score.Reverse();
        int rank = all_score.IndexOf(now_score) + 1;
        ranking.text = "#" + rank.ToString();
        if (rank >= 5)
        {
            ranking.colorGradientPreset = gradients[4];
        }
        else
        {
            if (rank >= 1)
            {
                ranking.colorGradientPreset = gradients[rank - 1];
            }
        }
        score_list.text = "";
        for(int i = 0; i < all_score.Count; i++)
        {
            score_list.text += $"#{i + 1} {all_score[i]:0.0000}%\n";
        }
    }
}
