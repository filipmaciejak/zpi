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


    new void Start()
    {
        base.Start();
        Transform body = mech.transform.Find("Body");
        rb = body.GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector3.zero;
        cannon = body.Find("Cannon").GetComponent<MechShooting>();

        moduleEventManager.onCannonModuleFired.AddListener((id) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                Shoot();
            }
        });

        moduleEventManager.onCannonAimModuleUpdate.AddListener((id, rotationDirection) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                ChangeRotationMultiplier(rotationDirection);
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


}