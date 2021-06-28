using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

// most important shortkeys
// tier0:
// alt+enter: tip
// ctrl+t: searching by fields/classes names
// right click + znajdŸ wszystkie odwo³ania: znajduje wszystkie odwo³ania xd
// ctrl+left click on field: jump into field declaration

// tier1:
// ctrl+l: copy and delete line 
// alt+arrow up/down: selected line up/down

[RequireComponent(typeof(VehicleInput))]
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

    [Header("Wheel Colliders")]
    [SerializeField] 
    private WheelCollider frontLeftWheelCollider;
    [SerializeField] 
    private WheelCollider frontRightWheelCollider;
    [SerializeField] 
    private WheelCollider rearLeftWheelCollider;
    [SerializeField] 
    private WheelCollider rearRightWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField] 
    private Transform frontLeftWheelTransform;
    [SerializeField] 
    private Transform frontRightWheeTransform;
    [SerializeField] 
    private Transform rearLeftWheelTransform;
    [SerializeField] 
    private Transform rearRightWheelTransform;

    public bool IsGrounded => frontLeftWheelCollider.isGrounded || frontRightWheelCollider.isGrounded || rearLeftWheelCollider.isGrounded || rearRightWheelCollider.isGrounded;

    private VehicleInput vehicleInput;
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
        vehicleInput = GetComponent<VehicleInput>();
    }

    private void Start()
    {
        GameplayManager.Instance.Player = this;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        UpdateWheels();
        HandleChangeTiltBlockerInput();
        
        if(CurrentTiltBlocker)
        {
            CurrentTiltBlocker.ControlTilt();
        }
    }


    private void HandleMovement()
    {
        ApplyAcceleration(vehicleInput.GetCurrentAcceleration());
        ApplyBraking(vehicleInput.GetCurrentBraking());
    }

    private void ApplyAcceleration(float acceleration)
    {
        acceleration = Mathf.Clamp(acceleration, -1f, 1f);
        float currentMotorForce = acceleration > 0 ? maxForwardMotorForce : maxBackwardMotorForce;
        float currentAccelerationForce = acceleration * currentMotorForce;
        frontLeftWheelCollider.motorTorque = currentAccelerationForce;
        frontRightWheelCollider.motorTorque = currentAccelerationForce;
    }

    private void ApplyBraking(float braking)
    {
        braking = Mathf.Clamp(braking, 0f, 1f);
        float currentbrakeForce = braking * maxBrakeForce;
        frontRightWheelCollider.brakeTorque = currentbrakeForce;
        frontLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearRightWheelCollider.brakeTorque = currentbrakeForce;
    }

    private void HandleSteering()
    {
        float currentSteerAngle = vehicleInput.GetCurrentSteeringAngle();
        currentSteerAngle = Mathf.Clamp(currentSteerAngle, -maxSteerAngle, maxSteerAngle);
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void HandleChangeTiltBlockerInput()
    {
        if (vehicleInput.ChangeTiltBlockerInput)
        {
            vehicleInput.ChangeTiltBlockerInput = false;
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
    }
}