using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleEffects : EffectsAbstract<PlayerVehicleEffect>
{
    public override void Play(PlayerVehicleEffect effect)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayForLimitedTime(PlayerVehicleEffect effect, float duration, ParticleSystemStopBehavior stopBehaviour)
    {
        throw new System.NotImplementedException();
    }

    public override void Stop(PlayerVehicleEffect effect, ParticleSystemStopBehavior stopBehavior)
    {
        throw new System.NotImplementedException();
    }
}

public enum PlayerVehicleEffect
{

}
