using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointsTracker), typeof(RagdollCharacterControllerExtension), typeof(CollidersHandler))]
public class NpcCharacterBehaviour : CharacterBehaviour
{
    public NpcCharacter npcCharacter;
    public NpcCharacterState State { get => characterGenericState as NpcCharacterState; private set => characterGenericState = value; }
    public RagdollCharacterControllerExtension Controller { get; private set; }
    public WaypointsTracker Tracker { get; private set; }
    public CollidersHandler CollidersHandler { get; private set; }

    public bool IsGatherable => State is DeadNpcCharacterState;

    private void Awake()
    {
        State = new AliveNpcCharacterState(this);
        Controller = GetComponent<RagdollCharacterControllerExtension>();
        Tracker = GetComponent<WaypointsTracker>();
        CollidersHandler = GetComponent<CollidersHandler>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(
            GetMovement().x,
            0f,
            GetMovement().y);

        Controller.SimpleMove(movement, GetRotation());

    }

    private void OnTriggerEnter(Collider collider)
    {
        State.OnTriggerEnter(collider);
    }

    public override Vector2 GetMovement() => State.GetMovement();

    public override Quaternion GetRotation() => State.GetRotation();

    public void ChangeState(NpcCharacterState newState)
    {
        NpcCharacterState previousState = State;
        State.OnStateExit(newState);
        State = newState;
        State.OnStateEnter(previousState);
    }

    public void SetAsRagdoll() => ChangeState(new DeadNpcCharacterState(this));
}
