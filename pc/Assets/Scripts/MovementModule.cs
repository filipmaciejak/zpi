using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModule : Module
{
    MechRotation movement;
    Boolean isForward;
    public void Start()
    {
        movement = mech.GetComponent<MechRotation>();
    }

    public void Update()
    {
        if(isBeingUsed) {
            Move();
        }
        else
        {
            movement.StopMovement();
        }
    }
    public void Move()
    {
        if (isForward)
        {
            movement.Move(1);
        }
        else
        {
            movement.Move(-1);
        }
    }
}
