using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeat", menuName = "MeatTruck/Meat")]
public class MeatData : ScriptableObject
{
    public string meatName;
    public string meatDescription;
    public float meatValue;
    public int meatFreshness;
    public MeatBehaviour meatPrefabBehaviour;
}
