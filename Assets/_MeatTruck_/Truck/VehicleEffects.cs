using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleEffects : EffectsAbstract<VehicleEffect>
{
    [Header("Tires smoke")]
    public float tiresSmokeDuration = 2f;
    public float tiresMinAccelerationToTriggerSmoke = 0.9f;
    [SerializeField]
    private ParticleSystem leftTireSmoke;
    [SerializeField]
    private ParticleSystem rightTireSmoke;

    public override void Play(VehicleEffect effect)
    {
        switch(effect)
        {
            case VehicleEffect.TiresSmoke:
                if(!leftTireSmoke.isPlaying)
                {
                    leftTireSmoke.Play(true);
                }
                if(!rightTireSmoke.isPlaying)
                {
                    rightTireSmoke.Play(true);
                }
                break;
        }
    }

    public override void PlayForLimitedTime(VehicleEffect effect, float duration, ParticleSystemStopBehavior stopBehaviour)
    {
        switch(effect)
        {
            case VehicleEffect.TiresSmoke:
                if(!leftTireSmoke.isPlaying || !rightTireSmoke.isPlaying)
                {
                    StartCoroutine(PlayForLimitedTimeEnumerator(effect, duration, stopBehaviour));
                }
                break;
        }
    }

    public override void Stop(VehicleEffect effect, ParticleSystemStopBehavior stopBehavior)
    {
        switch (effect) {
            case VehicleEffect.TiresSmoke:
                if(leftTireSmoke.isPlaying)
                {
                    leftTireSmoke.Stop(true, stopBehavior);
                }
                if(rightTireSmoke.isPlaying)
                {
                    rightTireSmoke.Stop(true, stopBehavior);
                }
                break;
        }
    }
}

public enum VehicleEffect
{
    TiresSmoke,
}