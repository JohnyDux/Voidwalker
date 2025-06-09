using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChefController : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    public Transform[] waypoints; // Array of points to move to
    public float waitTime = 2f; // Time to wait at each waypoint

    private int currentWaypointIndex = 0; // Index of the current waypoint

    public Animator chefAnimator;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            StartCoroutine(MoveToWaypoints());
        }
    }
   
    private IEnumerator MoveToWaypoints()
    {
        while (true)
        {
            // Set the destination to the current waypoint
            agent.SetDestination(waypoints[currentWaypointIndex].position);

            // Wait until the agent reaches the destination
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                chefAnimator.SetBool("Walking", true);
                yield return null; // Wait for the next frame
            }

            chefAnimator.SetBool("Walking", false);

            // Wait for the specified time at the waypoint
            yield return new WaitForSeconds(waitTime);

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop back to the first waypoint
        }
    }
}
