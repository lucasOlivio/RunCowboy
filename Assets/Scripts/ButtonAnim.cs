using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    public static bool goDown = true;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("goDown", goDown);
    }
}
