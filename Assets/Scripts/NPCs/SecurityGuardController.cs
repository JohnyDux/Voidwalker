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
    int currentWaypointIndex;
    public Animator SecurityGuardAnimator;

    //SHOOTING
    GameObject target;
    public bool shoot;

    //VISION CONE
    public float viewRadius = 5f;
    [Range(0, 360)] public float viewAngle = 90f;
    public float heightOffset = 0.5f;
    public float detectionRefreshRate = 0.2f;

    public Transform coneParent;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool showVisionCone = true;
    public Color detectionColor = Color.red;
    public Color noDetectionColor = Color.yellow;
    public Color searchColor = Color.magenta;
    [Range(3, 60)] public int coneResolution = 20;

    //TARGET
    private Transform currentTarget;
    private Vector3 lastKnownPosition;

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
            {
                // Set the destination to the current waypoint
                agent.SetDestination(waypoints[currentWaypointIndex].position);
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop back to the first waypoint
            }
        }
    }

    void Die()
    {
        canMove = false;
        SecurityGuardAnimator.SetBool("Dead", true);
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
        else if (shoot == false)
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

    void FindVisibleTargets()
    {
        // Use the cone parent's position for detection
        Vector3 detectionOrigin = coneParent.position + Vector3.up * heightOffset;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(detectionOrigin, viewRadius, targetMask);

        foreach (Collider targetCollider in targetsInViewRadius)
        {
            Transform target = targetCollider.transform;
            Vector3 dirToTarget = (target.position - detectionOrigin).normalized;

            // Use the cone parent's forward direction for angle calculation
            if (Vector3.Angle(coneParent.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(detectionOrigin, target.position);
                canMove = false;
                if (!Physics.Raycast(detectionOrigin, dirToTarget, dstToTarget, obstacleMask))
                {
                    currentTarget = target;
                    lastKnownPosition = target.position;
                    agent.SetDestination(lastKnownPosition);
                    return;
                }
            }
        }

        if (currentTarget != null)
        {
            currentTarget = null;
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            canMove = true;
        }
    }
}