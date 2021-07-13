using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardLowVelocityPlayerVehicleState : DrivingBackwardPlayerVehicleState
{
    public ForwardLowVelocityPlayerVehicleState(PlayerVehicle vehicle) : base(vehicle) { }
    public override bool ChangeState(VehicleState newState)
    {
        return true;
    }
}
