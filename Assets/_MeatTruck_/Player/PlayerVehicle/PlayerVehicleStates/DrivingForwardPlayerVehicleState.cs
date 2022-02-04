using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingForwardPlayerVehicleState : PlayerVehicleState
{
    public DrivingForwardPlayerVehicleState(PlayerVehicle playerVehicle) : base(playerVehicle) { }

    public override void OnStateEnter(VehicleState previousState)
    {
        if(PlayerVehicle.Gathering.IsTryingToLookForNPCToGather)
        {
            PlayerVehicle.PlayerVehicleEffects.Play(PlayerVehicleEffect.GatheringRangeTooFast);
        }
    }

    public override void OnStateExit(VehicleState nextState)
    {
        if (PlayerVehicle.Gathering.IsTryingToLookForNPCToGather)
        {
            PlayerVehicle.PlayerVehicleEffects.Stop(PlayerVehicleEffect.GatheringRangeTooFast);
        }
    }

    public override void Update()
    {
        if (GetCurrentAcceleration() < PlayerVehicle.VehicleController.VehicleEffects.tiresMinAccelerationToTriggerSmoke)
        {
            PlayerVehicle.VehicleController.VehicleEffects.Stop(VehicleEffect.TiresSmoke, ParticleSystemStopBehavior.StopEmitting);
        }

        if (!PlayerVehicle.IsDeliberatelyGoingForward)
        {
            PlayerVehicle.ChangeState(new DrivingLowVelocityPlayerVehicleState(PlayerVehicle));
        }
    }

    public override float GetCurrentAcceleration()
    {
        return PlayerVehicle.RawAccelerateInput;
    }

    public override float GetCurrentBraking()
    {
        return PlayerVehicle.RawNormalBrakeInput;
    }

    public override float GetCurrentSteeringAngle()
    {
        return DrivingGameplayManager.Instance.CurrentControllerMode.CalculateSteeringAngle(PlayerVehicle.VehicleController, PlayerVehicle.RawSteeringInput);
    }

    public override void OnGathering(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            PlayerVehicle.Gathering.IsTryingToLookForNPCToGather = true;
            PlayerVehicle.PlayerVehicleEffects.Play(PlayerVehicleEffect.GatheringRangeTooFast);
        }

        else if (context.canceled)
        {
            PlayerVehicle.Gathering.IsTryingToLookForNPCToGather = false;
            PlayerVehicle.PlayerVehicleEffects.Stop(PlayerVehicleEffect.GatheringRangeTooFast);
        }
    }
}
