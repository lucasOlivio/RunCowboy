using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    public float animSpeed;

    private const float PLAYER_Y = -4f;
    private const float MAP_RANGE = 3.5f;

    private int score;
    private float dificulty = .0f;
    private float maxDificulty = 0.8f;

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
        GetComponent<Animator>().speed = animSpeed;

        score = 0;
        state = State.Waiting;
        playerBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if(Input.touchCount > 0) {
            switch(state) {
                case State.Playing:
                    FollowTouch();
                    break;
                case State.Waiting:
                    state = State.Playing;
                    if(OnStart != null) OnStart(this, EventArgs.Empty);
                    break;
                default:
                    break;
            }
        }

        // FOR TESTING ON PC ONLY! REMOVE WHEN BUILD TO PRODUCTION!
        if(Input.GetMouseButtonDown(0) && state == State.Waiting) {
            state = State.Playing;
            if(OnStart != null) OnStart(this, EventArgs.Empty);
        }

        if(state == State.Playing)
            FollowMouse();
    }

    private void FollowMouse() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetPos = new Vector2(mousePos.x, mousePos.y);

        targetPos.x = Mathf.Clamp(targetPos.x, -MAP_RANGE, MAP_RANGE);
        targetPos.y = PLAYER_Y;

        transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * speed);
    }

    private void FollowTouch() {
        // Player folows the X position of the click at a certain speed
        Touch touch = Input.GetTouch(0);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 targetPos = new Vector2(mousePos.x, mousePos.y);

        targetPos.x = Mathf.Clamp(targetPos.x, -MAP_RANGE, MAP_RANGE);
        targetPos.y = PLAYER_Y;
        
        transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * speed);
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
