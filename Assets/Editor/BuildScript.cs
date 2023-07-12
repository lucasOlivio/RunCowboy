using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript 
{
    static void PerformBuild()
    {
        string[] defaultScene = { 
            "Assets/Scenes/MenuScene.unity",
            "Assets/Scenes/LoadingScene.unity",
            "Assets/Scenes/GameScene.unity",
            };

        BuildPipeline.BuildPlayer(defaultScene, "runcowboy.apk" ,
            BuildTarget.Android, BuildOptions.None);
    }

}