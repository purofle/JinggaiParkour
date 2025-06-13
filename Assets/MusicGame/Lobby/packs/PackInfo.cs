using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static OverAll;





public class PackInfo : MonoBehaviour
{
    public struct PackData
    {
        public string title;
        public string desc;
        public string link;
    }

    public PackData packData;

    public TMP_Text title;
    public TMP_Text desc;
    public Button downloadButton;
    public GameObject hoverImage;
    public Sprite[] sprites;
    void Start()
    {
        title.text = packData.title;
        desc.text = packData.desc;
    }

    public void TriggerDownload() {
        downloadButton.enabled = false;
        downloadButton.image.sprite = sprites[1];
        hoverImage.SetActive(true);
        StartCoroutine(DownLoad());
    }

    public IEnumerator DownLoad()
    {
        string url = packData.link;
        string downloadFolder = $"{Application.persistentDataPath}/download";
        if (!Directory.Exists(downloadFolder))
        {
            Directory.CreateDirectory(downloadFolder);
        }
        string desFileName = $"{downloadFolder}/{packData.title}.sdp";
        if (File.Exists(desFileName))
        {
            File.Delete(desFileName);
        }
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.downloadHandler = new DownloadHandlerFile(desFileName);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                Vector3 oriScale = hoverImage.transform.localScale;
                oriScale.y = 1 - request.downloadProgress;
                hoverImage.transform.localScale = oriScale;
                yield return null;
            }

            if (request.isDone)
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    downloadButton.image.sprite = sprites[4];
                    downloadButton.enabled = true;
                }
                else
                    downloadButton.image.sprite = sprites[2];
            }
        }
        if (File.Exists(desFileName))
        {
            FileBrowserSet.LoadMapPacks(desFileName);
            File.Delete(desFileName);
        }
    }
}
