using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteBallController : MonoBehaviour
{
    public Rigidbody rb;

    public void RollBall(float rollForce)
    {
        // Apply a force to the ball to make it roll
        Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        rb.AddForce(forceDirection * rollForce);
    }

    public void StopBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
