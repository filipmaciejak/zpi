using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderModule : Module 
{
    public override void Perform()
    {
        Debug.Log("PlaceholderModule.Perform()");
    }

    public override void SetEnergyBehaviour(bool isLowEnergy)
    {
        // TODO: Implement
    }
}
