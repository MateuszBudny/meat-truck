using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityManager : SingleBehaviour<CityManager>
{
    [SerializeField]
    private List<CityRegionData> defaultRegionsData;
    [Header("Inspector display purpose ONLY")]
    [SerializeField]
    private List<CityRegionDataWithInstanceValues> currentRegionsListInspectorDisplayPurposeOnly = new List<CityRegionDataWithInstanceValues>();


    private Dictionary<CityRegionData, CityRegion> currentRegions;
    public Dictionary<CityRegionData, CityRegion> CurrentRegions
    { 
        get => currentRegions;
        private set
        {
            currentRegions = value;
            currentRegionsListInspectorDisplayPurposeOnly = currentRegions.Select(keyValue => new CityRegionDataWithInstanceValues(keyValue.Key, keyValue.Value)).ToList();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        // TODO: when SaveSystem is ready, then this should be invoked only on first game launch
        CurrentRegions = defaultRegionsData.ToDictionary(regionData => regionData, regionData => regionData.region.DeepCopy());
    }

    [Serializable]
    private class CityRegionDataWithInstanceValues
    {
        [SerializeField]
        private CityRegionData cityRegionData;
        [SerializeField]
        private CityRegion cityRegionInstance;

        public CityRegionDataWithInstanceValues(CityRegionData cityRegionData, CityRegion cityRegionInstance)
        {
            this.cityRegionData = cityRegionData;
            this.cityRegionInstance = cityRegionInstance;
        }
    }
}
