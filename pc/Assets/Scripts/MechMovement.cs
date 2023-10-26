using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MechMovement : MonoBehaviour
{
    public float acceleration = 5f; // Adjust the speed as needed
    public float maxSpeed = 20f;
    public float slowSpeed = 10f;
    public float rotationSpeed = 100.0f; // Adjust the speed as needed
    public float angularSlow = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    public void Rotate(float rotationInput)
    {
        float rotationAngle = -1 * rotationInput * rotationSpeed * Time.deltaTime; //-1 is needed -> when we click right, we rotate to right

        rb.AddTorque(rotationAngle);
        Debug.Log(rb.angularVelocity);
    }

    public void Move(int movementDirection)
    {
        Vector2 direction = movementDirection * rb.transform.up;

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
