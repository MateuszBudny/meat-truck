using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SideCameraControllerMode : ControllerMode
{
    public SideCameraControllerMode(CinemachineVirtualCamera virtualCamera) : base(virtualCamera) {}

    public override float CalculateSteeringAngle(VehicleController vehicleController, Vector2 rawSteeringInput)
    {
        if(rawSteeringInput == Vector2.zero)
        {
            return 0f;
        } else
        {
            float vehicleAngleRelativeToCamera = vehicleController.transform.rotation.eulerAngles.y - (VirtualCamera.transform.rotation.eulerAngles.y + 180f); // +180 degrees, because when the camera is facing the vehicle, it has -180 degrees of Z rotation.
            float rawSteeringAngle = Vector2.SignedAngle(rawSteeringInput, Vector2.up);
            float steeringAngleRelativeToVehicleAndCamera = rawSteeringAngle - vehicleAngleRelativeToCamera;
            float steeringAngleBetweenMinus180And180 = MathUtils.RecalculateAngleToBetweenMinus180And180(steeringAngleRelativeToVehicleAndCamera);
            float clampedSteeringAngle = Mathf.Clamp(steeringAngleBetweenMinus180And180, -vehicleController.maxSteerAngle, vehicleController.maxSteerAngle);

            // next, clampedSteeringAngle is multiplied by inputStrength, where inputStrength has a meaning of "how strong the analog is pushed by the player". 
            // so when the player steers the analog in a opposite direction of the vehicle direction, but does it lightly by slightly moving the analog, then the vehicle will not turn rapidly, but also slightly towards the direction of the player's choice.
            float inputStrength = Mathf.InverseLerp(0, 2, rawSteeringInput.sqrMagnitude); // 0 is a minimum possible sqr magnitude of input vector2 (0,0) and 2 is a maximum possible sqr magnitude of input vector2 (e.g. 1,1 or -1,1, etc).
            float steeringAngleDependingOnRawInputStrength = clampedSteeringAngle * inputStrength;
            return steeringAngleDependingOnRawInputStrength;
        }
    }
}
