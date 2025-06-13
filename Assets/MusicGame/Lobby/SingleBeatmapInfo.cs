using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static LoadMaplist;

public class SingleBeatmapInfo : MonoBehaviour
{
    public AnBeatmapInfo beatmapInfo;
    public List<AnBeatmapInfo> beatmapInfos;
    public Sprite[] Presents;
    public Sprite[] LevelPresents;
    public TMP_Text title_object;
    public TMP_Text descrip_object;
    public TMP_Text level_object;
    public Image leftBackImage;
    public Image rightBackImage;
    public Image backImage;
    public Image Rating;
    public Image levelImage;
    public GameObject deleteButton;
    public GameObject deleteAllButton;

    public GameObject diffUpButton;
    public GameObject diffDownButton;

    public int diff_index = 0;

    string dataFolder;

    private void Awake() {
        dataFolder = $"{Application.persistentDataPath}/music";
    }

    public enum Margin_Type {
        MIDDLE,
        LEFT,
        RIGHT
    }

    public void SetBackground(Texture2D texture, float alpha = (float)185 / 255, Margin_Type margin_Type = Margin_Type.MIDDLE) {
        if (margin_Type == Margin_Type.MIDDLE)
        {
            backImage.sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            Color new_color = backImage.color;
            new_color.a = alpha;
            backImage.color = new_color;
            backImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;
        }
        else
        {
            backImage.gameObject.SetActive(false);
            Image image = margin_Type switch
            {
                Margin_Type.LEFT => leftBackImage,
                Margin_Type.RIGHT => rightBackImage,
                _ => leftBackImage,
            };
            ;
            image.gameObject.SetActive(true);
            Color new_color = image.color;
            new_color.a = alpha;
            image.color = new_color;
            image.sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }
    }
    void GetReady(string path){
        BeatmapInfo.beatmap_name = path;
        LoadMaplist.instance.OpenDisplayPanel();
    }

    void DeleteMap(string path, bool reload = true){
        FileBrowserHelpers.DeleteDirectory($"{dataFolder}/{path}");
        if (reload)
        {
            SceneManager.LoadScene("MusicLobby");
        }
    }

    void DeleteMapWithAllLevels()
    {
        foreach (AnBeatmapInfo temp_beatmapInfo in beatmapInfos)
        {
            DeleteMap(temp_beatmapInfo.path, false);
        }
        SceneManager.LoadScene("MusicLobby");
    }

    public void LoadBeatmapInfo()
    {
        beatmapInfo = beatmapInfos[diff_index];

        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.GetComponent<Button>().onClick.AddListener(() => GetReady(beatmapInfo.path));
        deleteButton.GetComponent<Button>().onClick.RemoveAllListeners();
        deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteMap(beatmapInfo.path));
        deleteAllButton.GetComponent<Button>().onClick.RemoveAllListeners();
        deleteAllButton.GetComponent<Button>().onClick.AddListener(() => DeleteMapWithAllLevels());

        title_object.text = beatmapInfo.title;
        descrip_object.text = $"曲师：{beatmapInfo.author}\n谱师：{beatmapInfo.mapper}";

        int max_rating = 100;
        if (File.Exists($"{Application.persistentDataPath}/record/{beatmapInfo.path}.dat"))
        {
            var data_list = JsonConvert.DeserializeObject<List<BeatmapManager.BeatmapResult>>(File.ReadAllText($"{Application.persistentDataPath}/record/{beatmapInfo.path}.dat"));
            foreach (BeatmapManager.BeatmapResult result in data_list)
            {
                max_rating = Math.Min(max_rating, result.rating);
            }
        }
        if (max_rating < 15)
        {
            Rating.sprite = Presents[max_rating];
        }
        else
        {
            Rating.sprite = null;
            Rating.color = new Color(0, 0, 0, 0);
        }
        level_object.text = beatmapInfo.level.ToString();
        // 评级
        levelImage.sprite = LevelPresents[(int)beatmapInfo.difficulty];
    }

    public void SwitchDiff(int delta){
        diff_index += delta;
        LoadBeatmapInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (diff_index <= 0)
        {
            diffDownButton.SetActive(false);
        }
        else
        {
            diffDownButton.SetActive(true);
        }
        if (diff_index >= beatmapInfos.Count - 1)
        {
            diffUpButton.SetActive(false);
        }
        else
        {
            diffUpButton.SetActive(true);
        }
        deleteButton.SetActive(LoadMaplist.IsDeleting());
        deleteAllButton.SetActive(LoadMaplist.IsDeleting());
    }
}
