using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityGuardController : MonoBehaviour
{
    [Header("Attributes")]
    public int health;

    [Header("AI Logic")]
    //MOVEMENT
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    public Transform[] waypoints; // Array of points to move to
    bool canMove;
    public float walkWaitTime = 2f; // Time to wait at each waypoint

    public int currentWaypointIndex; // Index of the current waypoint

    public Animator SecurityGuardAnimator;

    //SHOOTING
    GameObject target;
    public bool shoot;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        currentWaypointIndex = 0;
        canMove = true;
    }

    void Update()
    {
        Shoot();

        if (waypoints.Length > 0 && canMove == true)
        {
            StartCoroutine(MoveToWaypoints());
        }
        else
        {
            StopCoroutine(MoveToWaypoints());
        }

        Die();
    }
   
    private IEnumerator MoveToWaypoints()
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

    void Die()
    {
        if(health <= 0)
        {
            agent.speed = 0;
            SecurityGuardAnimator.SetBool("Dead", true);
        }
    }

    void Shoot()
    {
        if (shoot == true)
        {
            //stop moving
            canMove = false;
            agent.speed = 0;

            SecurityGuardAnimator.SetBool("PlayerDetected", true);

            //pull weapon
            SecurityGuardAnimator.SetBool("PlayerInRange", true);

            //attack
            SecurityGuardAnimator.SetBool("Attack", true);
        }
        else if(shoot == false)
        {
            //stop attack
            SecurityGuardAnimator.SetBool("Attack", false);

            //holster weapon
            SecurityGuardAnimator.SetBool("PlayerInRange", false);

            SecurityGuardAnimator.SetBool("PlayerDetected", false);

            //start moving
            canMove = true;
            agent.speed = 3.5f;
        }
    }
}
