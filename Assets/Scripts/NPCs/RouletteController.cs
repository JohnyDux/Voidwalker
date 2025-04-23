using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RouletteController : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI nameTag;
    public TMP_Text dialogueTextBox;

    public bool playerInTrigger;

    string currentStateName;
    public TextMeshProUGUI stateNameTextBox;

    [Header("Roulette Variables")]
    public Animator dealerAnimator;
    public int winNumber;

    private void Awake()
    {
        playerInTrigger = false;

        dialogueBox.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("Player in Trigger");

            nameTag.text = gameObject.name;
            dialogueBox.SetActive(true);
            dialogueTextBox.text = "Press E to Play";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player pressed E");
            StartsPlaying();
        }

        //Print states
        if (dealerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            currentStateName = "Idle";
        }
        else if (dealerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Betting"))
        {
            currentStateName = "Betting";
        }
        else if (dealerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spinning"))
        {
            currentStateName = "Spinning";
        }
        else if (dealerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Determine Winning Number State"))
        {
            currentStateName = "Determine Winning Number State";
        }
        else if (dealerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Payout State"))
        {
            currentStateName = "Payout State";
        }
        else
        {
            currentStateName = "End Round State";
        }

        stateNameTextBox.text = "Current State: " + currentStateName;

        //UI for Player
            
        if(currentStateName == "Betting")
        {
            dialogueTextBox.text = "Press B to Bet";
        }
        else if (currentStateName == "Spinning")
        {
            dialogueTextBox.text = "The ball is spinning";
        }
        else if (currentStateName == "Determine Winning Number State")
        {
            dialogueTextBox.text = "Win Number:" + winNumber;
        }
        else if (currentStateName == "Payout State")
        {
            dialogueTextBox.text = "Paying the player";
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            dialogueBox.SetActive(false);
        }
    }

    void StartsPlaying()
    {
        if (playerInTrigger == true)
        {
            dealerAnimator.SetBool("startPlaying", true);
        }
    }
}