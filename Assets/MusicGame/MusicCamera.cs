using UnityEngine;
using System;
public class MusicCamera : MonoBehaviour
{
    public GameObject center;
    private Vector3 offset;
    private float move_timer = 0f;
    private float shake_timer = 1f;
    private bool toShake = false;
    private const float CROSS_TIME = 1f;
    private const float SHAKE_TIME = 0.5f;

    private bool toRotate = false;
    private float ori_rotateAngle = 0;
    private float rotateAngle = 0;
    private float ROTATE_CROSS_TIME = 1f;
    private float rotate_timer = 1f;

    void Start()
    {
        offset = transform.position - center.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = center.transform.position + offset;
        moveCamera();
    }

    public void triggerShake()
    {
        if(!DataStorager.settings.notShake){
            toShake = true;
        }
    }

    public void triggerRotate(float angle, float time = 1f)
    {
        rotateAngle = angle;
        ROTATE_CROSS_TIME = time;
        toRotate = true;
    }

    void moveCamera()
    {
        if (toShake)
        {
            shake_timer = 0f;
            toShake = false;
        }
        if (shake_timer < SHAKE_TIME)
        {
            transform.position += new Vector3(UnityEngine.Random.Range(shake_timer / SHAKE_TIME - 1, 1 - shake_timer / SHAKE_TIME), UnityEngine.Random.Range(shake_timer / SHAKE_TIME - 1, 1 - shake_timer / SHAKE_TIME));
        }
        shake_timer += Time.deltaTime;

        if (toRotate)
        {
            ori_rotateAngle = transform.rotation.eulerAngles.z;
            rotate_timer = 0f;
            toRotate = false;
        }
        if (rotate_timer < ROTATE_CROSS_TIME)
        {
            Vector3 ori = transform.rotation.eulerAngles;
            double progress = 1 - Math.Pow(1 - rotate_timer / ROTATE_CROSS_TIME, 4);
            ori.z = (float)(progress * rotateAngle + (1 - progress) * ori_rotateAngle);
            transform.rotation = Quaternion.Euler(ori); ;
        }
        rotate_timer += Time.deltaTime;
    }

    Vector3 CalcOffsetByTimer()
    {
        float t = move_timer += Time.deltaTime;
        return offset * (float)(1 - Math.Pow(1 - (t / CROSS_TIME), 4));
    }
}
