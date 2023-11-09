using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingModule : Module
{
    MechShooting shooting;

    void Start()
    {
        shooting = mech.GetComponentInChildren<MechShooting>();
    }

    void Update()
    {
        if (isBeingUsed)
        {
            Debug.Log("Strzelam");
            shooting.ShootBullet();
        }
    }


}
