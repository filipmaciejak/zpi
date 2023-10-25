using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is only here temporarily to test interactions without mobile controls
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _id = 0;

    void Update()
    {
        CrewmateEventManager.instance.onCrewmateMoveInputUpdate.Invoke(_id, Input.GetAxisRaw("Horizontal"), 0f);
        if (Input.GetButtonDown("Jump"))
        {
            CrewmateEventManager.instance.onCrewmateJump.Invoke(_id);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            CrewmateEventManager.instance.onCrewmateInteractionStart.Invoke(_id);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            CrewmateEventManager.instance.onCrewmateInteractionEnd.Invoke(_id);
        }
    }
}
