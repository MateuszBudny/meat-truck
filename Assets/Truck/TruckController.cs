using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TruckController : MonoBehaviour
{
    private float accelerateInput;
    private float normalBrakeInput;
    private float handBrakeInput;
    private float steeringInput;

    [Header("Controller Settings")]
    [SerializeField]
    private float forwardMotorForce;
    [SerializeField]
    private float backwardMotorForce;
    [SerializeField] 
    private float brakeForce;
    [SerializeField] 
    private float maxSteerAngle;
    [SerializeField]
    private float accelerateBackwardInsteadOfBrakingVelocityThreshold = 0.2f;
    [SerializeField]
    private Rigidbody rigibody;

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

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        UpdateWheels();
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
        steeringInput = context.ReadValue<float>();
    }

    private void HandleMovement()
    {
        float acceleration;
        float braking;

        // the truck is certainly moving forward
        if(rigibody.velocity.x > accelerateBackwardInsteadOfBrakingVelocityThreshold)
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
        float currentSteerAngle = maxSteerAngle * steeringInput;
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