using TMPro;
using UnityEngine;

public class NegaProgressText : MonoBehaviour
{
    public BeatmapManager beatmapManager;
    public TMP_ColorGradient[] colorPresents;

    void Update()
    {
        float proress = beatmapManager.GetNegaProgress();
        if(proress < 0.6){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[4];
        }
        else if(proress < 0.8){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[3];
        }
        else if(proress < 0.97){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[2];
        }
        else if(proress < 1){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[1];
        } else {
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[0];
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = (beatmapManager.GetNegaProgress() * 100).ToString("0.0000");
    }
}
