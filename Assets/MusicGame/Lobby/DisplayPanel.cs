using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class DisplayPanel : MonoBehaviour
{
    public Sprite[] Presents;
    public TMP_Text Level;
    public TMP_Text title;
    public TMP_Text description;
    public GameObject recordItem;
    public GameObject recordItemFather;
    // Start is called before the first frame update
    void Start()
    {
        LoadPanel();
    }

    public void LoadPanel(){
        string beat_path = $"{Application.persistentDataPath}/music/{BeatmapInfo.beatmap_name}/data.sdz";
        string author = "";
        string mapper = "";
        foreach ( string line in File.ReadAllText(beat_path).Split("\n")){
            string[] data = line.Split("=");
            if(data[0].Replace(" ","") == "title"){
                title.text = data[1].Replace(" ","");
                continue;
            }
            if(data[0].Replace(" ","") == "author"){
                author = data[1].Replace(" ","");
                continue;
            }
            if(data[0].Replace(" ","") == "mapper"){
                mapper = data[1].Replace(" ","");
                continue;
            }
            if(data[0].Replace(" ","") == "level"){
                Level.text = data[1].Replace(" ","");
                continue;
            }
        }
        description.text = $"曲师：{author}\n谱师：{mapper}";

        string record_path = $"{Application.persistentDataPath}/record/{BeatmapInfo.beatmap_name}.dat";
        for(int j = 0;j < recordItemFather.transform.childCount;j++){
            Destroy(recordItemFather.transform.GetChild(0).gameObject);
        }
        if(File.Exists(record_path)){
            var data_list = JsonConvert.DeserializeObject<List<BeatmapManager.BeatmapResult>>(File.ReadAllText(record_path));
            foreach(BeatmapManager.BeatmapResult result in data_list){
                GameObject newItem = Instantiate(recordItem,recordItemFather.transform);
                newItem.SetActive(true);
                newItem.GetComponent<RecordItem>().beatmapResult = result;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
