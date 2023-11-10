using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BodyMovement : MonoBehaviour
{

    private Rigidbody2D rb;


    private float rotationMultiplier;
    private float direction;
    [SerializeField]
    float baseRotationSpeed = 30.0f; // Adjust the speed as needed
    [SerializeField]
    float maxRotationSpeed = 100.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector3.zero;
    }

    public void Rotate()
    {
        float rotationAngle = direction * baseRotationSpeed * rotationMultiplier;
        if (rb.angularVelocity > 0 && rb.angularVelocity > maxRotationSpeed) rb.angularVelocity = maxRotationSpeed;
        else if (rb.angularVelocity < 0 && Mathf.Abs(rb.angularVelocity) > maxRotationSpeed) rb.angularVelocity = -maxRotationSpeed;
        rb.AddTorque(rotationAngle);
    }

    public void ChangeRotationMultiplier(float input)
    {
        rotationMultiplier = Mathf.Abs(input);
        if(input > 0)
        {
            direction = -1;
        }
        else if (input < 0)
        {
            direction = 1;
        }
        else
        {
            direction = 0;
        }
    }
}