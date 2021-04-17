using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GameManager : SingleBehaviour<GameManager>
{
    public TruckController Player { get; set; }
    public ControllerMode CurrentControllerMode { get; set; } = new BackCameraControllerMode();

    public void OnChangeControllerModeInput(CallbackContext context)
    {
        if(CurrentControllerMode is SideCameraControllerMode)
        {
            CurrentControllerMode = new BackCameraControllerMode();
        } else
        {
            CurrentControllerMode = new SideCameraControllerMode();
        }

        CurrentControllerMode.OnModeChangedToThis();
    }
}
