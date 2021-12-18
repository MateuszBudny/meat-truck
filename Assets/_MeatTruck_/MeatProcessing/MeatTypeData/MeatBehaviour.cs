using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollidersHandler))]
public class MeatBehaviour : MonoBehaviour, IItemJumpTo
{
    public Meat meat;
    [SerializeField]
    private ItemJumpToSequence jumpSequence;

    public CollidersHandler CollidersHandler { get; private set; }

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        CollidersHandler = GetComponent<CollidersHandler>();
    }

    public void JumpTo(Vector3 target, bool scaleTo0, Action onComplete = null, bool turnOffColliders = false) => jumpSequence.JumpTo(target, scaleTo0, onComplete, turnOffColliders);
}