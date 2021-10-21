using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Meat
{
    [SerializeField]
    private MeatData data;
    public float CurrentFreshness { get; set; }

    public MeatData Data { get => data; set => data = value; }

    public Meat(MeatData data)
    {
        this.Data = data;
        CurrentFreshness = data.meatFreshness;
    }
}
