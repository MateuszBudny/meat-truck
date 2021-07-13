using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringPlayerVehicleState : DrivingBackwardPlayerVehicleState
{
    public GatheringPlayerVehicleState(PlayerVehicle vehicle) : base(vehicle) { }

    public override bool ChangeState(VehicleState newState)
    {
        return true;
    }
}
