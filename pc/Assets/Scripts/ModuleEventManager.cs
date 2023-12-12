using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModuleEventManager : MonoBehaviour
{
    public static ModuleEventManager instance;

    public UnityEvent<int> onMinigameAborted;
    public UnityEvent<int, Module.Type, Dictionary<string, string>> onModuleEntered;

    public UnityEvent<int, Dictionary<string, string>> onMinigameInitialized;

    public UnityEvent<int> onEnergyModuleUpdate;
    public UnityEvent<int, float> onGyroscopeModuleUpdate;
    public UnityEvent<int> onShieldModuleUpdate;

    public UnityEvent<int, float> onSpeedModuleUpdate;
    public UnityEvent<int, float> onSteeringModuleUpdate;

    public UnityEvent<int, float> onCannonAimModuleUpdate;
    public UnityEvent<int> onCannonModuleChamberOpened;
    public UnityEvent<int> onCannonModuleChamberClosed;
    public UnityEvent<int> onCannonModuleChamberLoaded;
    public UnityEvent<int> onCannonModuleChamberUnloaded;
    public UnityEvent<int> onCannonModuleFired;

    public Dictionary<int, int> teamIds;
    public List<List<int>> idsToRequest; //idsToRequest[teamId][index]


    void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        
        onMinigameAborted = new UnityEvent<int>();
        onModuleEntered = new UnityEvent<int, Module.Type, Dictionary<string, string>>();

        onEnergyModuleUpdate = new UnityEvent<int>();
        onGyroscopeModuleUpdate = new UnityEvent<int, float>();
        onShieldModuleUpdate = new UnityEvent<int>();

        onSpeedModuleUpdate = new UnityEvent<int, float>();
        onSteeringModuleUpdate = new UnityEvent<int, float>();

        onCannonAimModuleUpdate = new UnityEvent<int, float>();
        onCannonModuleChamberOpened = new UnityEvent<int>();
        onCannonModuleChamberClosed = new UnityEvent<int>();
        onCannonModuleChamberLoaded = new UnityEvent<int>();
        onCannonModuleChamberUnloaded = new UnityEvent<int>();
        onCannonModuleFired = new UnityEvent<int>();

        teamIds = new Dictionary<int, int>();

        idsToRequest = new List<List<int>>();
        idsToRequest.Add(new List<int>());
        idsToRequest.Add(new List<int>());
    }

    void Start()
    {
        onMinigameAborted.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onMinigameAborted<" + id + ">");
            }
        );

        onModuleEntered.AddListener(
            (id, type, dict) => {
                Debug.Log("ModuleEventManager: onModuleEntered<" + id + ", " + type + ">");
            }
        );

        onMinigameInitialized.AddListener(
            (id, dict) => {
                Debug.Log("ModuleEventManager: onModuleInitialized<" + id + ">");
            }
        );

        onEnergyModuleUpdate.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onEnergyModuleUpdate<" + id + ">");
            }
        );

        onGyroscopeModuleUpdate.AddListener(
            (id, parameter) => {
                Debug.Log("ModuleEventManager: onGyroscopeModuleUpdate<" + id + ", ...>");
            }
        );
        
        onShieldModuleUpdate.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onShieldModuleUpdate<" + id + ">");
            }
        );

        onSpeedModuleUpdate.AddListener(
            (id, speed_pos) => {
                Debug.Log("ModuleEventManager: onSpeedModuleUpdate<" + id + ", ...>");
            }
        );

        onSteeringModuleUpdate.AddListener(
            (id, steering_pos) => {
                Debug.Log("ModuleEventManager: onSteeringModuleUpdate<" + id + ", ...>");
            }
        );

        onCannonAimModuleUpdate.AddListener(
            (id, cannon_aim_pos) => {
                Debug.Log("ModuleEventManager: onCannonAimModuleUpdate<" + id + ", ...>");
            }
        );

        onCannonModuleChamberOpened.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onCannonModuleChamberOpened<" + id + ">");
            }
        );

        onCannonModuleChamberClosed.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onCannonModuleChamberClosed<" + id + ">");
            }
        );

        onCannonModuleChamberLoaded.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onCannonModuleChamberLoaded<" + id + ">");
            }
        );

        onCannonModuleChamberUnloaded.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onCannonModuleChamberUnloaded<" + id + ">");
            }
        );

        onCannonModuleFired.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onCannonModuleFired<" + id + ">");
            }
        );
    }
}
