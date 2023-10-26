using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField]
    GameObject mech1;
    MechMovement mechMovement;
    void Start()
    {
        mechMovement = mech1.GetComponent<MechMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mechMovement.Rotate(-1);
        }
        else if (Input.GetMouseButton(1))
        {
            mechMovement.Rotate(1);
        }
        else
        {
           // mechMovement.StopRotation();

        }
        if (Input.GetKey(KeyCode.W))
        {
            mechMovement.Move(1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mechMovement.Move(-1);
        }
        else
        {
            mechMovement.StopMovement();
        }
    }
}
