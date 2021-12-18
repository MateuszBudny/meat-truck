using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacterEffects : EffectsAbstract<NpcCharacterEffect>
{
    [Header("No Meat Bought Effect")]
    [SerializeField]
    private ScaleTween NoMeatBoughtScaleInTween;
    [SerializeField]
    private ScaleTween NoMeatBoughtScaleOutTween;

    public override void Play(NpcCharacterEffect effect)
    {
        switch(effect)
        {
            case NpcCharacterEffect.NoMeatBought:
                NoMeatBoughtScaleInTween.gameObject.SetActive(true);
                NoMeatBoughtScaleInTween.Play();
                break;
        }
    }

    public override void PlayForLimitedTime(NpcCharacterEffect effect, float duration, ParticleSystemStopBehavior stopBehaviour = ParticleSystemStopBehavior.StopEmitting)
    {
        StartCoroutine(PlayForLimitedTimeEnumerator(effect, duration, stopBehaviour));
    }

    public override void Stop(NpcCharacterEffect effect, ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting)
    {
        switch (effect)
        {
            case NpcCharacterEffect.NoMeatBought:
                NoMeatBoughtScaleOutTween.Play(() => NoMeatBoughtScaleOutTween.gameObject.SetActive(false));
                break;
        }
    }
}

public enum NpcCharacterEffect
{
    NoMeatBought,
}
