using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldChargingModule : Module
{
    private MechShield mechShield;

    [SerializeField]
    private int shieldBaseRechargeAmount;


    public override void Perform()
    {
        //
    }

    public override void SetEnergyBehaviour(bool isLowEnergy)
    {
        this.lowEnergyMode = isLowEnergy;
    }

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        mechShield = mech.GetComponent<MechShield>();

        moduleEventManager.onShieldModuleUpdate.AddListener(id =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                mechShield.SetShield(mechShield.GetShield() + Mathf.RoundToInt(shieldBaseRechargeAmount * (lowEnergyMode?lowEnergyModeMulltiplier:1f)));
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
