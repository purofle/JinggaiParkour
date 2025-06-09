using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestSound : MonoBehaviour
{
    public void UpdateSound()
    {
        gameObject.GetComponent<AudioSource>().clip = SoundManager.best_sound;
    }
}
