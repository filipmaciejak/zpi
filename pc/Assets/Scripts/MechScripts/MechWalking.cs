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
    private float baseStepCooldown = 0.6f;

    
    private float timeCounter;
    private float mass;
    private Rigidbody2D rb;
    private float direction = 0f;
    public float stepCooldownMultiplier = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mass = rb.mass;
        rb.centerOfMass = Vector3.zero;
        timeCounter = 0;
        maxVelocity += rb.drag;
    }

    float GetStepCycleTime()
    {
        return accTime + stepTime + baseStepCooldown * stepCooldownMultiplier;
    }

    float CalculateAccForce()
    {
        return maxVelocity * mass / accTime;
    }

    public void ApplyStepForce()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= GetStepCycleTime()) { timeCounter = 0; }
        else if (timeCounter >= 0 && timeCounter <= accTime)
        {
            Vector2 force = rb.transform.up * CalculateAccForce() * direction;
            rb.AddForce(force);
        }

    }

    public void ChangeStep(float input)
    {
        float multiplier = Mathf.Abs(input);
        stepCooldownMultiplier = multiplier;
        if (input > 0)
        {
            direction = 1;
        }
        else if (input < 0)
        {
            direction = -1;
        }
        else { direction = 0; }
    }
}