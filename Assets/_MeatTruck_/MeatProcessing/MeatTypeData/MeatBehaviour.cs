using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollidersHandler))]
public class MeatBehaviour : MonoBehaviour
{
    public Meat meat;

    public CollidersHandler CollidersHandler { get; private set; }

    private void Awake()
    {
        CollidersHandler = GetComponent<CollidersHandler>();
    }
}