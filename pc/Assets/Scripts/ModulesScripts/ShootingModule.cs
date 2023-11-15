using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
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

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    void Start()
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    {
        base.Start();
        Transform body = mech.transform.Find("Body");
        rb = body.GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector3.zero;
        cannon = body.Find("Cannon").GetComponent<MechShooting>();
    }

    public void Rotate()
    {
        float rotationAngle = direction * baseRotationSpeed * rotationMultiplier;
        if (rb.angularVelocity > 0 && rb.angularVelocity > maxRotationSpeed) rb.angularVelocity = maxRotationSpeed;
        else if (rb.angularVelocity < 0 && Mathf.Abs(rb.angularVelocity) > maxRotationSpeed) rb.angularVelocity = -maxRotationSpeed;
        rb.AddTorque(rotationAngle);
    }
    public void Shoot()
    {
        if (isShooting)
        {
            cannon.ShootBullet();
        }
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

    public override void Perform()
    {
        Debug.Log("Body center of mass: " + rb.centerOfMass);
        Rotate();
        Shoot();
    }
}