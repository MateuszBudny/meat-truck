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

    private float maxPopularity;

    public MeatPopularity(MeatData meatData, float popularity, bool thisOrNothing)
    {
        this.meatData = meatData;
        this.popularity = popularity;
        maxPopularity = popularity;
        this.thisOrNothing = thisOrNothing;
    }

    public float Weight => popularity;

    public void DecreasePopularityAfterBuy() => popularity = Mathf.Max(popularity - CityManager.Instance.meatPopularityDropOnBuy, CityManager.Instance.minMeatPopularityAfterDrop);

    public int GetMeatFinalValue() => Mathf.CeilToInt(meatData.meatValue * popularity);

    public void RestorePartOfPopularity()
    {
        if(popularity < maxPopularity)
        {
            popularity = Mathf.Min(maxPopularity, popularity + CityManager.Instance.meatPopularityRestorationSpeed);

        }
    }

    public MeatPopularity Copy() => new MeatPopularity(meatData, popularity, thisOrNothing);
}
