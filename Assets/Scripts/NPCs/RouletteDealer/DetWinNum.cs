using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetWinNum : StateMachineBehaviour
{
    int winNumber;
    float timeToCalculate = 5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        winNumber = Random.Range(0, 36);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeToCalculate = -Time.deltaTime;
        if (timeToCalculate <= 0)
        {
            Debug.Log("Win Number: " + winNumber);
            animator.SetBool("winBetsDetermined", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
