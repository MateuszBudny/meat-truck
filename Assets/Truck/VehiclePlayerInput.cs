using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class VehiclePlayerInput : VehicleInput
{
    [SerializeField]
    private float goingForwardVelocityThreshold = 0.05f;
    [SerializeField]
    private GameObject gatheringTrigger;

    private bool IsVehicleGoingForward => transform.InverseTransformDirection(rigidbody.velocity).z > goingForwardVelocityThreshold;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag(Tags.NpcHumanToGather.ToString()) && gatheringTrigger.activeSelf)
        {
            NpcCharacter npcCharacterCollided = collider.transform.parent.transform.parent.GetComponent<NpcCharacter>();
            if(npcCharacterCollided.IsGatherable)
            {
                // check if vehicle is stopped
                // add progress bar
                // and eventually add character to inventory
                // oh, and don't forget: REFACTOR
                Destroy(npcCharacterCollided.gameObject);
            }
        }
    }

    public void OnAccelerateInput(CallbackContext context)
    {
        RawAccelerateInput = context.ReadValue<float>();
    }

    public void OnNormalBrakeInput(CallbackContext context)
    {
        RawNormalBrakeInput = context.ReadValue<float>();
    }

    public void OnSteeringInput(CallbackContext context)
    {
        RawSteeringInput = context.ReadValue<Vector2>();
    }

    public void OnChangeTiltBlockerInput(CallbackContext context)
    {
        if(context.started)
        {
            ChangeTiltBlockerInput = true;
        }
    }

    // maybe this whole action shouldn't be in VehiclePlayerInput, but in some new PlayerVehicle class?
    // yea, refactor this into VehiclePlayerInput (or PlayerVehicleInput), PlayerVehicle and VehicleController. for npc it will be NpcVehicle and VehicleController.
    public void OnGathering(CallbackContext context)
    {
        if(context.started)
        {
            // action on gathering. imo npcs who are in range need to be constantly added or removed from list of npcs in range
            // or better. just activate the trigger collider when it is needed, it should work, just like explosions (maybe continuous collision detection?).
            // if for some reason it won't work, then you can animate the trigger collider, so it will be quickly expanding from 0 radius to max. this way it should catch everything, that is needed.
            gatheringTrigger.SetActive(true);
        }

        if(context.canceled)
        {
            gatheringTrigger.SetActive(false);
        }
    }

    public override float GetCurrentAcceleration()
    {
        if (IsVehicleGoingForward)
        {
            return RawAccelerateInput;
        }
        else
        {
            return RawAccelerateInput - RawNormalBrakeInput;
        }
    }

    public override float GetCurrentBraking()
    {
        if (IsVehicleGoingForward)
        {
            return RawNormalBrakeInput;
        }
        else
        {
            return 0f;
        }
    }

    public override float GetCurrentSteeringAngle()
    {
        return GameplayManager.Instance.CurrentControllerMode.CalculateSteeringAngle(VehicleController, RawSteeringInput);
    }
}
