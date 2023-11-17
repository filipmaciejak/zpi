using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrewmateEventManager : MonoBehaviour
{
    public static CrewmateEventManager instance;

    public UnityEvent<int, float, float> onCrewmateMoveInputUpdate;
    public UnityEvent<int> onCrewmateButtonAPushed;
    public UnityEvent<int> onCrewmateButtonAReleased;
    public UnityEvent<int> onCrewmateButtonBPushed;
    public UnityEvent<int> onCrewmateButtonBReleased;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        instance = this;

        onCrewmateMoveInputUpdate = new UnityEvent<int, float, float>();
        onCrewmateButtonAPushed = new UnityEvent<int>();
        onCrewmateButtonAReleased = new UnityEvent<int>();
        onCrewmateButtonBPushed = new UnityEvent<int>();
        onCrewmateButtonBReleased = new UnityEvent<int>();
    }
}
