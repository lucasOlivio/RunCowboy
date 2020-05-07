using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    public Transform ground;

    public List<Transform> rightBorderList;
    public List<Transform> leftBorderList;
}
