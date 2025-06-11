using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChefController : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    public Transform[] waypoints; // Array of points to move to
    bool canMove;
    public float walkWaitTime = 2f; // Time to wait at each waypoint

    public int currentWaypointIndex; // Index of the current waypoint

    float Timer;
    public float steerPotTime = 10f;
    public float plattingTime = 10f;

    public Animator chefAnimator;

    private void Start()
    {
        currentWaypointIndex = 0;
        StartCoroutine(MoveToWaypoints());
    }

    void Update()
    {
        if(currentWaypointIndex == 0)
        {
            StartCoroutine(SteerPot());
        }
        else if(currentWaypointIndex == 1)
        {
            StartCoroutine(Platting());
        }

        if (waypoints.Length > 0)
        {
            StartCoroutine(MoveToWaypoints());
        }
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
                    chefAnimator.SetBool("Walking", true);
                    yield return null; // Wait for the next frame
                }
                else
                {
                    chefAnimator.SetBool("Walking", false);

                    // Wait for the specified time at the waypoint
                    yield return new WaitForSeconds(walkWaitTime);

                    // Move to the next waypoint
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop back to the first waypoint
                }
            }
        }
    }

    private IEnumerator SteerPot()
    {
        canMove = false;
        chefAnimator.SetBool("OrderPlaced", true);

        // Wait for the specified time at the waypoint
        Timer = Timer - steerPotTime;

        chefAnimator.SetBool("OrderPlaced", false);
        canMove = true;

        yield return null;
    }

    private IEnumerator Platting()
    {
        canMove = false;
        chefAnimator.SetBool("DishCooked", true);

        // Wait for the specified time at the waypoint
        Timer = Timer - plattingTime;

        chefAnimator.SetBool("DishCooked", false);
        canMove = true;

        yield return null;
    }
}
