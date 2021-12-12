using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CityRegion
{
    public List<MeatPopularity> meatsPopularity;

    public CityRegion(List<MeatPopularity> meatsPopularity)
    {
        this.meatsPopularity = meatsPopularity;
    }

    public CityRegion Copy() => new CityRegion(new List<MeatPopularity>(meatsPopularity));
}
