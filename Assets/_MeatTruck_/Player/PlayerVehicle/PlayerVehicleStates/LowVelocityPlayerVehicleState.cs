using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowVelocityPlayerVehicleState : DrivingBackwardPlayerVehicleState
{
    public LowVelocityPlayerVehicleState(PlayerVehicle playerVehicle) : base(playerVehicle) { }

    public override void Update()
    {
        if(PlayerVehicle.IsDeliberatelyGoingForward)
        {
            PlayerVehicle.ChangeState(new DrivingForwardPlayerVehicleState(PlayerVehicle));
        }
        if(PlayerVehicle.IsDeliberatelyGoingBackward)
        {
            PlayerVehicle.ChangeState(new DrivingBackwardPlayerVehicleState(PlayerVehicle));
        }
    }
}
