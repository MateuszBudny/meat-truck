using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class VehiclePlayerInput : VehicleInput
{
    [SerializeField]
    private float goingForwardVelocityThreshold = 0.05f;

    private bool IsVehicleGoingForward => transform.InverseTransformDirection(rigidbody.velocity).z > goingForwardVelocityThreshold;

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

    public void OnGathering(CallbackContext context)
    {
        if(context.started)
        {
            // action on gathering. imo npcs who are in range need to be constantly added or removed from list of npcs in range
            // maybe this whole action shouldn't be in VehiclePlayerInput, but in some new PlayerVehicle class?
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
