using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeatPopularity : IWeightedElement
{
    public MeatData meatData;
    [Range(0f, 4f)]
    public float popularity = 1f;
    [Tooltip("If true and this meat is chosen as a one wanted by a customer, but MeatShop has no such meat, then no meat is bought. Otherwise, other meat is chosen.")]
    public bool thisOrNothing = false;

    public float Weight => popularity;
}
