using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    private ClientManager _clientManager;

    public JoystickController joystick;
    public List<Button> buttons;


    public void Awake()
    {
        _clientManager = GameManager.Instance.clientManager;
    }

    private void OnEnable()
    {
        //Add listeners for the above elements for the connection manager
        joystick.StartedControlling += OnStartedControlling;
        joystick.MovedControls += OnMovedControls;
        joystick.StoppedControlling += OnStoppedControlling;
        
        foreach(var button in buttons)
        {
            button.StartedPress += OnStartedPress;
            button.EndedPress += OnEndedPress;
        }
    }

    private void OnDisable()
    {
        joystick.StartedControlling -= OnStartedControlling;
        joystick.MovedControls -= OnMovedControls;
        joystick.StoppedControlling -= OnStoppedControlling;
        
        foreach(var button in buttons)
        {
            button.StartedPress -= OnStartedPress;
            button.EndedPress -= OnEndedPress;
        }
    }

    public void AddButton(Button button)
    {
        buttons.Add(button);
        button.StartedPress += OnStartedPress;
        button.EndedPress += OnEndedPress;
    }

    public void RemoveButton(Button button)
    {
        buttons.Remove(button);
        button.StartedPress -= OnStartedPress;
        button.EndedPress -= OnEndedPress;
    }
    
    private void OnStartedPress(string buttonName)
    {
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.BUTTON_PUSHED.ToString() },
            { "button_event", "started" },
            { "button_name", buttonName }
        };
        _clientManager.SendDict(sentDict);
    }

    private void OnEndedPress(string buttonName)
    {
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.BUTTON_PUSHED.ToString() },
            { "button_event", "ended" },
            { "button_name", buttonName }
        };
        _clientManager.SendDict(sentDict);
    }
    
    
    private void OnStartedControlling(Vector2 movement)
    {
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.JOYSTICK_POSITION.ToString() },
            { "joystick_event", "started" },
            { "x", movement.x.ToString() },
            { "y", movement.y.ToString() }
        };

        _clientManager.SendDict(sentDict);
    }

    private void OnMovedControls(Vector2 movement)
    {
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.JOYSTICK_POSITION.ToString() },
            { "joystick_event", "moved" },
            { "x", movement.x.ToString() },
            { "y", movement.y.ToString() }
        };

        _clientManager.SendDict(sentDict);
    }

    private void OnStoppedControlling(Vector2 movement)
    {
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.JOYSTICK_POSITION.ToString() },
            { "joystick_event", "ended" },
            { "x", movement.x.ToString() },
            { "y", movement.y.ToString() }
        };

        _clientManager.SendDict(sentDict);
    }
}
