using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Payout : StateMachineBehaviour
{
    public PlayerStats player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Pay(animator);
    }

    void Pay(Animator animator)
    {
        Debug.Log("Pay Player");
        player.moneyBalance = player.moneyBalance + 10;
        animator.SetBool("payoutsCompleted", true);
    }
}
