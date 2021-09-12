using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleState
{
    protected Vehicle vehicle;

    public virtual void OnStateEnter(VehicleState previousState) {}

    public virtual void OnStateExit(VehicleState nextState) {}

    public virtual void Update() {}

    public abstract float GetCurrentAcceleration();

    public abstract float GetCurrentBraking();

    public abstract float GetCurrentSteeringAngle();
}
