using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CityRegion
{
    public WeightedList<MeatPopularity> meatsPopularity;

    public CityRegion(WeightedList<MeatPopularity> meatsPopularity)
    {
        this.meatsPopularity = meatsPopularity;
    }

    public void RestorePartOfMeatsPopularity() => meatsPopularity.List.ForEach(meatPopularity => meatPopularity.RestorePartOfPopularity());

    public CityRegion DeepCopy() => new CityRegion(new WeightedList<MeatPopularity>(meatsPopularity.List.Select(meatPopularity => meatPopularity.Copy()).ToList()));
}
