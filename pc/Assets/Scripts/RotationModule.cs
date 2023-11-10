using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationModule : Module
{
    MechRotation movement;
    void Start()
    {
        movement = mech.GetComponent<MechRotation>();
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
