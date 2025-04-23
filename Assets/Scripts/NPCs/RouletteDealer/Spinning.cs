using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spinning : StateMachineBehaviour
{
    bool isSpinning;
    public float timeToSpin = 3f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isSpinning = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeToSpin -= Time.deltaTime;
        if (timeToSpin <= 0)
        {
            isSpinning = false;
            animator.SetBool("ballSettled", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
