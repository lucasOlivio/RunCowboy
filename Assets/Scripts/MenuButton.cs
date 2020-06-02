using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MenuButton : MonoBehaviour
{
    private void Awake() {
        GetComponent<Button_UI>().ClickFunc = () => {
            GameManager.GetInstance().NormalTime();
            Loader.animGoDown(false);
            Loader.Load(Loader.Scene.MenuScene);
        };
    }
}
