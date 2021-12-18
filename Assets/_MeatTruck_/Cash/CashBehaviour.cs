using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashBehaviour : MonoBehaviour, IItemJumpTo
{
    public int value;
    [SerializeField]
    private ItemJumpToSequence jumpSequence;

    public GameObject GameObject => gameObject;

    public void JumpTo(Vector3 target, bool scaleTo0, Action onComplete = null, bool turnOffColliders = false) => jumpSequence.JumpTo(target, scaleTo0, onComplete, turnOffColliders);
}
