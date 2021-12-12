using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScaleTween : MonoBehaviour
{
    [SerializeField]
    private float scaleDuration = 1f;
    [SerializeField]
    private float scalePunch = 2f;
    [SerializeField]
    private int vibrato = 10;
    [SerializeField]
    private float elasticity = 1f;

    public Tweener Play(Action onComplete = null)
    {
        return transform.DOPunchScale(new Vector3(scalePunch, scalePunch, scalePunch), scaleDuration, vibrato, elasticity)
            .OnComplete(() => onComplete?.Invoke());
    }
}
