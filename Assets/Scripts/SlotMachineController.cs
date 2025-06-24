using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotMachineController : MonoBehaviour
{
    public Row[] rows; // Array of Row scripts
    public float spinDuration; // Duration of the spin
    public float spinDelay; // Delay between each row spin

    public TextMeshProUGUI prizeText;

    //LEVER
    public Animator lever;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            prizeText.text = "PRESS L TO PLAY";

            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("L pressed");
                StartSpin();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            prizeText.text = "SLOT MACHINE";

        }
    }
    public void StartSpin()
    {
        prizeText.text = "SPINNING";
        TiltLever();
        StartCoroutine(SpinRows());
    }

    private IEnumerator SpinRows()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].Spin(); // Call the Spin method on each row
            yield return new WaitForSeconds(spinDelay); // Wait before spinning the next row
        }

        yield return new WaitForSeconds(spinDuration);
        CheckForWin(); // Check for winning combinations after all rows have spun
    }

    private void CheckForWin()
    {
        // Implement your win-checking logic here
        int winIndex1 = rows[0].winSpriteIndex;
        int winIndex2 = rows[1].winSpriteIndex;
        int winIndex3 = rows[2].winSpriteIndex;

        if(winIndex1 == winIndex2 && winIndex1 == winIndex3)
        {
            //prize 300
            prizeText.text = "PRIZE \n" + "300 CREDITS";
        }
        else if(winIndex1 == winIndex2)
        {
            //prize 100
            prizeText.text = "PRIZE \n" + "100 CREDITS";
        }
        else if (winIndex1 == winIndex3)
        {
            //prize 100
            prizeText.text = "PRIZE \n" + "100 CREDITS";
        }
        else if(winIndex2 == winIndex3)
        {
            //prize 100
            prizeText.text = "PRIZE \n" + "100 CREDITS";
        }
        else
        {
            //no prize
            prizeText.text = "NO PRIZE";
        }
    }

    //LEVER
    public void TiltLever()
    {
        lever.SetBool("Tilt", true);

        float time = 1;
        while(time > 0)
            time -= Time.deltaTime;

        if(time <=0)
            lever.SetBool("Tilt", false);
    }
}