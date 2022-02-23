using LowkeyFramework.AttributeSaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNpcCharacterData", menuName = "MeatTruck/Npc Character Data")]
public class NpcCharacterData : ReferenceableScriptableObject
{
    public NpcCharacterBehaviour behaviour;
}
