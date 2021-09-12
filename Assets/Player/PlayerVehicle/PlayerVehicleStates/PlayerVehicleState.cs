using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerVehicleState : VehicleState
{
    protected PlayerVehicle PlayerVehicle => vehicle as PlayerVehicle;

    public PlayerVehicleState(PlayerVehicle playerVehicle)
    {
        vehicle = playerVehicle;
    }
}
