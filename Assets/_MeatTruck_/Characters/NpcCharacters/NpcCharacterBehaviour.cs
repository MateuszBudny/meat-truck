using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointsTracker), typeof(RagdollCharacterControllerExtension), typeof(CollidersHandler))]
public class NpcCharacterBehaviour : CharacterBehaviour
{
    public NpcCharacter npcCharacter;

    public NpcCharacterState State { get => characterGenericState as NpcCharacterState; private set => characterGenericState = value; }
    public CityRegion CurrentCityRegion { get; private set; }
    public RagdollCharacterControllerExtension Controller { get; private set; }
    public WaypointsTracker Tracker { get; private set; }
    public CollidersHandler CollidersHandler { get; private set; }

    public bool IsGatherable => State is DeadNpcCharacterState;

    private void Awake()
    {
        State = new WalkingNpcCharacterState(this);
        Controller = GetComponent<RagdollCharacterControllerExtension>();
        Tracker = GetComponent<WaypointsTracker>();
        CollidersHandler = GetComponent<CollidersHandler>();
    }

    private void Start()
    {
        CurrentCityRegion = CityManager.Instance.CurrentRegions[npcCharacter.defaultCityRegionData];
    }

    private void Update()
    {
        State.OnUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        State.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        State.OnTriggerExit(collider);
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
