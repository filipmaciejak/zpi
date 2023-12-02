using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MechMainEnergy : MonoBehaviour
{
    [SerializeField]
    private float maxEnergy = 100;

    private float currentEnergy;

    private void Start()
    {
        currentEnergy = maxEnergy;
    }

    public bool AddEnergy(float energy)
    {
        currentEnergy += energy;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        else if (currentEnergy < 0)
        {
            currentEnergy = 0;
            return false;
        }
        return true;
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
