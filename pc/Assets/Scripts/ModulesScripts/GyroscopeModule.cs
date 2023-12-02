using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GyroscopeModule : Module
{
    [Range(0f, 1f)]
    [SerializeField]
    private float gyroscopeCalibration = 1f;

    [SerializeField]
    private float gyroscopeDecalibrationStep = 0.05f;

    [SerializeField]
    private float decalibrationMultiplier = 1f;

    [SerializeField]
    private float minCooldown;

    [SerializeField]
    private float maxCooldown;

    [SerializeField]
    private float destabilizationCooldown = 5f;

    private float currentCooldown;

    private float lastRotationTime;

    private float lastDestibilizationTime;

    [SerializeField]
    private ShootingModule cannonModule;

    [SerializeField]
    private Rigidbody2D mechBody;

    private new void Start()
    {
        base.Start();
        CalculateNewCooldown();
        lastRotationTime = Time.time;
        lastDestibilizationTime = Time.time;
        moduleEventManager.onGyroscopeModuleUpdate.AddListener((id, number) =>
        {
            if (moduleEventManager.teamIds.GetValueOrDefault(id, 0) == mechId)
            {
                gyroscopeCalibration = number;
                Debug.Log("Gyroscope new value" + number);
            }
        });
    }
    public override void Perform()
    {
        if (Time.time > lastRotationTime + currentCooldown)
        {
            lastRotationTime = Time.time;
            Debug.Log("Gyroscope: Rotate - Cooldown = " + currentCooldown * gyroscopeCalibration);
            Rotate();
        }
        if (Time.time > lastDestibilizationTime + destabilizationCooldown)
        {
            lastDestibilizationTime = Time.time;
            Debug.Log("Gyroscope: Destabilize");
            Destablize();
        }
    }

    private void Destablize()
    {
        if (!isBeingUsed)
        {
            gyroscopeCalibration -= gyroscopeDecalibrationStep;
            gyroscopeCalibration = Mathf.Max(gyroscopeCalibration, 0.05f);
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
        mechBody.AddTorque(cannonModule.GetRotationMultiplier() + ((1 - gyroscopeCalibration) * direction * decalibrationMultiplier));
    }

    private void CalculateNewCooldown()
    {
        currentCooldown = Random.Range(minCooldown, maxCooldown * gyroscopeCalibration);
    }

    public override void SetEnergyBehaviour(bool isLowEnergy)
    {
        //
    }
    
    public float GetGyroscopeDecalibration()
    {
        return gyroscopeCalibration;
    }


}
