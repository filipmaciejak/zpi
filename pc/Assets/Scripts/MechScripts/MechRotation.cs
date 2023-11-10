using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum MechRotationSpeed
{
    ROTATE_LEFT = -1,
    STOP = 0,
    ROTATE_RIGHT = 1
}

public class MechRotation : MonoBehaviour
{

    public float maxRotationSpeed = 100f;
    public float baseRotationSpeed = 30.0f;
    public float rotationMultiplier = 0f;
    public float rotationDirection = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Rotate()
    {
        float rotationForce = baseRotationSpeed * rotationMultiplier * rotationDirection;
        if (rb.angularVelocity > 0 && rb.angularVelocity > maxRotationSpeed) rb.angularVelocity = maxRotationSpeed;
        else if (rb.angularVelocity < 0 && Mathf.Abs(rb.angularVelocity) > maxRotationSpeed) rb.angularVelocity = -maxRotationSpeed;
        rb.AddTorque(rotationForce);
    }

    public void ChangeRotationMultiplier(float rotationMultiplier)
    {
        this.rotationMultiplier = Mathf.Abs(rotationMultiplier);
        if(rotationMultiplier > 0)
        {
            rotationDirection = -1;
        }
        else if(rotationMultiplier < 0)
        {
            rotationDirection = 1;
        }
        else { rotationDirection = 0; }
    }
}
