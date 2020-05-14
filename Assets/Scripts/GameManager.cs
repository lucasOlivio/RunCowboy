using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float slowness = 10f;
    public Text startText;

    public void Start() {
        Cowboy.GetInstance().OnEnd += OnEnd;
        Cowboy.GetInstance().OnStart += OnStart;
    }

    public void OnStart(object sender, System.EventArgs e) {
        startText.GetComponent<Text>().enabled = false;
    }


    public void OnEnd(object sender, System.EventArgs e) {
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
