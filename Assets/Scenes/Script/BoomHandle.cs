using System.Collections;
using UnityEngine;

public class BoomHandle : MonoBehaviour
{
    public float destroyTime = 5.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyAfterDelay());
    }
    void Start()
    {

    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
