using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour
{
    private void Awake() {
        transform.Find("PlayButton").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.GameScene);
        };

        transform.Find("QuitButton").GetComponent<Button_UI>().ClickFunc = () => {
            Application.Quit();
        };
    }
}
