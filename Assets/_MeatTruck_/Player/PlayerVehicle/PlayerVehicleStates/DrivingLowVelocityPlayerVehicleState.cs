using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class DrivingLowVelocityPlayerVehicleState : DrivingBackwardPlayerVehicleState
{
    public DrivingLowVelocityPlayerVehicleState(PlayerVehicle playerVehicle) : base(playerVehicle) {}

    public override void OnStateExit(VehicleState nextState)
    {
        StopLookingForNpcToGather();
    }

    public override void Update()
    {
        if (GetCurrentAcceleration() > PlayerVehicle.VehicleController.VehicleEffects.tiresMinAccelerationToTriggerSmoke)
        {
            PlayerVehicle.VehicleController.VehicleEffects.PlayForLimitedTime(VehicleEffect.TiresSmoke, PlayerVehicle.VehicleController.VehicleEffects.tiresSmokeDuration, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            PlayerVehicle.VehicleController.VehicleEffects.Stop(VehicleEffect.TiresSmoke, ParticleSystemStopBehavior.StopEmitting);
        }

        if (PlayerVehicle.IsDeliberatelyGoingForward)
        {
            PlayerVehicle.ChangeState(new DrivingForwardPlayerVehicleState(PlayerVehicle));
        }
        if (PlayerVehicle.IsDeliberatelyGoingBackward)
        {
            PlayerVehicle.ChangeState(new DrivingBackwardPlayerVehicleState(PlayerVehicle));
        }
    }

    public override void OnGathering(CallbackContext context)
    {
        if (context.started)
        {
            StartLookingForNpcToGather();
        }

        if (context.canceled)
        {
            StopLookingForNpcToGather();
        }
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.NpcHumanToGather.ToString()) && PlayerVehicle.Gathering.gatheringTrigger.activeSelf)
        {
            NpcCharacterBehaviour npcCharacterCollided = collider.transform.parent.transform.parent.GetComponent<NpcCharacterBehaviour>();
            if (npcCharacterCollided.IsGatherable)
            {
                // TODO:
                // add particle effects
                // add progress bar
                // and eventually add character to inventory
                PlayerVehicle.ChangeState(new GatheringPlayerVehicleState(PlayerVehicle, npcCharacterCollided));
            }
        }
    }

    private void StartLookingForNpcToGather()
    {
        PlayerVehicle.Gathering.gatheringTrigger.SetActive(true);
    }

    private void StopLookingForNpcToGather()
    {
        PlayerVehicle.Gathering.gatheringTrigger.SetActive(false);
    }
}
