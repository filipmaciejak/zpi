using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRechargingModule : Module
{

    [SerializeField]
    private float amountOfEnergyToRecharge;

    private MechMainEnergy mechEnergy;
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

        mechEnergy = mech.GetComponent<MechMainEnergy>();

        moduleEventManager.onEnergyModuleUpdate.AddListener(id => 
        {
            if(moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                mechEnergy.AddEnergy(amountOfEnergyToRecharge);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
