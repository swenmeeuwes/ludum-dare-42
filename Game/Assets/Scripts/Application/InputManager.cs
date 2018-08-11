using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum InputType
{
    KeyboardAndMouse,
    Controller
}

public class InputManager : ITickable
{
    #region Injected

    private SignalBus _signalBus;
    private ControllerSettings _controllerSettings;

    #endregion

    private InputType _currentInputType = InputType.KeyboardAndMouse;
    public InputType CurrentInputType {
        get { return _currentInputType; }
        private set
        {
            _currentInputType = value;
            _signalBus.Fire(new InputTypeChangedSignal
            {
                Type = value
            });
        }
    }

    public InputManager(SignalBus signalBus, ControllerSettings controllerSettings)
    {
        _signalBus = signalBus;
        _controllerSettings = controllerSettings;
    }

    public void Tick()
    {
        if (CheckKeyboardAndMouse())
            CurrentInputType = InputType.KeyboardAndMouse;
        else if (CheckController())
            CurrentInputType = InputType.Controller;
    }

    /// <summary>
    /// Checks if a controller was used
    /// </summary>
    /// <returns>If a controller was used</returns>
    private bool CheckController()
    {
        // Unity y u no provide a 'Input.anyKey' for controllers?
        return Input.GetAxisRaw(InputAxes.Horizontal) > _controllerSettings.DeadZone ||
                Input.GetAxisRaw(InputAxes.Horizontal) < -_controllerSettings.DeadZone ||
                Input.GetAxisRaw(InputAxes.Vertical) > _controllerSettings.DeadZone ||
                Input.GetAxisRaw(InputAxes.Vertical) < -_controllerSettings.DeadZone ||
                Input.GetAxisRaw(InputAxes.HorizontalRight) > _controllerSettings.DeadZone ||
                Input.GetAxisRaw(InputAxes.HorizontalRight) < -_controllerSettings.DeadZone ||
                Input.GetAxisRaw(InputAxes.VerticalRight) > _controllerSettings.DeadZone ||
                Input.GetAxisRaw(InputAxes.VerticalRight) < -_controllerSettings.DeadZone;
    }

    private bool CheckKeyboardAndMouse()
    {
        //return Input.anyKey; // Triggers when some buttons on the controller are pressed
        return Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.S) ||
               Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.D) ||
               Input.GetKey(KeyCode.KeypadEnter) ||
               Input.GetKey(KeyCode.Space) ||
               Input.GetKey(KeyCode.UpArrow) ||
               Input.GetKey(KeyCode.DownArrow) ||
               Input.GetKey(KeyCode.LeftArrow) ||
               Input.GetKey(KeyCode.RightArrow) ||
               Input.GetMouseButton(0) ||
               Input.GetMouseButton(1) ||
               Input.GetMouseButton(2);
    }
}
