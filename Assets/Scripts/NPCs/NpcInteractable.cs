using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcInteractable : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI dialogueTextBox;

    public bool playerInTrigger;

    [TextArea(3, 10)]
    public string dialogueLine;

    public string currentStateName;

    [Header("Roulette Variables")]
    public Animator dealerAnimator;
    bool startPlaying;
    bool placingBets;
    public int winNumber;

    private void Awake()
    {
        playerInTrigger = false;
    }

    private void Start()
    {
        dialogueBox.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("Player in Trigger");

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnPlayingRoulette();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            Debug.Log("Player out of Trigger");
        }
    }

    void OnPlayingRoulette()
    {
        if(playerInTrigger == true)
        {
            nameTag.text = gameObject.name;
            dialogueTextBox.text = dialogueLine;
            dialogueBox.SetActive(true);

            Debug.Log("Key Pressed. Start Playing");
            //Start playing
            startPlaying = true;

            //show indicator to place bets
            if (placingBets == true)
            {
                dealerAnimator.SetBool("betsPlaced", true);
                Debug.Log("Bets Placed");
            }
        }
    }
}
