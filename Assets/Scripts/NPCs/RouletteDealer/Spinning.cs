using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spinning : StateMachineBehaviour
{
    bool isSpinning;
    public float timeToSpin;

    public GameObject ballPrefab;
    public Transform ballSpawnPos;
    GameObject ball;
    public RouletteBallController ballController;
    public float ballRollForce;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ball = Instantiate(ballPrefab, ballSpawnPos.position, ballSpawnPos.rotation);
        Debug.Log(ballSpawnPos.position);
        ballController = ball.GetComponent<RouletteBallController>();
        isSpinning = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ballController.RollBall(ballRollForce);

        timeToSpin -= Time.deltaTime;
        if (timeToSpin <= 0)
        {
            ballController.StopBall();
            isSpinning = false;
            animator.SetBool("ballSettled", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
