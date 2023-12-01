using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootingModule : Module
{



    private Rigidbody2D rb;

    [SerializeField] 
    private bool isShooting;

    private MechShooting cannon;

    [SerializeField]
    private float rotationMultiplier;
    [SerializeField]
    private float direction;
    [SerializeField]
    float baseRotationSpeed = 30.0f;
    [SerializeField]
    float maxRotationSpeed = 100.0f;

    private bool isCannonLoaded = false;
    private bool isCannonOpened = true;

    new void Start()
    {
        base.Start();
        Transform body = mech.transform.Find("Body");
        rb = body.GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector3.zero;
        cannon = body.Find("Cannon").GetComponent<MechShooting>();

        moduleEventManager.onCannonModuleChamberLoaded.AddListener((id) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                isCannonLoaded = true;
            }
        });

        moduleEventManager.onCannonModuleFired.AddListener((id) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                Shoot();
                isCannonLoaded = false;
            }
        });

        moduleEventManager.onCannonAimModuleUpdate.AddListener((id, rotationDirection) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                Debug.Log(rotationDirection.ToString());
                ChangeRotationMultiplier(rotationDirection);
            }
        });

        moduleEventManager.onCannonModuleChamberClosed.AddListener((id) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                isCannonOpened = false;
            }
        });

        moduleEventManager.onCannonModuleChamberOpened.AddListener((id) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                isCannonOpened = true;
            }
        });
    }

    public void Rotate()
    {
        float rotationAngle = direction * GetRotationSpeed();
        if (rb.angularVelocity > 0 && rb.angularVelocity > maxRotationSpeed) rb.angularVelocity = maxRotationSpeed;
        else if (rb.angularVelocity < 0 && Mathf.Abs(rb.angularVelocity) > maxRotationSpeed) rb.angularVelocity = -maxRotationSpeed;
        rb.AddTorque(rotationAngle);
    }
    public void Shoot()
    {
        Debug.Log("Shoot!");
 
        cannon.ShootBullet();

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
    public float GetRotationMultiplier()
    {
        return rotationMultiplier;
    }
    public override void Perform()
    {

        if (IsBeingUsed())
        {
            Debug.Log($"Rotating {rotationMultiplier}");
            Rotate();
        }
    }

    private float GetRotationSpeed()
    {
        return baseRotationSpeed * rotationMultiplier * (lowEnergyMode ? lowEnergyModeMulltiplier: 1);
    }

    private float GetCannonCooldown()
    {
        return cannon.GetCooldown() * (lowEnergyMode ? 1 / lowEnergyModeMulltiplier : 1);
    }
    public override void SetEnergyBehaviour(bool isLowEnergy)
    {
        lowEnergyMode = isLowEnergy;
    }

    public bool IsCannonLoaded()
    {
        return isCannonLoaded;
    }

    public bool IsCannonClosed()
    {
        return isCannonOpened;
    }
}