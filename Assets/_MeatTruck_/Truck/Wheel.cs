using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField]
    private new WheelCollider collider;
    [SerializeField]
    private Transform visual;

    public bool IsGrounded => collider.isGrounded;

    public void UpdateVisual()
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        visual.SetPositionAndRotation(pos, rot);
    }

    public void ApplyAcceleration(float accelerationValue)
    {
        collider.motorTorque = accelerationValue;
    }

    public void ApplyBraking(float brakingValue)
    {
        collider.brakeTorque = brakingValue;
    }

    public void ApplySteering(float angle)
    {
        collider.steerAngle = angle;
    }
}
