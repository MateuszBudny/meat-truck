using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointsTracker))]
public class NpcCharacter : Character
{  
    public NpcCharacterState State { get => characterGenericState as NpcCharacterState; private set => characterGenericState = value; }
    public RagdollCharacterControllerExtension Controller { get; private set; }
    public WaypointsTracker Tracker { get; private set; }

    public bool IsGatherable => State is DeadNpcCharacterState;

    private void Awake()
    {
        State = new AliveNpcCharacterState(this);
        Controller = GetComponent<RagdollCharacterControllerExtension>();
        Tracker = GetComponent<WaypointsTracker>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(
            GetMovement().x,
            0f,
            GetMovement().y);

        Controller.SimpleMove(movement);
    }

    private void OnTriggerEnter(Collider collider)
    {
        State.OnTriggerEnter(collider);
    }

    public override Vector2 GetMovement() => State.GetMovement();

    public override Quaternion GetRotation() => State.GetRotation();

    /// <summary>
    /// Use this to change state. (do not use state's inner ChangeState() method directly!)
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(NpcCharacterState newState)
    {
        if (State.ChangeState(newState))
        {
            State = newState;
        }
    }

    public void SetAsRagdoll() => Controller.SetAsRagdoll();
}
