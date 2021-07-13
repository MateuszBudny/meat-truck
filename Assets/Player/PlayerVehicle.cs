using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(VehicleController))]
public class PlayerVehicle : Vehicle
{
    [SerializeField]
    private float goingForwardVelocityThreshold = 0.05f;
    [SerializeField]
    private GameObject gatheringTrigger;

    public PlayerVehicleState State { get => vehicleGenericState as PlayerVehicleState; private set => vehicleGenericState = value; }

    private float RawAccelerateInput { get; set; }
    private float RawNormalBrakeInput { get; set; }
    //public float HandBrakeInput { get; protected set; } // TODO
    private Vector2 RawSteeringInput { get; set; }

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
            ChangeTiltBlockerInput();
        }
    }

    // maybe this whole action shouldn't be in VehiclePlayerInput, but in some new PlayerVehicle class?
    // yea, refactor this into VehiclePlayerInput (or PlayerVehicleInput), PlayerVehicle and VehicleController. for npc it will be NpcVehicle and VehicleController.
    public void OnGathering(CallbackContext context)
    {
        if(context.started)
        {
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
