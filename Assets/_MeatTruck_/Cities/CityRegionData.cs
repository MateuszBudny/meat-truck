using LowkeyFramework.AttributeSaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCityRegion", menuName = "MeatTruck/City Region Data")]
public class CityRegionData : ReferenceableScriptableObject
{
    public CityRegion region;
}