using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    public float animSpeed;
    public static bool goDown = true;

    private const float PLAYER_Y = -4f;
    private const float MAP_RANGE = 3.5f;

    private int score;
    private float dificulty = .0f;
    private float maxDificulty = 1f;

    private Rigidbody2D playerBody;
    private Vector3 touchRealPosition;

    private State state;
    private enum State {
        Waiting,
        Playing,
        Dead
    }

    public event EventHandler OnStart, OnEnd;
    public float speed;

    private static Cowboy instance;

    public static Cowboy GetInstance() {
        return instance;
    }

    private void Awake() {
		instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("goDown", goDown);
        GetComponent<Animator>().speed = animSpeed;

        score = 0;
        state = State.Waiting;
        playerBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        FollowTouch();
    }

    private void FollowTouch() {
        switch(state) {
            case State.Playing:
                
                // Player folows the X position of the click at a certain speed
                Vector3 touchPos;
                if(Input.touchCount > 0) {
                    Touch touch = Input.GetTouch(0);
                    touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                }else{
                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                if(touchPos != null && touchPos.y < 7) {
                    Vector2 targetPos = new Vector2(touchPos.x, touchPos.y);

                    targetPos.x = Mathf.Clamp(targetPos.x, -MAP_RANGE, MAP_RANGE);
                    targetPos.y = PLAYER_Y;
                    
                    transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * speed);
                }

                break;
            case State.Waiting:

                if(Input.GetMouseButtonDown(0) || Input.touchCount > 0) {
                    state = State.Playing;
                    if(OnStart != null) OnStart(this, EventArgs.Empty);
                }
                
                break;
            default:
                break;
        }
    }

    public int GetScore() {
        return score;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Coin") {
            score++;

            Level level = Level.GetInstance();

            if(score%5==0 && dificulty < maxDificulty){
                dificulty += .1f;
                GameManager.GetInstance().ySpeed = GameManager.GetInstance().initialYSpeed + (GameManager.GetInstance().initialYSpeed * dificulty);

                level.spawnTime = level.initialSpawnTime - ((level.initialSpawnTime * dificulty) * .75f);
            }

            level.RemoveObj(col.GetComponent<Transform>());
            SoundManager.GetInstance().PlaySoundEffect(SoundManager.SoundEffect.Coin);
        } else if(col.tag == "Fence") {
            state = State.Dead;

            col.transform.GetComponent<Animator>().enabled = true;

            if(OnEnd != null) OnEnd(this, EventArgs.Empty);
        }
    }
}
