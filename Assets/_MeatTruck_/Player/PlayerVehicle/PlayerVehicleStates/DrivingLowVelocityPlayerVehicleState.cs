using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class DrivingLowVelocityPlayerVehicleState : DrivingBackwardPlayerVehicleState
{
    public DrivingLowVelocityPlayerVehicleState(PlayerVehicle playerVehicle) : base(playerVehicle) {}

    public override void OnStateEnter(VehicleState previousState)
    {
        if(PlayerVehicle.Gathering.IsTryingToLookForNPCToGather)
        {
            StartLookingForNpcToGatherForReal();
        }
    }

    public override void OnStateExit(VehicleState nextState)
    {
        if(PlayerVehicle.Gathering.IsTryingToLookForNPCToGather)
        {
            StopLookingForNpcToGatherForReal();
        }
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
            PlayerVehicle.Gathering.IsTryingToLookForNPCToGather = true;
            StartLookingForNpcToGatherForReal();
        }
        else if (context.canceled)
        {
            PlayerVehicle.Gathering.IsTryingToLookForNPCToGather = false;
            StopLookingForNpcToGatherForReal();
        }
    }

    public override void OnOpenShop(CallbackContext context)
    {
        if(context.started)
        {
            PlayerVehicle.ChangeState(new MeatShopPlayerVehicleState(PlayerVehicle));
        }
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.NpcHumanToGather.ToString()) && PlayerVehicle.Gathering.gatheringTrigger.activeSelf)
        {
            NpcCharacterBehaviour npcCharacterCollided = collider.transform.parent.transform.parent.GetComponent<NpcCharacterBehaviour>();
            if (npcCharacterCollided.IsGatherable)
            {
                PlayerVehicle.ChangeState(new GatheringPlayerVehicleState(PlayerVehicle, npcCharacterCollided));
            }
        }
    }

    private void StartLookingForNpcToGatherForReal()
    {
        PlayerVehicle.PlayerVehicleEffects.Play(PlayerVehicleEffect.GatheringRangeCorrectSpeed);
        PlayerVehicle.Gathering.gatheringTrigger.SetActive(true);
    }

    private void StopLookingForNpcToGatherForReal()
    {
        PlayerVehicle.PlayerVehicleEffects.Stop(PlayerVehicleEffect.GatheringRangeCorrectSpeed);
        PlayerVehicle.Gathering.gatheringTrigger.SetActive(false);
    }
}
