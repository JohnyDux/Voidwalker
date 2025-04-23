using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Betting : StateMachineBehaviour
{//este aqui tá mal algures e tem a ver com a cena dos nomes nos diálogos
    bool hasBet;
    public float betValue;
    public PlayerStats player;

    public GameObject dialogueBox;
    public TextMeshProUGUI nameTag;
    public TMP_Text dialogueTextBox;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasBet = false;
        nameTag.text = animator.GetComponentInParent<GameObject>().name;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Player pressed B");
            if(player.moneyBalance > 0)
            {
                hasBet = true;
                betValue = 10;
                player.moneyBalance = player.moneyBalance - betValue;
            }
            else
            {
                dialogueBox.SetActive(true);
                dialogueTextBox.text = "Not enough funds to play";
            }
        }

        animator.SetBool("betsPlaced", hasBet);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
