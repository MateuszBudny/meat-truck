using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTiltBlocker : TiltBlocker
{
    [SerializeField]
    private AnimationCurve rightForceCurve;
    [SerializeField]
    private Transform rightRayPosition;
    [SerializeField]
    private float rightRayMaxDistance = 1f;
    [SerializeField]
    private float rightAreaSize = 0.5f;
    [SerializeField]
    private Layers rightToLayers = Layers.Ground;
    [Tooltip("Used when right ray didn't hit anything permitted.")]
    [SerializeField]
    private Vector3 defaultNormal = Vector3.up;
    [SerializeField]
    private float maxAngleToRight = 30f;
    [SerializeField]
    private MaxAngleExceededBehaviour maxAngleExceededBehaviour = MaxAngleExceededBehaviour.RightToMaxAngle;
    [SerializeField]
    private bool showDebugGizmo = false;

    private enum MaxAngleExceededBehaviour
    {
        RightToMaxAngle,
        RightToDefaultNormal,
    }

    public override void ControlTilt()
    {
        if (vehicleController.IsGrounded)
        {
            Vector3 smoothGroundNormal = new Vector3();

            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    RaycastHit rh;
                    Vector3 rayOffset = (i * transform.right + j * transform.forward) * rightAreaSize;

                    Ray findNormalRay = new Ray(rightRayPosition.position + rayOffset, -transform.up);

                    if (showDebugGizmo)
                    {
                        Debug.DrawRay(findNormalRay.origin, findNormalRay.direction * rightRayMaxDistance);
                    }

                    if (Physics.Raycast(findNormalRay, out rh, rightRayMaxDistance, (int)rightToLayers))
                    {
                        float normalAngle = Vector3.Angle(rh.normal, Vector3.up);
                        if (normalAngle <= maxAngleToRight)
                        {
                            smoothGroundNormal += rh.normal;
                        }
                        else
                        {
                            switch (maxAngleExceededBehaviour)
                            {
                                case MaxAngleExceededBehaviour.RightToMaxAngle:
                                    Vector3 normalWithMaxAngle = Quaternion.AngleAxis(maxAngleToRight, Vector3.Cross(Vector3.up, rh.normal)) * Vector3.up;
                                    smoothGroundNormal += normalWithMaxAngle;
                                    break;
                                case MaxAngleExceededBehaviour.RightToDefaultNormal:
                                    smoothGroundNormal += defaultNormal;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        smoothGroundNormal += defaultNormal;
                    }
                }
            }

            Vector3 torqueDir = Vector3.Cross(transform.up, smoothGroundNormal.normalized);
            float torqueAngle = Vector3.Angle(smoothGroundNormal.normalized, transform.up);
            vehicleController.rigidbody.AddTorque(torqueDir * rightForceCurve.Evaluate(torqueAngle), ForceMode.Acceleration);
        }
    }
}
