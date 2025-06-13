using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    // public Rigidbody the_rigidbody;
    public GameObject center;
    public GameObject BGM;
    public GameObject realDunzi;
    public AudioSource hurt;
    public Animator animator;
    public Animator self_animatior;

    public Animator hurtUI;
    private Vector3 velocity;

    public const int MAX_TRACKS = 3;
    public const int TRACK_WIDTH = 3;
    public const float MOVE_SPEED = 20f;
    public float speedTimes = 1f;
    public const float CROSS_TIME = 0.4f;
    public int now_track = 2;
    public bool alive = true;
    public List<string> buffTags = new();
    public List<float> buffTimes = new();
    public const float Gravity = 50f;

    public bool invincible = false;
    private bool trigger_die = false;

    private float jumpStrength = 15f;
    private bool toMoving = false;
    private bool isMoving = false;
    private bool isFlying = false;
    private float should_pos = 0;
    private float delta_pos = 0;
    private float origin_pos = 0;
    private float all_timer = 0;
    private int life = 1;
    private float offsetMiles = 0;
    private List<FromTo> movementList = new();
    
	private float maxReachedSpeed = 0; // 最大速度显示

    private float move_timer = 0f; // 计时器
    // Star is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(DataStorager.settings.CustomMaxLife > 0){
            life = DataStorager.settings.CustomMaxLife;
        } else {
            life = DataStorager.maxLife.count;
        }
        if(invincible){
            addBuff("invincible",100000);
        }
        // addBuff("invincible",100000);
        // SpeedUp(10);
    }

    public int GetLife(){
        return life;
    }

    public Vector3 GetVelocity(){
        return velocity;
    }

    	
	public float GetDisplaySpeed(){
		maxReachedSpeed = Math.Max(GetVelocity().z,maxReachedSpeed);
		return maxReachedSpeed;
	}

    public void AddOffsetMiles(float miles){
        offsetMiles += miles;
    }

    public float GetMiles(){
        return gameObject.transform.position.z + offsetMiles;
    }

    public bool ConsumeLife(int count = 1){
        if(count >= life){
            life = 0;
            return false;
        } else {
            hurtUI.SetTrigger("TriggerHurt");
            hurt.Play();
            life -= count;
            return true;
        }
    }

    public void addBuff(string buffname, float time)
    {
        if (buffTags.Contains(buffname))
        {
            buffTimes[buffTags.IndexOf(buffname)] += time;
        }
        else
        {
            buffTags.Add(buffname);
            self_animatior.SetTrigger(buffname);
            buffTimes.Add(time);
        }
    }

    public bool checkBuff(string buffname)
    {
        return buffTags.Contains(buffname);
    }

    void ComsumeBuff()
    {
        for (int i = 0; i < buffTags.Count; i++)
        {
            buffTimes[i] -= Time.deltaTime;
            if (buffTimes[i] < 0)
            {
                buffTimes.RemoveAt(i);
                self_animatior.SetTrigger("un" + buffTags[i]);
                buffTags.RemoveAt(i);
            }
        }
    }
    bool checkGrouned()
    {
        if (center.transform.position.y < 2.5)
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            handleKeyInput();
            handleFingerInput();
            updatePosHorizon();
            KeepRunning();
            ComsumeBuff();
        }
        else
        {
            if (!trigger_die)
            {
                trigger_die = true;
                destorySelf();
            }
        }
    }

    public class FromTo
    {
        public int fingerId;
        public Vector2 first;
        public Vector2 second;
    }

    void CalcAndResponse(FromTo fromto)
    {
        Vector2 vec = fromto.second - fromto.first;
        float max_result = 0;
        // 距离过小则忽略
        if (Vector2.Distance(Vector2.zero, vec) < 10)
        {
            return;
        }
        int now_index = 0;
        max_result = Vector2.Dot(vec, Vector2.up);
        if (Vector2.Dot(vec, Vector2.right) > max_result)
        {
            now_index = 1;
            max_result = Vector2.Dot(vec, Vector2.right);
        }
        if (Vector2.Dot(vec, Vector2.down) > max_result)
        {
            now_index = 2;
            max_result = Vector2.Dot(vec, Vector2.down);
        }
        if (Vector2.Dot(vec, Vector2.left) > max_result)
        {
            now_index = 3;
        }
        switch (now_index)
        {
            case 0: moveUp(); break;
            case 1: moveRight(); break;
            case 2: moveDown(); break;
            case 3: moveLeft(); break;
        }
    }

    void handleFingerInput()
    {
        // 还是保持和音游判定相同了。
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                FromTo movement = new()
                {
                    fingerId = touch.fingerId,
                    first = touch.position
                };
                movementList.Add(movement);
            }
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {

                for (int k = 0; k < movementList.Count; k++)
                {
                    FromTo movement = movementList[k];
                    if (movement.fingerId == touch.fingerId)
                    {
                        movement.second = touch.position;
                        if (Vector2.Distance(movement.first, movement.second) > 25 || touch.phase == TouchPhase.Ended)
                        {
                            CalcAndResponse(movement);
                            movementList.RemoveAt(k);
                            k--;
                        }
                    }
                }
            }
        }
    }

    void handleKeyInput()
    {
        // if (Input.GetButtonDown("Horizontal"))
        // {
        //     if (Input.GetAxisRaw("Horizontal") < 0)
        //     {
        //         moveLeft();
        //     }
        //     else
        //     {
        //         moveRight();
        //     }
        // }
        // if (Input.GetButtonDown("Jump"))
        // {
        //     moveUp();
        // }
        // if (Input.GetButtonDown("Vertical"))
        // {
        //     if (Input.GetAxisRaw("Vertical") < 0)
        //     {
        //         moveDown();
        //     }
        //     else
        //     {
        //         moveUp();
        //     }
        // }
        KeyCode[] leftKeys = DataStorager.keysettings.left;
        foreach( KeyCode key in leftKeys ){
            if(Input.GetKeyDown(key)){
                 moveLeft();
            }
        }

        KeyCode[] rightKeys = DataStorager.keysettings.right;
        foreach( KeyCode key in rightKeys ){
            if(Input.GetKeyDown(key)){
                 moveRight();
            }
        }

        KeyCode[] upKeys = DataStorager.keysettings.up;
        foreach( KeyCode key in upKeys ){
            if(Input.GetKeyDown(key)){
                 moveUp();
            }
        }

        KeyCode[] downKeys = DataStorager.keysettings.down;
        foreach( KeyCode key in downKeys ){
            if(Input.GetKeyDown(key)){
                 moveDown();
            }
        }
    }

    float CalcOffsetByTimer()
    {
        float t = move_timer += Time.deltaTime;
        return (float)(delta_pos * (1 - Math.Pow(1 - t / CROSS_TIME, 4)));
    }

    void GenerateBoom(){
        var camera = GlobalTargetManager.camera;
        camera.GetComponent<FixedCamera>().triggerShake();
        var boom = GlobalTargetManager.boom;
        var newboom = Instantiate(boom);
        newboom.transform.position = center.transform.position;
        // newboom.Play();
    }

    void destorySelf()
    {
        GenerateBoom();
        velocity = Vector3.zero;
        // the_rigidbody.freezeRotation = true;
        // the_rigidbody.useGravity = false;
        realDunzi.GetComponent<Renderer>().enabled = false;
        BGM.GetComponent<AudioSource>().Stop();
    }

    public void KillSelf()
    {
        alive = false;
        DataStorager.SaveStatus();
    }
    public bool isAlive()
    {
        return alive;
    }
    void updatePosHorizon()
    {
        if (toMoving)
        {
            origin_pos = gameObject.transform.position.x;
            should_pos = TRACK_WIDTH * (now_track - (MAX_TRACKS + 1) / 2);
            delta_pos = should_pos - gameObject.transform.position.x;
            move_timer = 0f;
            isMoving = true;
        }
        if (isMoving)
        {
            gameObject.transform.position = new Vector3(origin_pos + CalcOffsetByTimer(), gameObject.transform.position.y, gameObject.transform.position.z);
            if (move_timer >= CROSS_TIME)
            {
                isMoving = false;
            }
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Obstacle"))
    //     {
    //         boom.transform.position = the_rigidbody.transform.position;
    //         boom.Play();
    //         the_rigidbody.AddForce(Vector3.up * 40f, ForceMode.Impulse);
    //     }
    // }
    // 移动
    void moveUp()
    {
        if (checkGrouned())
        {
            // the_rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            velocity += new Vector3(0,20,0);
            isFlying = true;
            // isGrounded = false;
        }
    }
    void moveLeft()
    {
        if (now_track > 1)
        {
            now_track -= 1;
            toMoving = true;
        }
    }
    void moveRight()
    {
        if (now_track < MAX_TRACKS)
        {
            now_track += 1;
            toMoving = true;
        }
    }
    void moveDown()
    {
        // the_rigidbody.AddForce(Vector3.down * 50f, ForceMode.Impulse);
        velocity -= new Vector3(0,(float)(gameObject.transform.position.y / 0.12),0);
        animator.SetTrigger("Flating");
    }
    float CalcDistance(Vector3 pos1,Vector3 pos2,Vector3 pos3){
        float a = (pos2 - pos3).magnitude;
        float b = (pos1 - pos3).magnitude;
        float c = (pos1 - pos2).magnitude;
        float p = ( a + b + c ) / 2;
        float area = (float)Math.Sqrt(p*(p-a)*(p-b)*(p-c));
        // Debug.Log(area * 2 / c);
        return area * 2 / c;
    }
    // 持续奔跑
    void KeepRunning()
    {
        all_timer += Time.deltaTime;
        // 着地
        if (center.transform.position.y < 1)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1 - (center.transform.position.y - gameObject.transform.position.y), gameObject.transform.position.z);
            velocity = Vector3.zero;
            isFlying = false;
        }
        if(isFlying){
            velocity = new Vector3(velocity.x, velocity.y - Gravity * Time.deltaTime, velocity.z);
        }
        velocity = new Vector3(velocity.x, velocity.y, MOVE_SPEED * speedTimes * (1 + all_timer / 100));
        // 到达一定速度后，第二种检测碰撞模式
        if(velocity.z > 150){
            Vector3 newPos = gameObject.transform.position + velocity * Time.deltaTime;
            var lists = GameObject.FindGameObjectsWithTag("Obstacle");
            for(int i = 0;i < lists.Length; i++){
                if(gameObject.transform.position.z < lists[i].transform.position.z && lists[i].transform.position.z < newPos.z){
                    if(CalcDistance(gameObject.transform.position,newPos,lists[i].transform.position) < 1.615){
                        lists[i].GetComponent<OnHit>().HandleTrigger(gameObject.GetComponent<Collider>());
                    }
                }
            }
            lists = GameObject.FindGameObjectsWithTag("Item");
            for(int i = 0;i < lists.Length; i++){
                if(gameObject.transform.position.z < lists[i].transform.position.z && lists[i].transform.position.z < newPos.z){
                    if(CalcDistance(gameObject.transform.position,newPos,lists[i].transform.position) < 1.615){
                        var obst = lists[i].GetComponent<CrashingDunzi>();
                        if(obst){
                            obst.HandleTrigger(gameObject.GetComponent<Collider>());
                        };
                        var obst_2 = lists[i].GetComponent<Invincible>();
                        if(obst_2){
                            obst_2.HandleTrigger(gameObject.GetComponent<Collider>());
                        };
                    }
                }
            }
            lists = GameObject.FindGameObjectsWithTag("Coin");
            for(int i = 0;i < lists.Length; i++){
                if(gameObject.transform.position.z < lists[i].transform.position.z && lists[i].transform.position.z < newPos.z){
                    if(CalcDistance(gameObject.transform.position,newPos,lists[i].transform.position) < 0.825){
                        lists[i].GetComponent<Coin>().HandleTrigger(gameObject.GetComponent<Collider>());
                    }
                }
            }
        }
        // 位移
        gameObject.transform.position += velocity * Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(all_timer * velocity.z * 32,0,0);
    }

    // 特殊效果
    public void SpeedUp(float times){
        speedTimes += times;
    }
}
