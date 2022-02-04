using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleEffects : EffectsAbstract<PlayerVehicleEffect>
{
    [SerializeField]
    private ParticleSystem gatheringSmoke;
    [SerializeField]
    private ParticleSystem gatheringFinishedSuccessfully;
    [SerializeField]
    private GameObject gatheringRangeCorrectSpeed;
    [SerializeField]
    private GameObject gatheringRangeTooFast;

    // TODO
    // maybe effects objects should have MonoBehaviour on them which will wrap their effect type?

    public override void Play(PlayerVehicleEffect effect)
    {
        switch(effect)
        {
            case PlayerVehicleEffect.GatheringSmoke:
                gatheringSmoke.Play(true);
                break;
            case PlayerVehicleEffect.GatheringFinishedSuccessfully:
                gatheringFinishedSuccessfully.Play(true);
                break;
            case PlayerVehicleEffect.GatheringRangeCorrectSpeed:
                gatheringRangeCorrectSpeed.SetActive(true);
                break;
            case PlayerVehicleEffect.GatheringRangeTooFast:
                gatheringRangeTooFast.SetActive(true);
                break;
        }
    }

    public override void PlayForLimitedTime(PlayerVehicleEffect effect, float duration, ParticleSystemStopBehavior stopBehaviour)
    {
        throw new System.NotImplementedException();
    }

    public override void Stop(PlayerVehicleEffect effect, ParticleSystemStopBehavior stopBehavior)
    {
        switch (effect)
        {
            case PlayerVehicleEffect.GatheringSmoke:
                gatheringSmoke.Stop(true, stopBehavior);
                break;
            case PlayerVehicleEffect.GatheringFinishedSuccessfully:
                gatheringFinishedSuccessfully.Stop(true, stopBehavior);
                break;
            
        }
    }

    public override void Stop(PlayerVehicleEffect effect)
    {
        switch(effect)
        {
            case PlayerVehicleEffect.GatheringRangeCorrectSpeed:
                gatheringRangeCorrectSpeed.SetActive(false);
                break;
            case PlayerVehicleEffect.GatheringRangeTooFast:
                gatheringRangeTooFast.SetActive(false);
                break;
        }
    }
}

public enum PlayerVehicleEffect
{
    GatheringSmoke,
    GatheringFinishedSuccessfully,
    GatheringRangeCorrectSpeed,
    GatheringRangeTooFast,
}
