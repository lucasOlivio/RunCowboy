using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    private const float PLAYER_Y = -2f;
    private const float MAP_RANGE = 3.5f;

    private int score;

    private Rigidbody2D playerBody;
    private Vector3 touchRealPosition;

    private State state;
    private enum State {
        Waiting,
        Playing
    }

    public event EventHandler OnStart;
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
            FollorMouse();
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

    private void FollorMouse() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            Level.GetInstance().RemoveObj(col.GetComponent<Transform>());
            SoundManager.PlaySound(SoundManager.Sound.Coin);
        }
    }
}
