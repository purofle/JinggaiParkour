using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayer : MonoBehaviour
{
    public enum Difficulty
    {
        SO_POWERFUL,
        POWERFUL,
        HARD,
        NORMAL,
        EASY,
        FUN,
        NONE
    }

    public Sprite[] LevelPresents;
    public Difficulty difficulty = Difficulty.NONE;
    public int level = 0;
    public TMP_Text level_object;
    // Start is called before the first frame update
    void Start()
    {
        level_object.text = level.ToString();
        // 评级
        gameObject.GetComponent<Image>().sprite = LevelPresents[(int)difficulty];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
