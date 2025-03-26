using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleBeatmapInfo : MonoBehaviour
{
    public string path = "";
    public string title = "";
    public string description = "";
    public Sprite[] Presents;
    public Sprite[] LevelPresents;
    public int max_rating;
    public float level;
    public TMP_Text title_object;
    public TMP_Text descrip_object;
    public TMP_Text level_object;
    public Image backImage;
    public Image Rating;
    public Image levelImage;
    public GameObject deleteButton;

    string dataFolder;

    private void Awake() {
        dataFolder = $"{Application.persistentDataPath}/music";
    }

    public void setBackground(Texture2D texture) {
        backImage.sprite = Sprite.Create(texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        backImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;
    }
    void GetReady(string path){
        BeatmapInfo.beatmap_name = path;
        LoadMaplist.OpenDisplayPanel();
    }

    void DeleteMap(string path){
        FileBrowserHelpers.DeleteDirectory($"{dataFolder}/{path}");
        SceneManager.LoadScene("MusicLobby");
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => GetReady(path));
        deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteMap(path));
        title_object.text = title;
        descrip_object.text = description;
        if(max_rating < 15){
            Rating.sprite = Presents[max_rating];
        } else {
            Rating.sprite = null;
            Rating.color = new Color(0,0,0,0);
        }
        level_object.text = level.ToString();
        // 评级
        if(level < 6){
            levelImage.sprite = LevelPresents[3];
        } else if (level < 10){
            levelImage.sprite = LevelPresents[2];
        } else if (level < 13){
            levelImage.sprite = LevelPresents[1];
        } else {
            levelImage.sprite = LevelPresents[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        deleteButton.SetActive(LoadMaplist.IsDeleting());
    }
}
