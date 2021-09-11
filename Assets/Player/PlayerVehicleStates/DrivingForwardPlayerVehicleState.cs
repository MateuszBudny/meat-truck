using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingForwardPlayerVehicleState : PlayerVehicleState
{
    public DrivingForwardPlayerVehicleState(PlayerVehicle playerVehicle) : base(playerVehicle) {}

    public override float GetCurrentAcceleration()
    {
        return PlayerVehicle.RawAccelerateInput;
    }

    public override float GetCurrentBraking()
    {
        return PlayerVehicle.RawNormalBrakeInput;
    }

    public override float GetCurrentSteeringAngle()
    {
        return GameplayManager.Instance.CurrentControllerMode.CalculateSteeringAngle(PlayerVehicle.VehicleController, PlayerVehicle.RawSteeringInput);
    }
}
