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
    private float rotationSpeedMultiplier = 2f;

    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float direction;

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
       // if (rb.angularVelocity > 0 && rb.angularVelocity > maxRotationSpeed) rb.angularVelocity = maxRotationSpeed;
       // else if (rb.angularVelocity < 0 && Mathf.Abs(rb.angularVelocity) > maxRotationSpeed) rb.angularVelocity = -maxRotationSpeed;
        rb.AddTorque(rotationAngle);
        Debug.Log("Angular velocity: " + rb.angularVelocity);
    }
    public void Shoot()
    { 
        cannon.ShootBullet();

    }
    public void ChangeRotationMultiplier(float input)
    {
        rotationSpeed = Mathf.Abs(input);
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
        return rotationSpeed;
    }
    public override void Perform()
    {

        if (IsBeingUsed())
        {
            Debug.Log($"Rotating {rotationSpeed}");
            Rotate();
        }
    }

    private float GetRotationSpeed()
    {
        return rotationSpeedMultiplier * rotationSpeed * (lowEnergyMode ? lowEnergyModeMulltiplier: 1);
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