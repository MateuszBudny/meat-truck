using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcCharacter : Character
{
    public NpcCharacterBehaviour NpcPrefab => prefab as NpcCharacterBehaviour;

    public NpcCharacter(CharacterBehaviour prefab) : base(prefab) { }

    public NpcCharacter Copy()
    {
        return new NpcCharacter(NpcPrefab);
    }
}
