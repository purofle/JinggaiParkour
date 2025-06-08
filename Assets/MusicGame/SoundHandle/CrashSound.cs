using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSound : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<AudioSource>().clip = SoundManager.boom_sound;
    }
}
