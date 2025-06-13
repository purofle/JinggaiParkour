using System.Collections;
using UnityEngine;

public class MusicBoomHandle : MonoBehaviour
{
    public float destroyTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        // gameObject.GetComponent<AudioSource>().Play();
        if(!DataStorager.settings.notBoomFX){
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(DestroyAfterDelay());
    }
    void Start()
    {
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime * 50 / Player.targetPlayer.GetVelocity());
        Destroy(gameObject);
    }
}
