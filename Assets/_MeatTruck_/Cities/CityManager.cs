using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityManager : SingleBehaviour<CityManager>
{
    [SerializeField]
    private List<CityRegionData> defaultRegionsData;

    public Dictionary<CityRegionData, CityRegion> CurrentRegions { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        // TODO: when SaveSystem is ready, then this should be invoked only on first game launch
        CurrentRegions = defaultRegionsData.ToDictionary(regionData => regionData, regionData => regionData.region.Copy());
    }
}
