using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class GatheringPlayerVehicleState : DrivingLowVelocityPlayerVehicleState
{
    private NpcCharacterBehaviour npcCharacterBeingGathered;
    private Coroutine gatheringCoroutine;

    public GatheringPlayerVehicleState(PlayerVehicle playerVehicle, NpcCharacterBehaviour npcCharacterBeingGathered) : base(playerVehicle) 
    {
        this.npcCharacterBeingGathered = npcCharacterBeingGathered;
    }

    public override void OnStateEnter(VehicleState previousState)
    {
        StartGathering();
    }

    public override void OnStateExit(VehicleState nextState)
    {
        ForceStopGathering();
    }

    public override void OnGathering(CallbackContext context)
    {
        if(context.canceled)
        {
            PlayerVehicle.ChangeState(new DrivingLowVelocityPlayerVehicleState(PlayerVehicle));
        }
    }

    private void StartGathering()
    {
        gatheringCoroutine = PlayerVehicle.StartCoroutine(StartGatheringEnumerator());
    }

    private IEnumerator StartGatheringEnumerator()
    {
        PlayerVehicle.PlayerVehicleEffects.Play(PlayerVehicleEffect.GatheringSmoke);
        yield return new WaitForSecondsRealtime(PlayerVehicle.Gathering.characterGatheringDuration);

        GatheringFinished();
    }

    private void GatheringFinished()
    {
        GameManager.Instance.Player.Inventory.Corpses.Add(npcCharacterBeingGathered.npcCharacter);
        npcCharacterBeingGathered.CollidersHandler.CollidersSetEnabled(false);
        npcCharacterBeingGathered.mainRigidbody.transform.DOJump(PlayerVehicle.VehicleController.CenterOfMass.transform.position, 3f, 1, 1f)
            .OnComplete(() => 
            {
                Object.Destroy(npcCharacterBeingGathered.gameObject);
                PlayerVehicle.PlayerVehicleEffects.Play(PlayerVehicleEffect.GatheringFinishedSuccessfully);
            })
            .Play();

        PlayerVehicle.ChangeState(new DrivingLowVelocityPlayerVehicleState(PlayerVehicle));
    }

    private void ForceStopGathering()
    {
        PlayerVehicle.PlayerVehicleEffects.Stop(PlayerVehicleEffect.GatheringSmoke, ParticleSystemStopBehavior.StopEmitting);
        PlayerVehicle.StopCoroutine(gatheringCoroutine);
        PlayerVehicle.Gathering.IsTryingToLookForNPCToGather = false;
    }
}
