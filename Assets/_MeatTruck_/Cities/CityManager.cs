using LowkeyFramework.AttributeSaveSystem;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityManager : SingleBehaviour<CityManager>
{
    [SerializeField]
    private List<CityRegionData> defaultRegionsData;
    public float meatPopularityDropOnBuy = 0.5f;
    public float minMeatPopularityAfterDrop = 0.25f;
    [Tooltip("For every second.")]
    public float meatPopularityRestorationSpeed = 0.01f;

    [Header("Runtime usage ONLY")]
    [SerializeField]
    private List<CityRegionDataWithInstanceValues> currentRegionsListForRuntimeInspector = new List<CityRegionDataWithInstanceValues>();

    [SaveField]
    private Dictionary<CityRegionData, CityRegion> currentRegions;
    public Dictionary<CityRegionData, CityRegion> CurrentRegions
    {
        get
        {
            LoadSavedStateAndInitRegions();   
            return currentRegions;
        }
        private set
        {
            currentRegions = value;
        }
    }

    private void Start()
    {
        LoadSavedStateAndInitRegions();

        StartCoroutine(MeatsPopularityRestorationEnumerator());
    }

    private void LoadSavedStateAndInitRegions()
    {
        if (currentRegions == null)
        {
            SaveManager.Instance.Load();
            if (currentRegions == null)
            {
                // this happens on the first ever game launch only
                CurrentRegions = defaultRegionsData.ToDictionary(regionData => regionData, regionData => regionData.region.DeepCopy());
            }
        }
    }

    private IEnumerator MeatsPopularityRestorationEnumerator()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1f);
            CurrentRegions.ToList().ForEach(regionPair => regionPair.Value.RestorePartOfMeatsPopularity());
#if UNITY_EDITOR
            currentRegionsListForRuntimeInspector = currentRegions.Select(keyValue => new CityRegionDataWithInstanceValues(keyValue.Key, keyValue.Value)).ToList();
#endif
        }
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
