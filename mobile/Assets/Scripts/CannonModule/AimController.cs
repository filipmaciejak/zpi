using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;

public class AimController : MonoBehaviour
{
    public event Action<float> AimPositionUpdated;
    
    public float aimPosition = 0f;

    //angle 0 is completely flat
    public float deadZoneAngle = 10f;

    public float maxAngle = 90f;
    
    
    private void Start()
    {
        InputSystem.EnableDevice(AttitudeSensor.current);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 attitude = AttitudeSensor.current.attitude.ReadValue().eulerAngles;
        
        var attitudeX = attitude.x;
        if (attitudeX > 180)
            attitudeX -= 360;
        var attitudeY = attitude.y;
        if (attitudeY > 180)
            attitudeY -= 360;
        var northFacingDirection = new Vector2(attitudeY, -attitudeX);
        float angle = - Mathf.PI * attitude.z / 180.0f;
        var adjustedDirection = new Vector2(
            northFacingDirection.x * Mathf.Cos(angle) - northFacingDirection.y * Mathf.Sin(angle), 
            northFacingDirection.x * Mathf.Sin(angle) + northFacingDirection.y * Mathf.Cos(angle));

        var tiltAngle = adjustedDirection.x;
        
        float result = 0;
        
        // In deadzone
        if ((tiltAngle > 0 && tiltAngle < deadZoneAngle) ||
            (tiltAngle < 0 && tiltAngle > -deadZoneAngle))
        {
            result = 0;
            aimPosition = result;
            AimPositionUpdated?.Invoke(aimPosition);
            return;
        }

        bool overLeftMax = tiltAngle < -maxAngle;
        bool overRightMax = tiltAngle > maxAngle;
        if (overLeftMax || overRightMax)
        {
            if (overLeftMax)
                result = -1;
            if (overRightMax)
                result = 1;

            aimPosition = result;
            AimPositionUpdated?.Invoke(aimPosition);
            return;
        }

        if(tiltAngle > 0)
            aimPosition = (tiltAngle - deadZoneAngle) / (maxAngle - deadZoneAngle);
        if(tiltAngle < 0)
            aimPosition = (tiltAngle + deadZoneAngle) / (maxAngle - deadZoneAngle);
        
        AimPositionUpdated?.Invoke(aimPosition);
    }
}
