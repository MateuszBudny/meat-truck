using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Meat
{
    public MeatData data;
    [HideInInspector]
    public float currentFreshness;

    public Meat(MeatData data)
    {
        this.data = data;
    }
}
