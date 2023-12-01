using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    CrewmateEventManager eventManager;

    void Start()
    {
        eventManager = CrewmateEventManager.instance;
    }

    void Update()
    {


        float x = 0f;
        float y = 0f;

        if (Input.GetKey(KeyCode.A)) {
            x += -1f;
        }

        if (Input.GetKey(KeyCode.D)) {
            x += 1f;
        }

        if (Input.GetKey(KeyCode.W)) {
            y += 1f;
        }

        if (Input.GetKey(KeyCode.S)) {
            y -= 1f;
        }

        eventManager.onCrewmateMoveInputUpdate.Invoke(0, x, y);

        if (Input.GetKeyDown(KeyCode.Space)) {
            eventManager.onCrewmateButtonAPushed.Invoke(0);
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            eventManager.onCrewmateButtonAReleased.Invoke(0);
        }
        
    }
}
