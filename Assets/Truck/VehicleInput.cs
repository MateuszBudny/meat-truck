using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleInput : MonoBehaviour
{
    public VehicleController VehicleController { get; protected set; }

    protected float RawAccelerateInput { get; set; }
    protected float RawNormalBrakeInput { get; set; }
    //public float HandBrakeInput { get; protected set; } // TODO
    protected Vector2 RawSteeringInput { get; set; }
    public bool ChangeTiltBlockerInput { get; set; }

    protected new Rigidbody rigidbody;

    private void Awake()
    {
        BaseAwake();
    }

    protected void BaseAwake()
    {
        VehicleController = GetComponent<VehicleController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public abstract float GetCurrentAcceleration();

    public abstract float GetCurrentBraking();

    public abstract float GetCurrentSteeringAngle();
}
