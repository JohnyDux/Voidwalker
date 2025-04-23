using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Payout : StateMachineBehaviour
{
    public PlayerStats player;
    public float Prize;
    GameObject ballPrefab;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = player.GetComponent<PlayerStats>();
        Pay(player, animator);
        ballPrefab = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ballPrefab);
    }

    void Pay(PlayerStats player, Animator animator)
    {
        Debug.Log("Pay Player");
        player.moneyBalance = player.moneyBalance + Prize;
        animator.SetBool("payoutsCompleted", true);
    }
}
