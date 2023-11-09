using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    public event Action<float> AimPositionUpdated;
    
    public float aimPosition = 0f;

    //angle 0 is completely flat
    public float deadZoneAngle = 10f;

    public float maxRightAngle = 90f;
    public float maxLeftAngle = 270f;
    
    
    private void Awake()
    {
        InputSystem.EnableDevice(AttitudeSensor.current);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 attitude = AttitudeSensor.current.attitude.ReadValue().eulerAngles;
        float tiltAngle = attitude.y;

        float result = 0;
        if ((tiltAngle > 0 && tiltAngle < deadZoneAngle / 2) ||
            (tiltAngle < 360 && tiltAngle > 360 - deadZoneAngle / 2))
        {
            result = 0;
            aimPosition = result;
            return;
        }
        
        if (tiltAngle < maxLeftAngle && tiltAngle > maxRightAngle)
        {
            if (tiltAngle < 180)
                result = 1;
            if (tiltAngle >= 180)
                result = -1;
            aimPosition = result;
            return;
        }

        if (tiltAngle <= maxRightAngle)
        {
            aimPosition = tiltAngle / maxRightAngle;
        }

        if (tiltAngle >= maxLeftAngle)
        {
            float distanceFromCenter = Math.Abs(tiltAngle - 360);
            float maxDistanceFromCenter = Math.Abs(maxLeftAngle - 360);
            aimPosition = -(distanceFromCenter / maxDistanceFromCenter);
        }
        AimPositionUpdated?.Invoke(aimPosition);
    }
}
