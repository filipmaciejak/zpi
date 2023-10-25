using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrewmateEventManager : MonoBehaviour
{
    public static CrewmateEventManager instance;

    public UnityEvent<int> onCrewmateJump;
    public UnityEvent<int, float, float> onCrewmateMoveInputUpdate;
    public UnityEvent<int> onCrewmateInteractionStart;
    public UnityEvent<int> onCrewmateInteractionEnd;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        onCrewmateJump = new UnityEvent<int>();
        onCrewmateMoveInputUpdate = new UnityEvent<int, float, float>();
        onCrewmateInteractionStart = new UnityEvent<int>();
        onCrewmateInteractionEnd = new UnityEvent<int>();
    }
}
