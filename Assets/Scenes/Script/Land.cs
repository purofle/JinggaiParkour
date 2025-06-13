using UnityEngine;

public class Land : MonoBehaviour
{
    private GameObject dunzi;
    private bool destoryable = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dunzi = GlobalTargetManager.real_dunzi;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z - dunzi.transform.position.z < -20 && destoryable){
            Destroy(gameObject);
        }
    }

    public void SetDestoryable(bool value = true){
        destoryable = value;
    }
}
