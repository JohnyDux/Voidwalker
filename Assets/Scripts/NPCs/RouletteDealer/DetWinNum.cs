using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetWinNum : StateMachineBehaviour
{
    public int winNum;
    public TMP_Text dialogueTextBox;
    float timeToCalculate = 5f;

    RouletteController roulette;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        roulette = FindObjectOfType<RouletteController>();

        winNum = Random.Range(0, 36);
        roulette.winNumber = winNum;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeToCalculate = -Time.deltaTime;
        if (timeToCalculate <= 0)
        {
            animator.SetBool("winBetsDetermined", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
