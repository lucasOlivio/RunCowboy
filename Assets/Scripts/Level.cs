using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float BORDER_HEIGHT = 26.2f;
    private const float BORDER_Y_DESTROY = -8f;
    private const float BARRIER_Y_DESTROY = -9f;

    public float ySpeed;
    public float spawnTime;

    public List<Transform> barrierList;
    public List<Transform> spawnList;

    private float nextSpawn = 0f;
    private List<int> lastHoles;

    // Ground movement variables
    private Vector2 offset;
    private Material groundMaterial;
    private Transform pfFence;

    // General Assets
    private GameAssets assets;

    private State state;
    private enum State {
        Waiting,
        Playing
    }

    private void Start() {
        state = State.Waiting;
        assets = GameAssets.GetInstance();

        groundMaterial = assets.ground.GetComponent<Renderer>().material;
        pfFence = assets.pfFence;

        lastHoles = new List<int> {spawnList.Count};

        Cowboy.GetInstance().OnStart += OnStart;
    }

    private void Update() {
        if(state == State.Playing){
            MoveGround();
            borderMove(assets.leftBorderList);
            borderMove(assets.rightBorderList);
            spawnBarriers(2);
            barrierMove();
        }
    }

    private void OnStart(object sender, System.EventArgs e) {
        state = State.Playing;
    }

    /*
     * WALLS
     */

    private void MoveGround() {
        /*
         * Gives the ground a moving animation by changing the offset of the material
         */
        offset = new Vector2(0, ySpeed);
        groundMaterial.mainTextureOffset += offset * Time.fixedDeltaTime;
    }

    private void borderMove(List<Transform> borderList) {
        /* 
         * Moves the borders downwards at a fixed speed, when the border passes a certain high it is moved over the top border
         */
        
        float yTopBorder = -100f;

        foreach (Transform border in borderList)
        {
            border.position += new Vector3(0, -1, 0) * ySpeed * Time.fixedDeltaTime;

            if(border.position.y < BORDER_Y_DESTROY) {
                for (int i = 0; i < borderList.Count; i++)
                {
                    if(borderList[i].position.y > yTopBorder){
                        yTopBorder = borderList[i].position.y;
                    }   
                }
                
                border.position = new Vector3(border.position.x, yTopBorder + (BORDER_HEIGHT * .64f), border.position.z);
            }
        }
    }

    public static int randomExcept(int max, List<int> exclude) 
    {
        System.Random r = new System.Random();
        int result = r.Next(max - exclude.Count);

        for (int i = 0; i < exclude.Count; i++) 
        {
            if (result < exclude[i])
                return result;
            result++;
        }
        return result;
    }

    /*
     * BARRIERS
     */

    private void spawnBarriers(int holesNumber) {
        /* Creates a line of barriers every x time
         */

        nextSpawn -= Time.fixedDeltaTime;

        
        if(nextSpawn <= 0){
            float barrierX, barrierY;
            Transform barrier;
            List<int> holesIndex = new List<int>();

            for(int i = 0; i < holesNumber; i++){
                int newHole = randomExcept(spawnList.Count, lastHoles);
                holesIndex.Add(newHole);
                lastHoles.Add(newHole);
                lastHoles.Sort();
            }

            lastHoles = holesIndex;
            lastHoles.Sort();

            for(int i = 0; i<spawnList.Count; i++){
                if(holesIndex.Contains(i))
                    continue;

                barrierX = spawnList[i].position.x;
                barrierY = spawnList[i].position.y;
                barrier = Instantiate(pfFence, new Vector3(barrierX, barrierY, 0), Quaternion.identity);
                barrierList.Add(barrier);
            }
            nextSpawn = spawnTime;
        }
    }

    private void barrierMove() {
        /* Moves the barrier downwards at a fixed speed, when the barrier passes a certain high it is destroyed
         */

        for (int i = 0; i < barrierList.Count; i++)
        {
            Transform barrier = barrierList[i];

            barrier.position += new Vector3(0, -1, 0) * ySpeed * Time.fixedDeltaTime;

            if(barrier.position.y < BARRIER_Y_DESTROY) {
                Destroy(barrier.gameObject);
                barrierList.RemoveAt(i);
                i--;
            }
        }
    } 
}
