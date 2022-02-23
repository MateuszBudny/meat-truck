using LowkeyFramework.AttributeSaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeat", menuName = "MeatTruck/Meat Data")]
public class MeatData : ReferenceableScriptableObject
{
    public string meatName;
    public string meatDescription;
    public float meatValue;
    public int meatFreshness;
    public MeatBehaviour meatPrefabBehaviour;
}
