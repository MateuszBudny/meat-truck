using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleEffects : EffectsAbstract<PlayerVehicleEffect>
{
    [SerializeField]
    private ParticleSystem gatheringSmoke;
    [SerializeField]
    private ParticleSystem gatheringFinishedSuccessfully;

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
}

public enum PlayerVehicleEffect
{
    GatheringSmoke,
    GatheringFinishedSuccessfully,
}
