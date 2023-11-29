using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeModule : Module
{
    private float x = 0;
    private float y = 0;

    [SerializeField]
    private float minCooldown;

    [SerializeField]
    private float maxCooldown;

    [SerializeField]
    private float coordinatesShiftDistance;

    private float currentCooldown;

    private float lastCoordShiftTime;

    [SerializeField]
    private ShootingModule shootingModule;
    private float DistanceFromZero
    {
        get
        {
            return x*x + y*y;
        }
    }

    private new void Start()
    {
        base.Start();
        CalculateNewCooldown();
        lastCoordShiftTime = Time.time;
    }
    public override void Perform()
    {
        Debug.Log("x: " + x + " y:" + y);
        Debug.Log("Current cooldown " + currentCooldown);
        if (Time.time > lastCoordShiftTime + currentCooldown)
        {
            CalculateNewCooldown();
            lastCoordShiftTime = Time.time;
            ShiftCoordinates();
            shootingModule.ChangeRotationMultiplier(shootingModule.GetRotationMultiplier() + DistanceFromZero * (x>0?1:-1));
        }
    
    }

    private void ShiftCoordinates()
    {
        float xLeftRightChance = Random.Range(0f, 1f);
        float yLeftRightChance = Random.Range(0f, 1f);

        Debug.Log("X chance " + xLeftRightChance);
        Debug.Log("Y chance " + yLeftRightChance);

        if (xLeftRightChance < 0.5) { x += coordinatesShiftDistance; }
        else { x -= coordinatesShiftDistance; }
        if(yLeftRightChance < 0.5) {  y += coordinatesShiftDistance; }
        else {  y -= coordinatesShiftDistance; }
    }

    private void CalculateNewCooldown()
    {
        currentCooldown = Random.Range(minCooldown, maxCooldown);
    }
    public void SetNewCoordinates(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public override void SetLowEnergyBehaviour(bool isLowEnergy)
    {
        throw new System.NotImplementedException();
    }
    

}
