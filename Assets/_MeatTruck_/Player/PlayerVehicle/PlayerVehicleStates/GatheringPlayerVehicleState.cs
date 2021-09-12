using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class GatheringPlayerVehicleState : DrivingLowVelocityPlayerVehicleState
{
    private NpcCharacter npcCharacterBeingGathered;

    public GatheringPlayerVehicleState(PlayerVehicle playerVehicle, NpcCharacter npcCharacterBeingGathered) : base(playerVehicle) 
    {
        this.npcCharacterBeingGathered = npcCharacterBeingGathered;
    }

    public override void OnStateEnter(VehicleState previousState)
    {
        OnStartGathering();
    }

    public override void OnStateExit(VehicleState nextState)
    {
        OnStopGathering();
    }

    public override void OnGathering(CallbackContext context)
    {
        if(context.canceled)
        {
            PlayerVehicle.ChangeState(new DrivingLowVelocityPlayerVehicleState(PlayerVehicle));
        }
    }

    private void OnStartGathering()
    {
        Object.Destroy(npcCharacterBeingGathered.gameObject);

    }

    private void OnStopGathering()
    {

    }
}
