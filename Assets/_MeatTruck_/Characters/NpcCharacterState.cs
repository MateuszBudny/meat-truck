using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcCharacterState : CharacterState
{
    protected NpcCharacterBehaviour NpcCharacter => characterBehaviour as NpcCharacterBehaviour;

    public NpcCharacterState(NpcCharacterBehaviour npcCharacter)
    {
        characterBehaviour = npcCharacter;
    }
}
