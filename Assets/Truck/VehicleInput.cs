using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleInput : MonoBehaviour
{
    public float AccelerateInput { get; protected set; }
    public float NormalBrakeInput { get; protected set; }
    //public float HandBrakeInput { get; protected set; } // TODO
    public Vector2 RawSteeringInput { get; protected set; }
    public bool ChangeTiltBlockerInput { get; set; }
}
