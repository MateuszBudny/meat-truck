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

    public float RawAccelerateInput { get; set; }
    public float RawNormalBrakeInput { get; set; }
    //public float HandBrakeInput { get; protected set; } // TODO
    public Vector2 RawSteeringInput { get; set; }

    private bool IsVehicleGoingForward => transform.InverseTransformDirection(rigidbody.velocity).z > goingForwardVelocityThreshold;

    protected override void Awake()
    {
        base.Awake();
        State = new ForwardLowVelocityPlayerVehicleState(this);
    }

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

    /// <summary>
    /// Use this to change state. (do not use state's inner ChangeState() method directly!)
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(PlayerVehicleState newState)
    {
        if (State.ChangeState(newState))
        {
            State = newState;
        }
    }
}
