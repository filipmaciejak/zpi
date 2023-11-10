using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField]
    GameObject mech1;
    [SerializeField]
    GameObject mech2;
    MechMovement mechMovement1;
    MechShooting mechShooting1;
    BodyMovement mechBody1;
    MechMovement mechMovement2;
    MechShooting mechShooting2;
    BodyMovement mechBody2;

    void Start()
    {
        mechMovement1 = mech1.GetComponentInChildren<MechMovement>();
        mechBody1 = mech1.GetComponentInChildren<BodyMovement>();
        mechShooting1 = mech1.GetComponentInChildren<MechShooting>();
        mechMovement2 = mech2.GetComponentInChildren<MechMovement>();
        mechShooting2 = mech2.GetComponentInChildren<MechShooting>();
        mechBody2 = mech2.GetComponentInChildren<BodyMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       CheckForMech1Movement();
       CheckForMech2Movement();
    }
    void CheckForMech1Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            mechMovement1.Rotate(1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            mechMovement1.Rotate(-1);
        }
        else
        {
            mechMovement1.StopRotation();
        }
        if (Input.GetKey(KeyCode.W))
        {
            mechMovement1.Move(1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mechMovement1.Move(-1);
        }
        else { 
            mechMovement1.StopMovement();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            mechShooting1.ShootBullet();
        }
        if (Input.GetKey(KeyCode.I))
        {
            mechBody1.Rotate(-1);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            mechBody1.Rotate(1);
        }
        else
        {
            mechBody1.StopRotationNow();
        }

    }
    void CheckForMech2Movement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mechMovement2.Rotate(1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            mechMovement2.Rotate(-1);
        }
        else
        {
            mechMovement2.StopRotation();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mechMovement2.Move(1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            mechMovement2.Move(-1);
        }
        else
        {
            //mechMovement2.StopMovement();
        }
        if (Input.GetKey(KeyCode.Slash))
        {
            mechShooting2.ShootBullet();
        }
        if (Input.GetKey(KeyCode.Semicolon))
        {
            mechBody2.Rotate(-1);
        }
        else if (Input.GetKey(KeyCode.Quote))
        {
            mechBody2.Rotate(1);
        }
        else
        {
            mechBody2.StopRotationNow();
        }
    }
}
