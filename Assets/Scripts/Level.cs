﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float BORDER_HEIGHT = 26.2f;
    private const float BORDER_Y_DESTROY = -8f;
    private const float OBJS_Y_DESTROY = -9f;

    public float initialSpawnTime;
    public float spawnTime;

    public List<Transform> objsList;
    public List<Transform> spawnList;

    private float nextSpawn = 0f;
    [SerializeField] 
    private List<int> possibleHoles;
    [SerializeField] 
    private List<int> allowedHoles;

    // Ground movement variables
    private Vector2 offset;
    private Transform pfFence;
    private Transform pfCoin;

    // General Assets
    private GameAssets assets;

    private State state;
    private enum State {
        Waiting,
        Playing
    }

    private static Level instance;

    public static Level GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        spawnTime = initialSpawnTime;
        GameManager.GetInstance().ySpeed = GameManager.GetInstance().initialYSpeed;
        state = State.Waiting;
        assets = GameAssets.GetInstance();

        pfFence = assets.pfFence;
        pfCoin = assets.pfCoin;

        Cowboy.GetInstance().OnStart += OnStart;
        Cowboy.GetInstance().OnEnd += OnEnd;

        possibleHoles = new List<int>();
        allowedHoles = new List<int>();
        for(int i = 0; i < spawnList.Count; i++){
            possibleHoles.Add(i);
        }
        allowedHoles.AddRange(possibleHoles);
    }

    private void Update() {
        if(state == State.Playing){
            SpawnObjs(2);
            ObjsMove();
        }
    }

    private void OnStart(object sender, System.EventArgs e) {
        state = State.Playing;
    }

    private void OnEnd(object sender, System.EventArgs e) {
        SoundManager.GetInstance().StopSoundBackground(SoundManager.SoundBackground.BackgroundMusic);
    }

    /*
     * BARRIERS
     */

    private void SpawnObjs(int holesNumber) {
        /* Creates a line of objs every x time
         */

        nextSpawn -= Time.fixedDeltaTime;

        
        if(nextSpawn <= 0){
            float objsX, objsY;
            Transform obj;
            List<int> holesIndex = new List<int>();

            for(int i = 0; i < holesNumber; i++){
                int newHole = allowedHoles[Random.Range(0, allowedHoles.Count)];

                holesIndex.Add(newHole);
                allowedHoles.Remove(newHole);

                if(allowedHoles.Count == 0) {
                    allowedHoles.AddRange(possibleHoles);
                }
            }

            for(int i = 0; i<spawnList.Count; i++){
                objsX = spawnList[i].position.x;
                objsY = spawnList[i].position.y;

                if(holesIndex.Contains(i)){
                    obj = Instantiate(pfCoin, new Vector3(objsX, objsY, 0), Quaternion.identity);
                } else {
                    obj = Instantiate(pfFence, new Vector3(objsX, objsY, 0), Quaternion.identity);
                }

                objsList.Add(obj);
            }
            nextSpawn = spawnTime;
        }
    }

    public void RemoveObj(Transform obj) {
        Destroy(obj.gameObject);
        objsList.Remove(obj);
    }

    private void ObjsMove() {
        /* Moves the objs downwards at a fixed speed, when the objs passes a certain high it is destroyed
         */

        for (int i = 0; i < objsList.Count; i++)
        {
            Transform objs = objsList[i];

            objs.position += new Vector3(0, -1, 0) * GameManager.GetInstance().ySpeed * Time.fixedDeltaTime;

            if(objs.position.y < OBJS_Y_DESTROY) {
                Destroy(objs.gameObject);
                objsList.RemoveAt(i);
                i--;
            }
        }
    }
}
