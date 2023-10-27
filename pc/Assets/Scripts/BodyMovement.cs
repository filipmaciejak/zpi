using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float rotationSpeed = 30.0f; // Adjust the speed as needed
    public float maxRotationSpeed = 100.0f;
    public float angularSlow = 5f;

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

    public void StopRotation()
    {
        float angularVelocity = rb.angularVelocity;
        float predictedVelocity = Mathf.Abs(angularVelocity) - angularSlow;
        if (predictedVelocity <= 0)
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
    
    public void StopRotationNow()
    {
        rb.angularVelocity = 0;
    }
}
