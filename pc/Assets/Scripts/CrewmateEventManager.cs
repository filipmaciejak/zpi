using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrewmateEventManager : MonoBehaviour
{
    public static CrewmateEventManager instance;

    public UnityEvent<int /* crewmate id */, float /* x */, float /* y */> onCrewmateMoveInputUpdate;
    public UnityEvent<int /* crewmate id */> onCrewmateButtonAPushed;
    public UnityEvent<int /* crewmate id */> onCrewmateButtonAReleased;
    public UnityEvent<int /* crewmate id */> onCrewmateButtonBPushed;
    public UnityEvent<int /* crewmate id */> onCrewmateButtonBReleased;

    void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    void Start()
    {
        onCrewmateMoveInputUpdate = new UnityEvent<int, float, float>();
        onCrewmateMoveInputUpdate.AddListener(
            (id, inputX, inputY) => {
                Debug.Log("CrewmateEventManager: onCrewmateMoveInputUpdate<" + id + ">");
            }
        );
        onCrewmateButtonAPushed = new UnityEvent<int>();
        onCrewmateButtonAPushed.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonAPushed<" + id + ">");
            }
        );
        onCrewmateButtonAReleased = new UnityEvent<int>();
        onCrewmateButtonAReleased.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonAReleased<" + id + ">");
            }
        );
        onCrewmateButtonBPushed = new UnityEvent<int>();
        onCrewmateButtonBPushed.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonBPushed<" + id + ">");
            }
        );
        onCrewmateButtonBReleased = new UnityEvent<int>();
        onCrewmateButtonBReleased.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonBReleased<" + id + ">");
            }
        );
    }
}
