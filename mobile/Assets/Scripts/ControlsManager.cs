using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    /*
     * This class should connect to the connection class in order to pass data to main instance
     */

    public JoystickController joystick;
    public List<Button> buttons;

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
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        throw new NotImplementedException();
    }

    private void OnStartedPress(string buttonName)
    {
        
    }

    private void OnEndedPress(string buttonName)
    {
        
    }
    
    
    private void OnStartedControlling(Vector2 movement)
    {
        
    }

    private void OnMovedControls(Vector2 movement)
    {
        
    }

    private void OnStoppedControlling(Vector2 movement)
    {
        
    }
}
