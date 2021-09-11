using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcCharacterState : CharacterState
{
    protected NpcCharacter NpcCharacter => character as NpcCharacter;

    public NpcCharacterState(NpcCharacter npcCharacter)
    {
        character = npcCharacter;
    }
}
