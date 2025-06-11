using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityGuardController : MonoBehaviour
{
    [Header("Attributes")]
    public int health;

    [Header("AI Logic")]
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    public Transform[] waypoints; // Array of points to move to
    bool canMove;
    public float walkWaitTime = 2f; // Time to wait at each waypoint

    public int currentWaypointIndex; // Index of the current waypoint

    public Animator SecurityGuardAnimator;

    private void Start()
    {
        currentWaypointIndex = 0;
        canMove = true;
        StartCoroutine(MoveToWaypoints());
    }

    void Update()
    {
        if (waypoints.Length > 0)
        {
            StartCoroutine(MoveToWaypoints());
        }

        Die();
    }
   
    private IEnumerator MoveToWaypoints()
    {
        if (canMove)
        {
            while (true)
            {
                // Set the destination to the current waypoint
                agent.SetDestination(waypoints[currentWaypointIndex].position);

                // Wait until the agent reaches the destination
                if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    SecurityGuardAnimator.SetBool("Walking", true);
                    yield return null; // Wait for the next frame
                }
                else
                {
                    SecurityGuardAnimator.SetBool("Walking", false);

                    // Wait for the specified time at the waypoint
                    yield return new WaitForSeconds(walkWaitTime);

                    // Move to the next waypoint
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop back to the first waypoint
                }
            }
        }
    }

    void Die()
    {
        if(health <= 0)
        {
            agent.speed = 0;
            SecurityGuardAnimator.SetBool("Dead", true);
        }
    }
}
