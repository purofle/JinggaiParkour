using System;
using System.Linq;
using Lofelt.NiceVibrations;
using UnityEngine;

public class MusicObstacle : MonoBehaviour
{

    private bool isTouched = false;
    // private bool isPerfect = false;
    private bool isInit = true;
    private bool isLast = false;
    public bool isBest = false;
    public int[] track;
    public GameObject player;
    public GameObject camera;
    public GameObject perfectboom;
    public GameObject greatboom;
    public AudioSource bestSound;
    public BeatmapManager beatmapManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void GenerateBoom(GameObject theboom){
        camera.GetComponent<MusicCamera>().triggerShake();
        var newboom = Instantiate(theboom);
        newboom.transform.position = gameObject.transform.position + new Vector3(0,2.5f,0);

        if(isBest){
            bestSound.Play();
        }

        if(!DataStorager.settings.notVibrate){
            HapticPatterns.PlayEmphasis(1.0f, 0.0f);
        }

    }

    public void setNote(){
        isInit = false;
    }

     public void setBestNote(){
        isBest = true;
    }

    public void setLastNote(){
        isLast = true;
    }

    void triggerEnd() {
        beatmapManager.triggerEnd();
    }

    bool isOnTrack() {
        if(track.Contains(player.GetComponent<Player>().GetNowTrack())){
            return true;
        }
        foreach(var inputImp in player.GetComponent<Player>().inputImpluses){
            if(track.Contains(inputImp.track)){
                player.GetComponent<Player>().inputImpluses.Remove(inputImp);
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Math.Abs(player.transform.position.z - gameObject.transform.position.z) < 2 * (player.GetComponent<Player>().GetVelocity() / 50)
          && isOnTrack()
          && Math.Abs(player.transform.position.y - gameObject.transform.position.y) < 2
        ){
            isTouched = true;
        }
        // 下落墩子也可以
        if(Math.Abs(player.transform.position.z - gameObject.transform.position.z) < 3.5 * (player.GetComponent<Player>().GetVelocity() / 50)
            && player.GetComponent<Player>().isDroping()
            && isOnTrack()
            && player.transform.position.y >= gameObject.transform.position.y
        ){
            isTouched = true;
        }

        if(player.transform.position.z >= gameObject.transform.position.z && isTouched && !isInit)
        {
            if(player.transform.position.z - gameObject.transform.position.z <= 1.25 * (player.GetComponent<Player>().GetVelocity() / 50)){
                // Perfect
                GenerateBoom(perfectboom);
                beatmapManager.AddNowPoint(1);
                if(isBest){
                    beatmapManager.AddNowBest(1);
                }
                if(isLast){
                    triggerEnd();
                }
                Destroy(gameObject);
                return;
            } else {
                // Great
                {
                    GenerateBoom(greatboom);
                    beatmapManager.AddNowPoint(0.95f);
                    if(isBest){
                        beatmapManager.AddNowBest(0.95f);
                    }
                    if(isLast){
                        triggerEnd();
                    }
                    Destroy(gameObject);
                    return;
                }
            }
        }


        if(player.transform.position.z - gameObject.transform.position.z > 10 * (player.GetComponent<Player>().GetVelocity() / 50)
            && !isInit){
            // GenerateBoom();
            beatmapManager.Miss();
            if(isLast){
                triggerEnd();
            }
            Destroy(gameObject);
            return;
        }
    }
}
