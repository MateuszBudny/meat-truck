using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class VehicleController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField]
    private float forwardMotorForce;
    [SerializeField]
    private float backwardMotorForce;
    [SerializeField] 
    private float brakeForce;
    public float maxSteerAngle;
    [SerializeField]
    private float accelerateBackwardInsteadOfBrakingVelocityThreshold = 0.2f;
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

    private Queue<TiltBlocker> tiltBlockers;
    private TiltBlocker CurrentTiltBlocker => tiltBlockers.Peek();

    private float accelerateInput;
    private float normalBrakeInput;
    private float handBrakeInput; // TODO
    private Vector2 rawSteeringInput;

    private void Awake()
    {
        tiltBlockers = new Queue<TiltBlocker>(GetComponents<TiltBlocker>());
        CurrentTiltBlocker.OnTiltBlockerEnable();
    }

    private void Start()
    {
        GameManager.Instance.Player = this;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        UpdateWheels();
        CurrentTiltBlocker.ControlTilt();
    }

    public void OnAccelerateInput(CallbackContext context)
    {
        accelerateInput = context.ReadValue<float>();
    }

    public void OnNormalBrakeInput(CallbackContext context)
    {
        normalBrakeInput = context.ReadValue<float>();
    }

    public void OnSteeringInput(CallbackContext context)
    {
        rawSteeringInput = context.ReadValue<Vector2>();
    }

    public void OnChangeTiltBlockerInput(CallbackContext context)
    {
        CurrentTiltBlocker.OnTiltBlockerDisable();
        tiltBlockers.Enqueue(tiltBlockers.Dequeue());
        CurrentTiltBlocker.OnTiltBlockerEnable();
    }

    private void HandleMovement()
    {
        float acceleration;
        float braking;

        // the truck is certainly moving forward
        if(rigidbody.velocity.x > accelerateBackwardInsteadOfBrakingVelocityThreshold)
        {
            acceleration = accelerateInput;
            braking = normalBrakeInput;
        }
        else // the truck is probably stopped or moving backward
        {
            acceleration = accelerateInput - normalBrakeInput;
            braking = 0f;
        }

        ApplyAcceleration(acceleration);
        ApplyBraking(braking);
    }

    private void ApplyAcceleration(float acceleration)
    {
        float currentMotorForce = acceleration > 0 ? forwardMotorForce : backwardMotorForce;
        float currentAccelerationForce = acceleration * currentMotorForce;
        frontLeftWheelCollider.motorTorque = -currentAccelerationForce;
        frontRightWheelCollider.motorTorque = -currentAccelerationForce;
    }

    private void ApplyBraking(float braking)
    {
        float currentbrakeForce = braking * brakeForce;
        frontRightWheelCollider.brakeTorque = currentbrakeForce;
        frontLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearRightWheelCollider.brakeTorque = currentbrakeForce;
    }

    private void HandleSteering()
    {
        float currentSteerAngle = GameManager.Instance.CurrentControllerMode.CalculateSteeringAngle(this, rawSteeringInput);
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
}