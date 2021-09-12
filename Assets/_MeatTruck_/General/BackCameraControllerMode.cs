using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackCameraControllerMode : ControllerMode
{
    public BackCameraControllerMode(CinemachineVirtualCamera virtualCamera) : base(virtualCamera) {}

    public override float CalculateSteeringAngle(VehicleController vehicleController, Vector2 rawSteeringInput)
    {
        return rawSteeringInput.x * vehicleController.maxSteerAngle;
    }
}