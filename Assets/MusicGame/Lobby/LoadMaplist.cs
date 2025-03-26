using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMaplist : MonoBehaviour
{
    struct BeatmapInfo {
        public string path;
        public string title;
        public string author;
        public string mapper;
        public float BPM;
        public float level;
    }

    private string dataFolder;
    private List<BeatmapInfo> beatmapInfos = new();

    public GameObject SingleItem;
    public GameObject MapList;
    public GameObject t_DisplayPanel;
    public static GameObject DisplayPanel;

    static bool isDeleteState = false;
    public Text deleteStateButtonText;

    private void Awake() {
        DisplayPanel = t_DisplayPanel;
        dataFolder = $"{Application.persistentDataPath}/music";
        if(!Directory.Exists(dataFolder)){
            Directory.CreateDirectory(dataFolder);
        }
        string[] subfolderPaths = Directory.GetDirectories(dataFolder, "*", SearchOption.TopDirectoryOnly);
        foreach (string path in subfolderPaths){
            if(!File.Exists($"{path}/data.sdz")){
                continue;
            }
            string folderName = Path.GetFileName(path);
            string beat_path = $"{path}/data.sdz";
            BeatmapInfo info = new()
            {
                path = folderName
            };
            foreach ( string line in File.ReadAllText(beat_path).Split("\n")){
                string[] data = line.Split("=");
                if(data[0].Replace(" ","") == "title"){
                    info.title = data[1].Replace(" ","");
                    continue;
                }
                if(data[0].Replace(" ","") == "bpm"){
                    info.BPM = float.Parse(data[1].Replace(" ",""));
                    continue;
                }
                if(data[0].Replace(" ","") == "author"){
                    info.author = data[1].Replace(" ","");
                    continue;
                }
                if(data[0].Replace(" ","") == "mapper"){
                    info.mapper = data[1].Replace(" ","");
                    continue;
                }
                if(data[0].Replace(" ","") == "level"){
                    info.level = float.Parse(data[1].Replace(" ",""));
                    continue;
                }
            }
            beatmapInfos.Add(info);
        }
        bool init = false;
        foreach (BeatmapInfo info in beatmapInfos){
            GameObject item;
            if(!init){
                item = SingleItem;
                init = true;
            } else {
                item = Instantiate(SingleItem, MapList.transform);
            }
            item.GetComponent<SingleBeatmapInfo>().title = info.title;
            item.GetComponent<SingleBeatmapInfo>().description = $"曲师：{info.author}\n谱师：{info.mapper}";
            item.GetComponent<SingleBeatmapInfo>().path = info.path;
            item.GetComponent<SingleBeatmapInfo>().level = info.level;
            if(File.Exists($"{dataFolder}/{info.path}/bg.png")){
                byte[] fileData = File.ReadAllBytes($"{dataFolder}/{info.path}/bg.png");
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                item.GetComponent<SingleBeatmapInfo>().setBackground(texture);
            }
            int max_rating = 100;
            if(File.Exists($"{Application.persistentDataPath}/record/{info.path}.dat")){
                var data_list = JsonConvert.DeserializeObject<List<BeatmapManager.BeatmapResult>>(File.ReadAllText($"{Application.persistentDataPath}/record/{info.path}.dat"));
                foreach(BeatmapManager.BeatmapResult result in data_list){
                    max_rating = Math.Min(max_rating,result.rating);
                }
            }
            item.GetComponent<SingleBeatmapInfo>().max_rating = max_rating;
        }
        // 无谱面则隐藏
        if(!init){
            SingleItem.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDeleteState){
            deleteStateButtonText.text = "删除谱面";
        } else {
            deleteStateButtonText.text = "取消删除";
        }
    }

    public static void OpenDisplayPanel(){
        DisplayPanel.SetActive(true);
        DisplayPanel.GetComponent<DisplayPanel>().LoadPanel();
    }

    public static void TurnOffDisplayPanel(){
        DisplayPanel.SetActive(false);
    }

    public static void GameStart(){
        SceneManager.LoadScene("MusicGame");
    }

    public static bool IsDeleting(){
        return isDeleteState;
    }

    public static void ChangeDeleteState(){
        isDeleteState = !isDeleteState;
    }
}
