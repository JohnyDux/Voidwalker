using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RouletteDealerController : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI nameTag;
    public TMP_Text dialogueTextBox;

    public TextMeshProUGUI stateTag;

    public PlayerStats player;
    public bool playerInTrigger;

    [Header("Roulette Variables")]
    public Animator dealerAnimator;
    public float winNumber;
    float finalRotation;

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

    void Betting(bool hasBet, float betMoney, int betNumber, PlayerStats player)
    {
        stateTag = GameObject.FindGameObjectWithTag("RouletteStateTag").GetComponent<TextMeshProUGUI>();
        stateTag.text = "Current State: Betting";

        hasBet = false;

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Player pressed B");
            if (player.moneyBalance > 0)
            {
                hasBet = true;
                betMoney = 10;
                player.moneyBalance = player.moneyBalance - betMoney;
            }
            else
            {
                dialogueBox.SetActive(true);
                dialogueTextBox.text = "Not enough funds to play";
            }
        }

        dealerAnimator.SetBool("betsPlaced", hasBet);
    }

    void Spinning(float numberOfRevolutions, float duration, GameObject rouletteBall)
    {
        stateTag = GameObject.FindGameObjectWithTag("RouletteStateTag").GetComponent<TextMeshProUGUI>();
        stateTag.text = "Current State: Spinning";

        // Find the GameObject with the tag "RouletteBall"
        rouletteBall = GameObject.FindGameObjectWithTag("RouletteBall");
        float elapsedTime = 0f; // Reset elapsed time

        if (rouletteBall != null)
        {
            // Calculate the rotation step based on elapsed time
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // Normalize time
            float currentRotation = Mathf.Lerp(0, numberOfRevolutions * 360, t); // Interpolate rotation

            // Rotate the RouletteBall around the Z axis
            rouletteBall.transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            // Optionally, you can log the current rotation for debugging
            Debug.Log("Current Rotation: " + currentRotation + " degrees");

            // If the rotation is complete, you can exit the state or reset
            if (t >= 1f)
            {
                // Optionally reset or transition to another state
                dealerAnimator.SetBool("ballSettled", true); // Example of transitioning
            }
        }
        else
        {
            Debug.LogWarning("No GameObject found with the tag 'RouletteBall'.");
        }

        finalRotation = rouletteBall.transform.eulerAngles.z;
    }

    void DetWinNum(float timeToCalculate)
    {
        float finalBallRotation;

        finalBallRotation = finalRotation;

        winNumber = finalBallRotation / 36;

        dealerAnimator.SetBool("winBetsDetermined", true);
    }

    void Payout(float Prize, GameObject ballPrefab)
    {
        player.moneyBalance = player.moneyBalance + Prize;
        dealerAnimator.SetBool("payoutsCompleted", true);
    }
}
