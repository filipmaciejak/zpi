using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField]
    GameObject mech1;
    [SerializeField]
    GameObject mech2;
    MechRotation mechRotation1;
    MechWalking mechWalking1;
    MechShooting mechShooting1;
    BodyMovement mechBody1;
    MechRotation mechRotation2;
    MechWalking mechWalking2;
    MechShooting mechShooting2;
    BodyMovement mechBody2;

    void Start()
    {
        mechRotation1 = mech1.GetComponentInChildren<MechRotation>();
        mechBody1 = mech1.GetComponentInChildren<BodyMovement>();
        mechWalking1 = mech1.GetComponentInChildren<MechWalking>();
        mechShooting1 = mech1.GetComponentInChildren<MechShooting>();
        mechRotation2 = mech2.GetComponentInChildren<MechRotation>();
        mechShooting2 = mech2.GetComponentInChildren<MechShooting>();
        mechBody2 = mech2.GetComponentInChildren<BodyMovement>();
        mechWalking2 = mech2.GetComponentInChildren<MechWalking>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    
    void ExecuteMech1Movement()
    {
        mechRotation1.Rotate();
        mechWalking1.ApplyStepForce();
        mechBody1.Rotate();
    }
}
