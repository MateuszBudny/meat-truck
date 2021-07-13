using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingBackwardPlayerVehicleState : DrivingForwardPlayerVehicleState
{
    public DrivingBackwardPlayerVehicleState(PlayerVehicle vehicle) : base(vehicle) { }

    public override float GetCurrentAcceleration()
    {
        return PlayerVehicle.RawAccelerateInput - PlayerVehicle.RawNormalBrakeInput;
    }

    public override float GetCurrentBraking()
    {
        return 0f;
    }

    public override bool ChangeState(VehicleState newState)
    {
        switch (newState)
        {
            case ForwardLowVelocityPlayerVehicleState _:
                return true;
            default:
                return false;
        }
    }
}
