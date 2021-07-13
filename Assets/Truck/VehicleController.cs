using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// most important shortkeys
// tier0:
// alt+enter: tip
// ctrl+t: searching by fields/classes names
// right click + znajdz wszystkie odwolania: znajduje wszystkie odwolania xd
// ctrl+left click on field: jump into field declaration

// tier1:
// ctrl+l: copy and delete line 
// alt+arrow up/down: selected line up/down

[RequireComponent(typeof(Vehicle))]
public class VehicleController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField]
    private GameObject centerOfMass;
    [SerializeField]
    private float maxForwardMotorForce = 1000;
    [SerializeField]
    private float maxBackwardMotorForce = 400;
    [SerializeField] 
    private float maxBrakeForce = 3000;
    public float maxSteerAngle = 30;
    public new Rigidbody rigidbody;

    [Header("Wheels")]
    [SerializeField] 
    private Wheel frontLeftWheel;
    [SerializeField] 
    private Wheel frontRightWheel;
    [SerializeField] 
    private Wheel rearLeftWheel;
    [SerializeField] 
    private Wheel rearRightWheel;

    public bool IsGrounded => frontLeftWheel.IsGrounded || frontRightWheel.IsGrounded || rearLeftWheel.IsGrounded || rearRightWheel.IsGrounded;

    private Queue<TiltBlocker> tiltBlockers;
    private TiltBlocker CurrentTiltBlocker => tiltBlockers.Peek();

    private void Awake()
    {
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
        tiltBlockers = new Queue<TiltBlocker>(GetComponents<TiltBlocker>());
        if(CurrentTiltBlocker)
        {
            CurrentTiltBlocker.OnTiltBlockerEnable(false);
        }
    }

    private void Start()
    {
        GameplayManager.Instance.PlayerVehicleController = this;
    }

    private void FixedUpdate()
    {
        UpdateWheelsVisual();
        
        if(CurrentTiltBlocker)
        {
            CurrentTiltBlocker.ControlTilt();
        }
    }

    public void ApplyAcceleration(float acceleration)
    {
        acceleration = Mathf.Clamp(acceleration, -1f, 1f);
        float currentMotorForce = acceleration > 0 ? maxForwardMotorForce : maxBackwardMotorForce;
        float currentAccelerationForce = acceleration * currentMotorForce;
        frontLeftWheel.ApplyAcceleration(currentAccelerationForce);
        frontRightWheel.ApplyAcceleration(currentAccelerationForce);
    }

    public void ApplyBraking(float braking)
    {
        braking = Mathf.Clamp(braking, 0f, 1f);
        float currentbrakeForce = braking * maxBrakeForce;
        frontRightWheel.ApplyBraking(currentbrakeForce);
        frontLeftWheel.ApplyBraking(currentbrakeForce);
        rearLeftWheel.ApplyBraking(currentbrakeForce);
        rearRightWheel.ApplyBraking(currentbrakeForce);
    }

    public void HandleSteering(float steerAngle)
    {
        float clampedSteerAngle = Mathf.Clamp(steerAngle, -maxSteerAngle, maxSteerAngle);
        frontLeftWheel.ApplySteering(clampedSteerAngle);
        frontRightWheel.ApplySteering(clampedSteerAngle);
    }

    public void HandleChangeTiltBlockerInput()
    {
        if(CurrentTiltBlocker)
        {
            CurrentTiltBlocker.OnTiltBlockerDisable();
        }
        tiltBlockers.Enqueue(tiltBlockers.Dequeue());
        if(CurrentTiltBlocker)
        {
            CurrentTiltBlocker.OnTiltBlockerEnable();
        }
    }

    private void UpdateWheelsVisual()
    {
        frontLeftWheel.UpdateVisual();
        frontRightWheel.UpdateVisual();
        rearRightWheel.UpdateVisual();
        rearLeftWheel.UpdateVisual();
    }
}