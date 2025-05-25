using System;
using System.Linq;
using Lofelt.NiceVibrations;
using Unity.VisualScripting;
using UnityEngine;

public class MusicObstacle : MonoBehaviour
{
    public int index = 0;
    private bool isTouched = false;
    // private bool isPerfect = false;
    private bool isInit = true;
    private bool isShow = false;
    private bool isLast = false;
    public bool isBest = false;
    bool forcePerfect = false;
    public int[] track;
    public GameObject player;
    public GameObject camera;
    public GameObject perfectboom;
    public GameObject greatboom;
    public AudioSource bestSound;
    public GameObject boomSounds;
    public BeatmapManager beatmapManager;
    // Start is called before the first frame update
    void Start()
    {
        if(DataStorager.settings.cinemaMod || track.Count() <= 0 || isShow){
            isTouched = true;
            forcePerfect = true;
        }

        // 愚人节彩蛋
        if(DateTime.Now.Day == 1 && DateTime.Now.Month == 4){
            isTouched = true;
            forcePerfect = true;
        }
    }

    void GenerateBoom(GameObject theboom){
        camera.GetComponent<MusicCamera>().triggerShake();
        var newboom = Instantiate(theboom);
        newboom.transform.position = gameObject.transform.position + new Vector3(0,2.5f,0);

        // 愚人节彩蛋
        if(DateTime.Now.Day == 1 && DateTime.Now.Month == 4){
            newboom.transform.localScale *= 100;
            newboom.transform.position += new Vector3(0,0,20) * player.GetComponent<Player>().GetVelocity() / 50;
        }

        // 播放声音
        boomSounds.transform.GetChild(index % boomSounds.transform.childCount).GetComponent<AudioSource>().Play();
        if (isBest)
        {
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

    public void setShowNote(){
        isShow = true;
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
            if(player.transform.position.z - gameObject.transform.position.z <= 1.25 * (player.GetComponent<Player>().GetVelocity() / 50) || forcePerfect){
                // Perfect
                GenerateBoom(perfectboom);
                beatmapManager.AddNowPoint(BeatmapManager.M_TYPE.Perfect,!isShow);
                if(isBest){
                    beatmapManager.AddNowBest(BeatmapManager.M_TYPE.Break_P,!isShow);
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
                    beatmapManager.AddNowPoint(BeatmapManager.M_TYPE.Great,!isShow);
                    if(isBest){
                        beatmapManager.AddNowBest(BeatmapManager.M_TYPE.Break_G,!isShow);
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
            beatmapManager.AddNowPoint(BeatmapManager.M_TYPE.Miss,!isShow);
            if(isBest){
                beatmapManager.AddNowBest(BeatmapManager.M_TYPE.Break_M,!isShow);
            }
            beatmapManager.Miss();
            if(isLast){
                triggerEnd();
            }
            Destroy(gameObject);
            return;
        }
    }
}
