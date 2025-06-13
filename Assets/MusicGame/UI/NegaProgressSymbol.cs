using UnityEngine;
using UnityEngine.UI;

public class NegaProgressSymbol : MonoBehaviour
{
    public BeatmapManager beatmapManager;
    public Sprite[] Presents;

    void Update()
    {
        float proress = beatmapManager.GetNegaProgress();
        if(proress < 0.6){
            gameObject.GetComponent<Image>().sprite = Presents[4];
        }
        else if(proress < 0.8){
            gameObject.GetComponent<Image>().sprite = Presents[3];
        }
        else if(proress < 0.97){
            gameObject.GetComponent<Image>().sprite = Presents[2];
        }
        else if(proress < 1){
            gameObject.GetComponent<Image>().sprite = Presents[1];
        } else {
            gameObject.GetComponent<Image>().sprite = Presents[0];
        }
    }
}
