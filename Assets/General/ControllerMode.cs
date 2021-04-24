using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;

public abstract class ControllerMode
{
    public CinemachineVirtualCamera VirtualCamera { get; set; }

    public ControllerMode(CinemachineVirtualCamera virtualCamera)
    {
        VirtualCamera = virtualCamera;
    }

    public abstract float CalculateSteeringAngle(VehicleController vehicleController, Vector2 rawSteeringInput);

    public virtual void OnModeChangedToThis()
    {
        GameManager.Instance.ControllerModes.ToList().ForEach(controllerMode => controllerMode.VirtualCamera.Priority = 0);
        VirtualCamera.Priority++;
    }
}
