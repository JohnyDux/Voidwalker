using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    // Customizable Parameters
    public float slideSpeed = 2f; // How fast the door slides
    public float openDistance = 2f; // How far the door slides open
    public bool isOpen = false; // Start state of the door (true = open, false = closed)
    public GameObject platform;

    // Private Variables
    [SerializeField] private Vector3 closedPosition; // Initial position of the door
    [SerializeField] private Vector3 openPosition; // Position when the door is fully open

    private void Start()
    {
        // Set the initial position based on isOpen
        if (isOpen)
        {
            platform.transform.position = openPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ToggleLift();
    }
    private void OnTriggerExit(Collider other)
    {
        ToggleLift();
    }

    // Method to toggle the door's state
    private void ToggleLift()
    {
        isOpen = !isOpen;

        if (isOpen == true)
        {
            // Slide the door open
            StartCoroutine(SlidePlatform(openPosition));
        }
        else if (isOpen == false)
        {
            // Slide the door closed
            StartCoroutine(SlidePlatform(closedPosition));
        }
    }

    // Coroutine for smooth sliding
    private IEnumerator SlidePlatform(Vector3 targetPosition)
    {
        while (Vector3.Distance(platform.transform.position, targetPosition) > 0.01f)
        {
            // Move the door smoothly towards the target position
            platform.transform.position = Vector3.Lerp(platform.transform.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Ensure the door is exactly at the target position
        platform.transform.position = targetPosition;
    }

    void OnDrawGizmos()
    {
        // Draw a red sphere for the closed position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(closedPosition, 1f);

        // Draw a green sphere for the open position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(openPosition, 1f);
    }

}
