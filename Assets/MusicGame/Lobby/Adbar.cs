using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Adbar : MonoBehaviour
{
    public string targetURL = "";
    public string additionalText = "";
    public string imagePath = "";

    public RawImage logoImage;
    public TMP_Text text;

    private void Awake()
    {
        if (File.Exists(imagePath))
        {
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // 自动调整纹理大小
            logoImage.texture = texture;
            logoImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;
        }
        text.text = additionalText;
    }

    public void OpenUrl()
    {
        if (targetURL.Length > 0)
        {
            Application.OpenURL(targetURL);
        }
    }
}
