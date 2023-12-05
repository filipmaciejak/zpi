using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MechMainEnergy : MonoBehaviour
{
    [SerializeField]
    private float maxEnergy = 100;

    [SerializeField]
    private float currentEnergy;

    public UnityEvent<bool> onEnergyModeChange;
    private float predictedEnergyChange = 0;
    private bool isLowEnergyMode = false;
    public void Awake()
    {
        onEnergyModeChange = new();
    }
    public void Start()
    {
        currentEnergy = maxEnergy;
    }
    public void LateUpdate()
    {
        if (predictedEnergyChange != 0)
        {
            currentEnergy += predictedEnergyChange;
            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
            else if (currentEnergy <= 0)
            {
                currentEnergy = 0;
                onEnergyModeChange.Invoke(true);
                Debug.Log("Low energy mode on ");
                isLowEnergyMode = true;
            }
            else if(isLowEnergyMode && currentEnergy > 0)
            {
                onEnergyModeChange.Invoke(false);
            }
            predictedEnergyChange = 0;
        }
    }
    public void ChangeEnergy(float energy)
    {
        predictedEnergyChange += energy;
    }

    public float GetCurrentEnergy()
    {
        if (currentEnergy >= 0) { return currentEnergy; }
        return currentEnergy;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

}
