using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static InputStruct;

public class Player : MonoBehaviour
{
    public static Player targetPlayer;

    public GameObject center;
    private Vector3 velocity;
    public int now_track = 2;
    public BeatmapManager beatmapManager;
    public const int MAX_TRACKS = 3;
    private float all_timer = 0;
    private float loosen_time = 0;
    private bool toMoving = false;
    private bool isMoving = false;
    private bool isDrop = false;
    public float gravity = 50f;
    public const int TRACK_WIDTH = 3;
    private float origin_pos = 0;
    private float should_pos = 0;
    private float delta_pos = 0;
    private float move_timer = 0f; // 计时器
    public const float MAX_CROSS_TIME = 0.2125f;
    public float cross_time = MAX_CROSS_TIME;
    private bool isFlying = false;
    private List<FromTo> movementList = new();

    public List<InputImpluse> inputImpluses = new();

    void CreateNewInputImpluse(int num) {
        inputImpluses.Add(
            new InputImpluse(){
                track = num,
                time = Time.fixedTime
            }
        );
    }

    void inputUpdate() {
        while(inputImpluses.Count > 0){
            if(Time.fixedTime - inputImpluses[0].time <= 0.1){
                break;
            }
            inputImpluses.RemoveAt(0);
        }
    }

    void Awake()
    {
        float speed = DataStorager.settings.MusicGameSpeed > 0 ? DataStorager.settings.MusicGameSpeed : 1;
        velocity.z = 50 * speed;
        targetPlayer = this;
    }

    IEnumerator FixPos(){
        while(true){
            ChangePos();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ChangePos()
    {
        Vector3 pos = transform.position;
        pos.z = beatmapManager.GetPlayingTime() * velocity.z;
        transform.position = pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FixPos());
        updateGravity();
    }

    void updateGravity(){
        gravity = (float)(2 * 8 / Math.Pow(60 / beatmapManager.getBPM(),2));
    }

    public float GetGravity(){
        return gravity;
    }

    // Update is called once per frame
    void FixedUpdate(){
        all_timer += Time.fixedDeltaTime;

        inputUpdate();
        updatePosHorizon();

        // 着地
        if (transform.position.y < 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0 , gameObject.transform.position.z);
            velocity.y = 0;
            isFlying = false;
            isDrop = false;
        }
        if(isFlying){
            velocity = new Vector3(velocity.x, velocity.y - gravity * Time.fixedDeltaTime, velocity.z);
        }

        gameObject.transform.position += velocity * Time.fixedDeltaTime;
        center.transform.rotation = Quaternion.Euler(all_timer * velocity.z * 32,0,0);
    }

    void Update()
    {
        loosen_time += Time.deltaTime;
        handleKeyInput();
        handleFingerInput();
        if(DataStorager.settings.relaxMod){
            handleNumInput();
        }
        updateGravity();

        if(DateTime.Now.Day == 1 && DateTime.Now.Month == 4){
            transform.position = new Vector3(math.cos(Time.time * 40) * 3,1 + math.sin(Time.time * 40),transform.position.z);
        }
    }

    public float GetVelocity()
    {
        return velocity.z;
    }

    public bool isDroping()
    {
        return isDrop;
    }

    public Vector3 GetPos()
    {
        return gameObject.transform.position;
    }


    float CalcOffsetByTimer()
    {
        float t = move_timer += Time.fixedDeltaTime;
        return (float)(delta_pos * (1 - Math.Pow(1 - t / cross_time, 8)));
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
            if (move_timer >= cross_time)
            {
                isMoving = false;
            }
        }
    }

    bool checkGrouned()
    {
        if (transform.position.y < 1)
        {
            isDrop = false;
            return true;
        }
        return false;
    }
    public void moveUp()
    {
        if (checkGrouned() || loosen_time < 0.1)
        {
            // the_rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            float new_speed = gravity * 60 / beatmapManager.getBPM();
            velocity += new Vector3(0,new_speed,0);
            if(!isFlying){
                loosen_time = 0;
            }
            isFlying = true;
            // isGrounded = false;
        }
    }
    public void moveLeft()
    {
        if (now_track > 1)
        {
            now_track -= 1;
            CreateNewInputImpluse(now_track);
            toMoving = true;
        }
    }
    public void moveRight()
    {
        if (now_track < MAX_TRACKS)
        {
            now_track += 1;
            CreateNewInputImpluse(now_track);
            toMoving = true;
        }
    }
    public void moveDown()
    {
        // the_rigidbody.AddForce(Vector3.down * 50f, ForceMode.Impulse);
        isDrop = true;
        checkGrouned();
        velocity -= new Vector3(0,(float)(gameObject.transform.position.y / 0.025),0);
        CreateNewInputImpluse(now_track);
        gameObject.GetComponent<Animator>().SetTrigger("DownFlat");
    }

    public void setCrossTime(float crotime){
        if(crotime > 0){
            if(crotime > 0.01f){
                cross_time = Math.Min(crotime, MAX_CROSS_TIME);
            } else {
                cross_time = 0.01f;
            }
        } else {
            cross_time = 0.01f;
        }
    }

        public class FromTo
    {
        // Fields
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

                for(int k = 0; k < movementList.Count; k++){
                    FromTo movement = movementList[k];
                    if(movement.fingerId == touch.fingerId){
                        movement.second = touch.position;
                        if(Vector2.Distance(movement.first,movement.second) > 25 || touch.phase == TouchPhase.Ended){
                            CalcAndResponse(movement);
                            movementList.RemoveAt(k);
                            k--;
                        }
                    }
                }
            }
        }
    }

    void handleNumInput(){
        KeyCode[] firstKeys = DataStorager.keysettings.pad1;
        foreach( KeyCode key in firstKeys ){
            if(Input.GetKeyDown(key)){
                 moveToIndex(1);
            }
        }

        KeyCode[] secondKeys = DataStorager.keysettings.pad2;
        foreach( KeyCode key in secondKeys ){
            if(Input.GetKeyDown(key)){
                 moveToIndex(2);
            }
        }

        KeyCode[] thirdKeys = DataStorager.keysettings.pad3;
        foreach( KeyCode key in thirdKeys ){
            if(Input.GetKeyDown(key)){
                 moveToIndex(3);
            }
        }
    }

    void moveToIndex(int index){
        now_track = index;
        CreateNewInputImpluse(now_track);
        toMoving = true;
    }

    void handleKeyInput()
    {
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

    public int GetNowTrack(){
        return now_track;
    }
}
