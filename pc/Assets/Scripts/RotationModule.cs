using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationModule : Module
{
    MechMovement movement;
    void Start()
    {
        movement = mech.GetComponent<MechMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingUsed)
        {
            movement.Rotate(1);
        }
        else
        {
            movement.StopRotation();
        }
    }
}
