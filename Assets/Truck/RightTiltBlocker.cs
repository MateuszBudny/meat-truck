using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTiltBlocker : TiltBlocker
{
    [SerializeField]
    private float rightForce;
    [SerializeField]
    private Transform rightRayPosition;
    [SerializeField]
    private float offsetSize;

    public override void ControlTilt()
    {
        if (vehicleController.IsGrounded)
        {
            Vector3 smoothGroundNormal = new Vector3();
            bool rayHit = false;

            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    RaycastHit rh;
                    Vector3 rayOffset = (i * transform.right + j * transform.forward) * offsetSize;

                    Ray findNormalRay = new Ray(rightRayPosition.position + rayOffset, Vector3.down);

                    Debug.DrawRay(findNormalRay.origin, findNormalRay.direction);

                    if (Physics.Raycast(findNormalRay, out rh))
                    {
                        rayHit = true;
                        smoothGroundNormal += rh.normal;
                    }
                }
            }

            if (rayHit)
            {
                Vector3 torqueDir = Vector3.Cross(transform.up, smoothGroundNormal.normalized);
                vehicleController.rigidbody.AddTorque(torqueDir * rightForce, ForceMode.Acceleration);
            }
        }
    }
}
