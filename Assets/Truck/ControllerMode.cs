using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class ControllerMode
{
    public abstract float CalculateSteeringInput(TruckController vehicleController, CallbackContext context);

    public abstract void OnModeChangedToThis();
}
