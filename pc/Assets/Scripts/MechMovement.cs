using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum MechVelocity
{
    REVERSE_FULL = -3,
    REVERSE_HALF = -2,
    REVERSE_QUARTER = -1,
    STOP = 0,
    FORWARD_QUARTER = 1,
    FORWARD_HALF = 2,
    FORWARD_FULL = 3
}

enum MechRotationSpeed
{
    ROTATE_LEFT = -1,
    STOP = 0,
    ROTATE_RIGHT = 1
}

public class MechMovement : MonoBehaviour
{
    public float acceleration = 5f; // Adjust the speed as needed
    public float maxSpeed = 20f;
    public float slowSpeed = 10f;
    public float rotationSpeed = 30.0f; // Adjust the speed as needed
    public float maxRotationSpeed = 100.0f;
    public float angularSlow = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Rotate(int rotationInput)
    {
        float rotationAngle = -1 * rotationInput * rotationSpeed * Time.deltaTime; //-1 is needed -> when we click right, we rotate to right
        if (rb.angularVelocity > 0 && rb.angularVelocity > maxRotationSpeed) rb.angularVelocity = maxRotationSpeed;
        else if (rb.angularVelocity < 0 && Mathf.Abs(rb.angularVelocity) > maxRotationSpeed) rb.angularVelocity = -maxRotationSpeed;
        rb.AddTorque(rotationAngle);
    }

    public void Move(int velocity)
    {
        Vector2 direction = velocity * rb.transform.up;

        rb.AddForce(direction * acceleration);
        if(rb.velocity.magnitude >= maxSpeed) { rb.velocity = rb.velocity.normalized * maxSpeed; }
    }

    public void StopMovement()
    {
        Vector2 slowVector = slowSpeed * rb.velocity.normalized;
        if (slowVector.magnitude >= rb.velocity.magnitude)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.AddForce(-slowVector);
        }
    }
    
    public void StopRotation()
    {
        float angularVelocity = rb.angularVelocity;
        float predictedVelocity = Mathf.Abs(angularVelocity) - angularSlow;
        if(predictedVelocity <= 0)
        {
            rb.angularVelocity = 0;
        }
        else
        {
            if (angularVelocity > 0)
            {
                rb.AddTorque(-angularSlow * Time.deltaTime);
            }
            else
            {
                rb.AddTorque(angularSlow * Time.deltaTime);
            }
        }
    }
}
