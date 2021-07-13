using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingForwardPlayerVehicleState : PlayerVehicleState
{
    public DrivingForwardPlayerVehicleState(PlayerVehicle vehicle) : base(vehicle) {}

    public override bool ChangeState(VehicleState newState)
    {
        switch(newState)
        {
            case ForwardLowVelocityPlayerVehicleState _:
                return true;
            default:
                return false;
        }
    }

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
