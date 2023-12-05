using System.Collections.Generic;
using UnityEngine;

public class EnergyRechargingModule : Module
{

    [SerializeField]
    private float amountOfEnergyToRecharge;

    public override void Perform()
    {
        //
    }

    public override void SetEnergyBehaviour(bool isLowEnergy)
    {
        //
    }

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

        moduleEventManager.onEnergyModuleUpdate.AddListener(id => 
        {
            if(moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                mechEnergy.ChangeEnergy(amountOfEnergyToRecharge);
            }
        });
    }
}
