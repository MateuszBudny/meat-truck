using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TiltBlocker : MonoBehaviour
{
    protected VehicleController vehicleController;

    protected void Awake()
    {
        vehicleController = GetComponent<VehicleController>();
    }

    public virtual void OnTiltBlockerEnable() {}

    public virtual void OnTiltBlockerDisable() {}

    public virtual void ControlTilt() {}
}
