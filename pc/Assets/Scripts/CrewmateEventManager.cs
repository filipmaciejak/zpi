using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrewmateEventManager : MonoBehaviour
{
    public static CrewmateEventManager instance;

    public UnityEvent<int, float /* x */, float /* y */> onCrewmateMoveInputUpdate;
    public UnityEvent<int> onCrewmateButtonAPushed;
    public UnityEvent<int> onCrewmateButtonAReleased;
    public UnityEvent<int> onCrewmateButtonBPushed;
    public UnityEvent<int> onCrewmateButtonBReleased;

    void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        
        onCrewmateMoveInputUpdate = new UnityEvent<int, float, float>();
        onCrewmateButtonAPushed = new UnityEvent<int>();
        onCrewmateButtonAReleased = new UnityEvent<int>();
        onCrewmateButtonBPushed = new UnityEvent<int>();
        onCrewmateButtonBReleased = new UnityEvent<int>();
    }

    void Start()
    {
        onCrewmateMoveInputUpdate.AddListener(
            (id, inputX, inputY) => {
                Debug.Log("CrewmateEventManager: onCrewmateMoveInputUpdate<" + id + ">");
            }
        );

        onCrewmateButtonAPushed.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonAPushed<" + id + ">");
            }
        );

        onCrewmateButtonAReleased.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonAReleased<" + id + ">");
            }
        );

        onCrewmateButtonBPushed.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonBPushed<" + id + ">");
            }
        );
        
        onCrewmateButtonBReleased.AddListener(
            (id) => {
                Debug.Log("CrewmateEventManager: onCrewmateButtonBReleased<" + id + ">");
            }
        );
    }
}
