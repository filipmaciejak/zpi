using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MechWalkingModule : Module
{
    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private float accTime = 0.2f;
    [SerializeField]
    private float stepTime = 0.4f;

    [SerializeField]
    private float baseStepCooldown = 1.5f;

    [SerializeField]
    private float maxRotationSpeed = 100f;

    [SerializeField]
    private float baseRotationSpeed = 30.0f;

    [SerializeField]
    private float rotationMultiplier = 0f;

    [SerializeField]
    private float rotationDirection = 0f;

    private float timeCounter;
    private float mass;
    private Rigidbody2D rb;

    [SerializeField]
    private float direction = 0f;

    [SerializeField]
    private float stepCooldownMultiplier = 1f;

    new void Start()
    {
        base.Start();
        rb = mech.transform.Find("Legs").GetComponent<Rigidbody2D>();
        mass = rb.mass;
        rb.centerOfMass = Vector3.zero;
        timeCounter = 0;
        maxVelocity += rb.drag;
    }

    float GetStepCycleTime()
    {
        return accTime + stepTime + GetStepCooldown();
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
            Vector2 force = CalculateAccForce() * direction * rb.transform.up;
            rb.AddForce(force);
        }

    }
    public void ChangeStep(float input)
    {
        float multiplier = Mathf.Abs(input);
        stepCooldownMultiplier = 1/multiplier;
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

    public void Rotate()
    {
        float rotationForce = GetRotationSpeed() * rotationDirection;
        if (rb.angularVelocity > 0 && rb.angularVelocity > maxRotationSpeed) rb.angularVelocity = maxRotationSpeed;
        else if (rb.angularVelocity < 0 && Mathf.Abs(rb.angularVelocity) > maxRotationSpeed) rb.angularVelocity = -maxRotationSpeed;
        rb.AddTorque(rotationForce);
    }

    public void ChangeRotationMultiplier(float rotationMultiplier)
    {
        this.rotationMultiplier = Mathf.Abs(rotationMultiplier);
        if (rotationMultiplier > 0)
        {
            rotationDirection = -1;
        }
        else if (rotationMultiplier < 0)
        {
            rotationDirection = 1;
        }
        else { rotationDirection = 0; }
    }
    public override void Perform()
    {
        ApplyStepForce();
        Rotate();
    }

    public override void SetLowEnergyBehaviour(bool isLowEnergy)
    {
        lowEnergyMode = isLowEnergy;
    }

    public float GetRotationSpeed()
    {
        return baseRotationSpeed * rotationMultiplier * (lowEnergyMode ? lowEnergyModeMulltiplier : 1);
    }

    public float GetStepCooldown()
    {
        return baseStepCooldown * (lowEnergyMode ? 3 : stepCooldownMultiplier);
    } 


}