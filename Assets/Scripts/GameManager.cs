using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float slowness = 10f;
    public Text startText;

    public float initialYSpeed;
    public float ySpeed;
    private Vector2 offset;

    private static GameManager instance;

    public static GameManager GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    public void Start() {

        string sceneName = SceneManager.GetActiveScene().name;
        
        if(sceneName == "MenuScene") {
            SoundManager.GetInstance().PlaySoundBackground(SoundManager.SoundBackground.MenuBackgroundMusic);
        } else if(sceneName == "GameScene") {
            SoundManager.GetInstance().PlaySoundBackground(SoundManager.SoundBackground.BackgroundMusic);
            Cowboy.GetInstance().OnEnd += OnEnd;
            Cowboy.GetInstance().OnStart += OnStart;
        } else if(sceneName == "LoadingScene") {
            Cowboy.GetInstance().GetComponent<Animation>().Play();
        }
    }

    public void Update() {
        MoveGround();
        MoveBorder();
    }

    private void MoveGround() {
        /*
         * Gives the ground a moving animation by changing the offset of the material
         */
        offset = new Vector2(0, ySpeed);
        GameAssets.GetInstance().ground.GetComponent<Renderer>().material.mainTextureOffset += offset * Time.fixedDeltaTime;
    }

    private void MoveBorder() {
        /*
         * Gives the border a moving animation by changing the offset of the material
         */
        offset = new Vector2(0, ySpeed/8);
        GameAssets.GetInstance().borderLeft.GetComponent<Renderer>().material.mainTextureOffset += offset * Time.fixedDeltaTime;
        GameAssets.GetInstance().borderRight.GetComponent<Renderer>().material.mainTextureOffset += offset * Time.fixedDeltaTime;
    }

    public void OnStart(object sender, System.EventArgs e) {
        SoundManager.GetInstance().PlaySoundBackground(SoundManager.SoundBackground.HorseRunning);
        startText.GetComponent<Text>().enabled = false;
    }


    public void OnEnd(object sender, System.EventArgs e) {
        SoundManager.GetInstance().StopSoundBackground(SoundManager.SoundBackground.HorseRunning);
        SoundManager.GetInstance().PlaySoundEffect(SoundManager.SoundEffect.WoodCrack);
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel() {
        Time.timeScale = 1f / slowness;
        Time.fixedDeltaTime =  Time.fixedDeltaTime / slowness;

        yield return new WaitForSeconds(1.7f / slowness);

        Time.timeScale = 1f;
        Time.fixedDeltaTime =  Time.fixedDeltaTime * slowness;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
