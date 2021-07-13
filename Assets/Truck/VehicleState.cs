using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleState
{
    protected Vehicle vehicle;

    /// <summary>
    /// Returns if the current state can be changed to the new state. Do not use directly!
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    public abstract bool ChangeState(VehicleState newState);

    public abstract float GetCurrentAcceleration();

    public abstract float GetCurrentBraking();

    public abstract float GetCurrentSteeringAngle();
}
