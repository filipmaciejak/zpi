using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnergyModuleManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public SpriteRenderer batterySprite;
    public Button button;

    public Texture2D fullBatteryTexture;

    private ClientManager _clientManager;
    
    private float minAngle = 0.0f;
    private float maxAngle = 360.0f;

    private float angularMargin = 20f;
    private float accelerationMargin = 0.2f;
    private float accelerationReqirement = 0.115f;
    private float accelerationCeiling = 5.0f;

    private float currentEnergyInCell = 0.0f;
    private float maxEnergyInCell = 4.0f;

    public void Awake()
    {
        _clientManager = GameManager.Instance.clientManager;
    }
    void Start()
    {
        InputSystem.EnableDevice(LinearAccelerationSensor.current);
        InputSystem.EnableDevice(AttitudeSensor.current);
    }

    public void OnEnable()
    {
        button.EndedPress += OnButtonEndedPress;
    }

    public void OnDisable()
    {
        button.EndedPress -= OnButtonEndedPress;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 acceleration = LinearAccelerationSensor.current.acceleration.ReadValue();
        Vector3 attitude = AttitudeSensor.current.attitude.ReadValue().eulerAngles;
        
        if ((attitude.x < minAngle + angularMargin || attitude.x > maxAngle - angularMargin) &&
            (attitude.y < minAngle + angularMargin || attitude.y > maxAngle - angularMargin))
        {
            if(Math.Abs(acceleration.x) < accelerationMargin && Math.Abs(acceleration.y) < accelerationMargin)
            {
                if(Math.Abs(acceleration.z) > accelerationReqirement && Math.Abs(acceleration.z) < accelerationCeiling)
                {
                    currentEnergyInCell += Time.deltaTime;
                }
            }
        }

        if(currentEnergyInCell > maxEnergyInCell) {
            currentEnergyInCell -= maxEnergyInCell;
            SendMinigameSuccess();
        }

        Rect newSpriteRect;
        Vector2 newPivot;
	if(currentEnergyInCell == 0) {
	    newSpriteRect = new Rect(0,0,fullBatteryTexture.width, 0);
	    newPivot = new Vector2(0.5f, fullBatteryTexture.height / (2 * 1));
	} else {
	    newSpriteRect = new Rect(0,0,fullBatteryTexture.width, fullBatteryTexture.height * currentEnergyInCell / maxEnergyInCell);
	    newPivot = new Vector2(0.5f, fullBatteryTexture.height / (2 * fullBatteryTexture.height * currentEnergyInCell / maxEnergyInCell));
	}
        batterySprite.sprite = Sprite.Create(fullBatteryTexture, newSpriteRect, newPivot, 1);

        text.SetText( $"Acceleration\nX={acceleration.x:#0.00} Y={acceleration.y:#0.00} Z={acceleration.z:#0.00}\n\n" +
                        $"Attitude\nX={attitude.x:#0.00} Y={attitude.y:#0.00} Z={attitude.z:#0.00}\n\n" +
                        $"Result\n{currentEnergyInCell}");
    }

    void OnButtonEndedPress(string placeholder)
    {
        SceneManager.LoadScene("MovementScene");
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.ABORT_MINIGAME.ToString() },
        };

        _clientManager.SendDict(sentDict);
    }

    void SendMinigameSuccess()
    {
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
            { "energy", "1" },
        };

        _clientManager.SendDict(sentDict);
    }
}
