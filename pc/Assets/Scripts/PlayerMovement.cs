using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public int id;
    CrewmateEventManager eventManager;

    void Start()
    {
        eventManager = CrewmateEventManager.instance;
    }

    void Update()
    {

        bool anyInput = false;

        if (Input.GetKey(KeyCode.A)) {
            eventManager.onCrewmateMoveInputUpdate.Invoke(0, -1, 0);
            anyInput = true;
        }

        if (Input.GetKey(KeyCode.D)) {
            eventManager.onCrewmateMoveInputUpdate.Invoke(0, 1, 0);
            anyInput = true;
        }

        if (Input.GetKey(KeyCode.W)) {
            eventManager.onCrewmateMoveInputUpdate.Invoke(0, 0, 1);
            anyInput = true;
        }

        if (Input.GetKey(KeyCode.S)) {
            eventManager.onCrewmateMoveInputUpdate.Invoke(0, 0, -1);
            anyInput = true;
        }

        if (!anyInput) {
            eventManager.onCrewmateMoveInputUpdate.Invoke(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            eventManager.onCrewmateButtonAPushed.Invoke(0);
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            eventManager.onCrewmateButtonAReleased.Invoke(0);
        }
        
    }
}
