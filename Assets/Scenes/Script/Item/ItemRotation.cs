using Unity.Mathematics;
using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    private float angle = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * 100;
        gameObject.transform.rotation = Quaternion.Euler(0,angle,0);
    }
}
