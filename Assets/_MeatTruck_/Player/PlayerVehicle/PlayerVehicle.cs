using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(VehicleController))]
public class PlayerVehicle : Vehicle
{
    [SerializeField]
    private float deliberateMovementVelocityThreshold = 0.05f;

    public float CurrentSpeed => transform.InverseTransformDirection(rigidbody.velocity).z;
    public bool IsDeliberatelyGoingForward => CurrentSpeed > deliberateMovementVelocityThreshold;
    public bool IsDeliberatelyGoingBackward => CurrentSpeed < -deliberateMovementVelocityThreshold;

    public PlayerVehicleState State { get => vehicleGenericState as PlayerVehicleState; private set => vehicleGenericState = value; }
    public CharactersGathering Gathering { get; private set; }
    public PlayerVehicleEffects PlayerVehicleEffects { get; private set; }
    public float RawAccelerateInput { get; set; }
    public float RawNormalBrakeInput { get; set; }
    //public float HandBrakeInput { get; protected set; } // TODO
    public Vector2 RawSteeringInput { get; set; }

    protected override void Awake()
    {
        base.Awake();
        State = new DrivingLowVelocityPlayerVehicleState(this);
        Gathering = GetComponent<CharactersGathering>();
        PlayerVehicleEffects = GetComponent<PlayerVehicleEffects>();
    }

    private void Update()
    {
        State.Update();
    }

    private void OnTriggerEnter(Collider collider)
    {
        State.OnTriggerEnter(collider);
    }

    public void ChangeState(PlayerVehicleState newState)
    {
        PlayerVehicleState previousState = State;
        State.OnStateExit(newState);
        State = newState;
        State.OnStateEnter(previousState);
    }

    public override float GetCurrentAcceleration()
    {
        return State.GetCurrentAcceleration();
    }

    public override float GetCurrentBraking()
    {
        return State.GetCurrentBraking();
    }

    public override float GetCurrentSteeringAngle()
    {
        return State.GetCurrentSteeringAngle();
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

    public void OnGathering(CallbackContext context)
    {
        State.OnGathering(context);
    }
}
