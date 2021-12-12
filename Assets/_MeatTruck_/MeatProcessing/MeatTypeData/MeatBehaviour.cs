using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollidersHandler), typeof(JumpTween), typeof(PunchScaleTween))]
public class MeatBehaviour : MonoBehaviour
{
    public Meat meat;

    public CollidersHandler CollidersHandler { get; private set; }

    private JumpTween jumpTweenBehaviour;
    private PunchScaleTween punchScaleTweenBehaviour;

    private void Awake()
    {
        CollidersHandler = GetComponent<CollidersHandler>();
        jumpTweenBehaviour = GetComponent<JumpTween>();
        punchScaleTweenBehaviour = GetComponent<PunchScaleTween>();
    }

    public void JumpTo(Vector3 target, bool turnOffColliders = false, Action onComplete = null)
    {
        if(turnOffColliders)
        {
            CollidersHandler.CollidersSetEnabled(false);
        }
        jumpTweenBehaviour.Play(target, onComplete);
        punchScaleTweenBehaviour.Play();
    }
}