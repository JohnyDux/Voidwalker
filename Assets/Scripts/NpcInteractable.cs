using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcInteractable : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI dialogueTextBox;

    [TextArea(3, 10)]
    public string dialogueLine;

    private void Start()
    {
        dialogueBox.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nameTag.text = gameObject.name;
            dialogueTextBox.text = dialogueLine;
            dialogueBox.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(false);
        }
    }
}
