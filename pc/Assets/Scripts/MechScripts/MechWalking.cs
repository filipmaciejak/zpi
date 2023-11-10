using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MechWalking : MonoBehaviour
{
    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private float accTime = 0.2f;
    [SerializeField]
    private float stepTime = 0.4f;

    [SerializeField]
    private float stepCooldown = 0.7f;

    private float timeCounter;
    private float mass;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mass = rb.mass;
        rb.centerOfMass = Vector3.zero;
        timeCounter = 0;
        maxVelocity += rb.drag;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyStepForce();
    }

    float GetStepCycleTime()
    {
        return accTime + stepTime + stepCooldown;
    }
    
    float CalculateAccForce()
    {
        return maxVelocity * mass / accTime;
    }

    void ApplyStepForce()
    {
        timeCounter += Time.deltaTime;
        if(timeCounter >= GetStepCycleTime()) { timeCounter = 0; }
        else if(timeCounter >= 0 && timeCounter <= accTime) 
        {
            Vector2 force = rb.transform.up * CalculateAccForce();
            rb.AddForce(force);
        }

    }
}
