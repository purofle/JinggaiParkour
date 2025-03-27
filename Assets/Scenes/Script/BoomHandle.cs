using System.Collections;
using UnityEngine;

public class BoomHandle : MonoBehaviour
{
    public float destroyTime = 5.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        if(!DataStorager.settings.notBoomFX){
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(DestroyAfterDelay());
    }
    void Start()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
