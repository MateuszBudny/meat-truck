using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRouteToMeatShopNpcCharacterState : OnTemporaryRouteCharacterNpcState
{
    public OnRouteToMeatShopNpcCharacterState(NpcCharacterBehaviour npcCharacter, Waypoint startingWaypoint, bool returnOnRoute) : base(npcCharacter, startingWaypoint, null, returnOnRoute) 
    {
        onCompleted = () => MeatShopManager.Instance.CustomerBuy(NpcCharacterBehaviour, ChooseMeatToBuy());
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
        NpcCharacterBehaviour.ChangeState(new WalkingNpcCharacterState(NpcCharacterBehaviour));
    }

    public MeatData ChooseMeatToBuy()
    {
        WeightedList<MeatPopularity> meatsToChooseFrom = new WeightedList<MeatPopularity>(NpcCharacterBehaviour.CurrentCityRegion.meatsPopularity.List);
        MeatData chosenMeat = null;
        while (!chosenMeat && meatsToChooseFrom.List.Count > 0)
        {
            MeatPopularity tempChosenMeatPopularity = meatsToChooseFrom.GetRandomWeightedElement();
            chosenMeat = GameManager.Instance.Player.Inventory.HasMeatType(tempChosenMeatPopularity.meatData) ? tempChosenMeatPopularity.meatData : null;
            if(!chosenMeat)
            {
                if(tempChosenMeatPopularity.thisOrNothing)
                {
                    break;
                }
                else
                {
                    meatsToChooseFrom.List.Remove(tempChosenMeatPopularity);
                }
            }
        }

        if(!chosenMeat)
        {
            NoMeatBought();
        }

        return chosenMeat;
    }

    private void NoMeatBought()
    {
        NpcCharacterBehaviour.NpcCharacterEffects.PlayForLimitedTime(NpcCharacterEffect.NoMeatBought, 2f);
    }
}
