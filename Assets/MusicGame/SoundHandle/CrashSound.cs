using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSound : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<AudioSource>().clip = SoundManager.boom_sound;
        gameObject.GetComponent<AudioSource>().Play();
    }
}
