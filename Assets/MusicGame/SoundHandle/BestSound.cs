using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestSound : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<AudioSource>().clip = SoundManager.best_sound;
    }
}
