using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSound : MonoBehaviour
{
    public void UpdateSound()
    {
        gameObject.GetComponent<AudioSource>().clip = SoundManager.boom_sound;
    }
}
