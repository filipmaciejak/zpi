using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingModule : Module
{
    MechShooting shooting;

    void Start()
    {
        shooting = mech.GetComponent<MechShooting>();
    }

    void Update()
    {
        if (isBeingUsed)
        {
            shooting.ShootBullet();
        }
    }


}
