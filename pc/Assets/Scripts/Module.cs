using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    public GameObject mech;
    public GameObject mechManager;
    public enum Type {ENERGY_MODULE, SHIELD_MODULE, GYROSCOPE_MODULE, MOVEMENT_MODULE, CANNON_MODULE};
    public Type type;

    [SerializeField]
    private float energyConsumption = 5;

    [SerializeField]
    private float consumeCooldown = 2f;

    [SerializeField]
    private float activeEnergyConsumptionMultiplier = 1.0f;

    protected MechMainEnergy mechEnergy;

    [SerializeField]
    protected bool lowEnergyMode = false;

    [Range(0f, 1f)]
    [SerializeField]
    protected float lowEnergyModeMulltiplier = .5f;

    protected int mechId;
    protected ModuleEventManager moduleEventManager;

    private float timeSinceLastEnergyConsumption;
    public bool isBeingUsed = false;
    public abstract void Perform();
    public bool IsBeingUsed()
    {
        return isBeingUsed;
    }

    public void Start()
    {
        mechManager.GetComponent<MechManager>().AddModule(this);
        mechEnergy = mech.GetComponent<MechMainEnergy>();
        timeSinceLastEnergyConsumption = Time.time;


        mechId = mech.GetComponent<MechState>().teamId;
        moduleEventManager = ModuleEventManager.instance;
    }

    public void ConsumeEnergy()
    {
        if(Time.time >= timeSinceLastEnergyConsumption + consumeCooldown) 
        {
            float energyToConsume = energyConsumption * (isBeingUsed ? activeEnergyConsumptionMultiplier : 1);
            timeSinceLastEnergyConsumption = Time.time;
            if (!mechEnergy.AddEnergy(-energyToConsume))
            {
                SetEnergyBehaviour(true);
            }
            else if (lowEnergyMode) { SetEnergyBehaviour(false); }
        }
    }

    public abstract void SetEnergyBehaviour(bool isLowEnergy);
}
