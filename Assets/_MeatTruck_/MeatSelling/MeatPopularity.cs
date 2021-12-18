using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeatPopularity : IWeightedElement
{
    public static float popularityDropOnBuy = 0.5f;
    public static float minPopularityAfterDrop = 0.25f;

    public MeatData meatData;
    [Range(0f, 4f)]
    public float popularity = 1f;
    [Tooltip("If true and this meat is chosen as a one wanted by a customer, but MeatShop has no such meat, then no meat is bought. Otherwise, other meat is chosen.")]
    public bool thisOrNothing = false;

    public MeatPopularity(MeatData meatData, float popularity, bool thisOrNothing)
    {
        this.meatData = meatData;
        this.popularity = popularity;
        this.thisOrNothing = thisOrNothing;
    }

    public float Weight => popularity;

    public void DecreasePopularityAfterBuy() => popularity = Mathf.Max(popularity - popularityDropOnBuy, minPopularityAfterDrop);

    public int GetMeatFinalValue() => Mathf.CeilToInt(meatData.meatValue * popularity);

    public MeatPopularity Copy() => new MeatPopularity(meatData, popularity, thisOrNothing);
}
