using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GyroscopeModuleManager : MonoBehaviour
{
    public SpriteRenderer calibrationBar;
    public Button buttonA;
    public Button buttonB;
    public GameObject aim;
    public TextMeshProUGUI text;

    private ClientManager _clientManager;

    private Texture2D fullBarTexture;
    private float calibrationValue;
    private float maxDistanceFromTarget;
    private float forceFactor;
    private bool isCalibrating;

    public void Awake()
    {
        _clientManager = GameManager.Instance.clientManager;
    }

    void Start()
    {
        InputSystem.EnableDevice(AttitudeSensor.current);
        fullBarTexture = Resources.Load("FullCalibrationBar") as Texture2D;
        calibrationValue = 0.4f;
        maxDistanceFromTarget = 4f;
        forceFactor = 10.0f;
        isCalibrating = false;
    }

    public void OnEnable()
    {
        buttonB.EndedPress += OnButtonBEndedPress;
        buttonA.EndedPress += OnButtonAEndedPress;
    }

    public void OnDisable()
    {
        buttonB.EndedPress -= OnButtonBEndedPress;
        buttonA.EndedPress -= OnButtonAEndedPress;
    }

    void Update()
    {
        Vector3 attitude = AttitudeSensor.current.attitude.ReadValue().eulerAngles;

        text.SetText( $"Attitude\nX={attitude.x:#0.00} Y={attitude.y:#0.00} Z={attitude.z:#0.00}\n\n" +
                        $"Result\n{calibrationValue}\n\n" +
                        $"Distance\n{aim.GetComponent<Transform>().localPosition.magnitude}");

        calibrationValue = (maxDistanceFromTarget - aim.GetComponent<Transform>().localPosition.magnitude) / maxDistanceFromTarget;
        calibrationBar.sprite = Sprite.Create(fullBarTexture, new Rect(0, 0, fullBarTexture.width, fullBarTexture.height * calibrationValue), new Vector2(0.5f, fullBarTexture.height / (2 * fullBarTexture.height * calibrationValue)));
    }

    private void FixedUpdate()
    {
        if(isCalibrating)
        {
            Vector3 attitude = AttitudeSensor.current.attitude.ReadValue().eulerAngles;
            var attitudeX = attitude.x;
            if (attitudeX > 180)
                attitudeX -= 360;
            var attitudeY = attitude.y;
            if (attitudeY > 180)
                attitudeY -= 360;
            //swapping x and y here is not a mistake.
            var northFacingDirection = new Vector2(attitudeY, -attitudeX).normalized;
            //adjusting for orientation
            float angle = - Mathf.PI * attitude.z / 180.0f;
            var adjustedDirection = new Vector2(northFacingDirection.x * Mathf.Cos(angle) - northFacingDirection.y * Mathf.Sin(angle), northFacingDirection.x * Mathf.Sin(angle) + northFacingDirection.y * Mathf.Cos(angle));
            aim.GetComponent<Rigidbody2D>().AddForce(adjustedDirection.normalized * forceFactor);
        }
    }

    void OnButtonBEndedPress(string placeholder)
    {
        SceneManager.LoadScene("MovementScene");
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.ABORT_MINIGAME.ToString() },
        };

        _clientManager.SendDict(sentDict);
    }

    void OnButtonAEndedPress(string placeholder)
    {
        isCalibrating = !isCalibrating;
        aim.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
            { "parameter", calibrationValue.ToString("0.00") },
        };

        _clientManager.SendDict(sentDict);
    }
}
