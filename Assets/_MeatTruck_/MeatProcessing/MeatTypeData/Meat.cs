using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
[Serializable]
public class Meat
{
    [JsonProperty]
    [SerializeField]
    private MeatData data;
    [JsonProperty]
    private float currentFreshness;

    public MeatData Data { get => data; set => data = value; }
    public float CurrentFreshness { get => currentFreshness; set => currentFreshness = value; }

    public Meat(MeatData data)
    {
        Data = data;
        CurrentFreshness = data.meatFreshness;
    }
}
