using Unity.Mathematics;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        gameObject.GetComponentInParent<AudioSource>().Play();
        DataStorager.coin.count += 1;
        Destroy(gameObject);
    }

    public void HandleTrigger(Collider other){
        OnTriggerEnter(other);
    }
}
