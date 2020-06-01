using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderUpdate : MonoBehaviour
{
    public Animator animator;  

    private void Start() {
        animator.SetLayerWeight (1, 1f);
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(1).IsName("animCowboyLowering") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.0f){
            Loader.LoadTargetScene();
        }
    }
}
