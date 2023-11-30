using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GyroscopeModule : Module
{
    [Range(0f, 1f)]
    [SerializeField]
    private float gyroscopeDecalibration;

    [SerializeField]
    private float decalibrationMultiplier = 1f;

    [SerializeField]
    private float minCooldown;

    [SerializeField]
    private float maxCooldown;

    [SerializeField]
    private float coordinatesShiftDistance;

    private float currentCooldown;

    private float lastRotationTime;

    [SerializeField]
    private ShootingModule shootingModule;

    [SerializeField]
    private Rigidbody2D mechBody;

    private new void Start()
    {
        base.Start();
        CalculateNewCooldown();
        lastRotationTime = Time.time;

        moduleEventManager.onGyroscopeModuleUpdate.AddListener((id, number) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                gyroscopeDecalibration = 1 - number;
                Debug.Log(number);
            }
        });
    }
    public override void Perform()
    {
        if (Time.time > lastRotationTime + currentCooldown)
        {
            Debug.Log("Destabilize");
            CalculateNewCooldown();
            lastRotationTime = Time.time;
            Rotate();
        }
    
    }

    private void Rotate()
    {
        float leftRightChance = Random.Range(0f, 1f);
        int direction;
        if (leftRightChance < .5f)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        mechBody.AddTorque(shootingModule.GetRotationMultiplier() + (gyroscopeDecalibration * direction * decalibrationMultiplier));
    }

    private void CalculateNewCooldown()
    {
        currentCooldown = Random.Range(minCooldown, maxCooldown);
    }

    public override void SetEnergyBehaviour(bool isLowEnergy)
    {
        throw new System.NotImplementedException();
    }
    

}
