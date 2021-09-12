using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class PlayerVehicleState : VehicleState
{
    protected PlayerVehicle PlayerVehicle => vehicle as PlayerVehicle;

    public PlayerVehicleState(PlayerVehicle playerVehicle)
    {
        vehicle = playerVehicle;
    }

    public virtual void OnGathering(CallbackContext context) {}
}
