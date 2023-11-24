using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModuleEventManager : MonoBehaviour
{
    public static ModuleEventManager instance;

    public UnityEvent<int> onMinigameAborted;
    public UnityEvent<int, Module.Type> onModuleEntered;

    public UnityEvent<int, Dictionary<string, string>> onMinigameInitialized;

    public UnityEvent<int> onEnergyModuleUpdate;
    public UnityEvent<int, float> onGyroscopeModuleUpdate;
    public UnityEvent<int> onShieldModuleUpdate;
    public UnityEvent<int, float> onSpeedModuleUpdate;
    public UnityEvent<int, float> onSteeringModuleUpdate;

    void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        
        onMinigameAborted = new UnityEvent<int>();
        onModuleEntered = new UnityEvent<int, Module.Type>();
        onMinigameInitialized = new UnityEvent<int, Dictionary<string, string>>();

        onEnergyModuleUpdate = new UnityEvent<int>();
        onGyroscopeModuleUpdate = new UnityEvent<int, float>();
        onShieldModuleUpdate = new UnityEvent<int>();
        onSpeedModuleUpdate = new UnityEvent<int, float>();
        onSteeringModuleUpdate = new UnityEvent<int, float>();
    }

    void Start()
    {
        onMinigameAborted.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onMinigameAborted<" + id + ">");
            }
        );

        onModuleEntered.AddListener(
            (id, type) => {
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
                Debug.Log("ModuleEventManager: onGyroscopeModuleUpdate<" + id + ", " + parameter + ">");
            }
        );
        
        onShieldModuleUpdate.AddListener(
            (id) => {
                Debug.Log("ModuleEventManager: onShieldModuleUpdate<" + id + ">");
            }
        );

        onSpeedModuleUpdate.AddListener(
            (id, speed_pos) => {
                Debug.Log("ModuleEventManager: onSpeedModuleUpdate<" + id + ", " + speed_pos + ">");
            }
        );

        onSteeringModuleUpdate.AddListener(
            (id, steering_pos) => {
                Debug.Log("ModuleEventManager: onSteeringModuleUpdate<" + id + ", " + steering_pos + ">");
            }
        );
    }
}
