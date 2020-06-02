using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene {
        GameScene,
        LoadingScene,
        MenuScene
    }

    private static Scene targetScene;

    public static void animGoDown(bool goDown) {
        Cowboy.goDown = goDown;
        ButtonAnim.goDown = goDown;
    }

    public static void Load(Scene scene) {
        SceneManager.LoadScene(Scene.LoadingScene.ToString());

        targetScene = scene;
    }

    public static void LoadTargetScene() {
        SceneManager.LoadScene(targetScene.ToString());
    }
}