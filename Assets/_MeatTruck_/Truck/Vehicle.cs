using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleController))]
public abstract class Vehicle : MonoBehaviour
{
    public VehicleController VehicleController { get; protected set; }

    protected VehicleState vehicleGenericState;
    protected new Rigidbody rigidbody;

    protected virtual void Awake()
    {
        VehicleController = GetComponent<VehicleController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public abstract float GetCurrentAcceleration();

    public abstract float GetCurrentBraking();

    public abstract float GetCurrentSteeringAngle();

    protected virtual void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
    }

    protected virtual void HandleMovement()
    {
        ApplyCurrentAcceleration();
        ApplyCurrentBraking();
    }

    protected virtual void HandleSteering()
    {
        VehicleController.HandleSteering(GetCurrentSteeringAngle());
    }

    protected virtual void ChangeTiltBlockerInput()
    {
        VehicleController.HandleChangeTiltBlockerInput();
    }

    private void ApplyCurrentAcceleration() => VehicleController.ApplyAcceleration(GetCurrentAcceleration());

    private void ApplyCurrentBraking() => VehicleController.ApplyBraking(GetCurrentBraking());
}
