using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip init_boom_sound;
    public static AudioClip boom_sound;

    public AudioClip init_best_sound;
    public static AudioClip best_sound;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        boom_sound = init_boom_sound;
        best_sound = init_best_sound;
    }
}
