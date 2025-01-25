using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isFlickering", true);
    }

    // Este método se llama al salir del estado Flickering
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isFlickering", false);
    }
}
