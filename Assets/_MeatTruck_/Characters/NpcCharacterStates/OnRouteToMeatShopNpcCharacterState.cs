using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRouteToMeatShopNpcCharacterState : OnTemporaryRouteCharacterNpcState
{
    public OnRouteToMeatShopNpcCharacterState(NpcCharacterBehaviour npcCharacter, Waypoint startingWaypoint, bool returnOnRoute) : base(npcCharacter, startingWaypoint, null, returnOnRoute) 
    {
        onCompleted = () => MeatShopManager.Instance.CustomerBuy(NpcCharacter, ChooseMeatToBuy());
    }

    public override void OnStateEnter(CharacterState previousState)
    {
        MeatShopManager.Instance.CustomersWalkingToShop.Add(this);
    }

    public override void OnStateExit(CharacterState nextState)
    {
        base.OnStateExit(nextState);
        MeatShopManager.Instance.CustomersWalkingToShop.Remove(this);
    }

    public void OnMeatShopClosed()
    {
        NpcCharacter.ChangeState(new WalkingNpcCharacterState(NpcCharacter));
    }

    public Meat ChooseMeatToBuy()
    {
        return null;
    }
}
