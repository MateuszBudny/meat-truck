using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTween : MonoBehaviour
{
    [SerializeField]
    private float jumpDuration = 1f;
    [SerializeField]
    private float jumpPower = 3f;
    [SerializeField]
    private int jumpsNum = 1;


    public Sequence Play(Vector3 target, Action onComplete = null)
    {
        return transform.DOJump(target, jumpPower, jumpsNum, jumpDuration)
            .OnComplete(() => onComplete?.Invoke())
            .Play();
    }
}
