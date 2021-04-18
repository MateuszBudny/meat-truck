using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class ControllerMode
{
    public CinemachineVirtualCamera VirtualCamera { get; set; }

    public ControllerMode(CinemachineVirtualCamera virtualCamera)
    {
        VirtualCamera = virtualCamera;
    }

    public abstract float CalculateSteeringInput(TruckController vehicleController, CallbackContext context);

    public virtual void OnModeChangedToThis()
    {
        GameManager.Instance.ControllerModes.ForEach(controllerMode => controllerMode.VirtualCamera.Priority = 0);
        VirtualCamera.Priority++;
    }
}
