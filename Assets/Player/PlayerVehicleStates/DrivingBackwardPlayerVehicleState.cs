using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingBackwardPlayerVehicleState : DrivingForwardPlayerVehicleState
{
    public DrivingBackwardPlayerVehicleState(PlayerVehicle playerVehicle) : base(playerVehicle) { }

    public override float GetCurrentAcceleration()
    {
        return PlayerVehicle.RawAccelerateInput - PlayerVehicle.RawNormalBrakeInput;
    }

    public override float GetCurrentBraking()
    {
        return 0f;
    }
}
