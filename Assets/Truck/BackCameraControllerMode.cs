using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackCameraControllerMode : ControllerMode
{
    public override float CalculateSteeringInput(TruckController vehicleController, InputAction.CallbackContext context)
    {
        return context.ReadValue<float>();
    }

    public override void OnModeChangedToThis()
    {
        
    }
}