﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderUpdate : MonoBehaviour
{
    public Animator animator;  

    private void Start() {

    }

    private void Update()
    {
        if((animator.GetCurrentAnimatorStateInfo(1).IsName("animCowboyDown") || animator.GetCurrentAnimatorStateInfo(1).IsName("animCowboyUp")) && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.0f){
            Loader.LoadTargetScene();
        }
    }
}
