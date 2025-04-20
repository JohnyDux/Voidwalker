using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Payout : StateMachineBehaviour
{
    Transform rouletteDealer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rouletteDealer = GameObject.FindGameObjectWithTag("RouletteDealer").transform;
        Pay(animator);
    }

    void Pay(Animator animator)
    {
        Debug.Log("Pay Player");
        animator.SetBool("payoutsCompleted", true);
    }
}
