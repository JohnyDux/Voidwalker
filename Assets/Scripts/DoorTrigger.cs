using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // Customizable Parameters
    public float slideSpeed = 2f; // How fast the door slides
    public float openDistance = 2f; // How far the door slides open
    public bool isOpen = false; // Start state of the door (true = open, false = closed)
    public GameObject rightDoor;

    // Private Variables
    [SerializeField]private Vector3 closedPosition; // Initial position of the door
    [SerializeField] private Vector3 openPosition; // Position when the door is fully open

    void Start()
    {
        // Store the initial position as the closed position
        closedPosition = rightDoor.transform.position;

        // Calculate the open position based on the open distance
        openPosition = closedPosition + new Vector3(0, 0, -openDistance); // Assuming sliding along the X-axis

        // Set the initial position based on isOpen
        if (isOpen)
        {
            rightDoor.transform.position = openPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ToggleDoor();
    }
    private void OnTriggerExit(Collider other)
    {
        ToggleDoor();
    }

    // Method to toggle the door's state
    private void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen == true)
        {
            // Slide the door open
            StartCoroutine(SlideDoor(openPosition));
        }
        else if (isOpen == false)
        {
            // Slide the door closed
            StartCoroutine(SlideDoor(closedPosition));
        }
    }

    // Coroutine for smooth sliding
    private IEnumerator SlideDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(rightDoor.transform.position, targetPosition) > 0.01f)
        {
            // Move the door smoothly towards the target position
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Ensure the door is exactly at the target position
        rightDoor.transform.position = targetPosition;
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
