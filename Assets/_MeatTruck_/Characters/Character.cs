using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    public CharacterBehaviour prefab;

    public Character(CharacterBehaviour prefab)
    {
        this.prefab = prefab;
    }
}
