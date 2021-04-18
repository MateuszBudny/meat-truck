using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SideCameraControllerMode : ControllerMode
{
    public SideCameraControllerMode(CinemachineVirtualCamera virtualCamera) : base(virtualCamera) {}

    public override float CalculateSteeringInput(TruckController vehicleController, InputAction.CallbackContext context)
    {

        return context.ReadValue<Vector2>().x;
    }
}