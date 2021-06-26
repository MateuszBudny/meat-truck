using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NpcCharacterControllerExtension : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float walkSpeed = 0.5f;

    [Header("Ragdoll Rigidbodies Properties")]
    [SerializeField]
    private int rigidbodySolverIterations = 12;
    [SerializeField]
    private int rigidbodySolverVelocityIterations = 12;
    [SerializeField]
    private int rigidbodyMaxAngularSpeed = 30;
    
    private CharacterController controller;
    private List<Collider> ragdollColliders = new List<Collider>();
    private List<Rigidbody> ragdollRigidbodies;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        GetComponentsInChildren<Collider>().ToList().ForEach(collider =>
            {
                if(!(collider is CharacterController))
                {
                    ragdollColliders.Add(collider);
                }
            });
        ragdollRigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        ragdollRigidbodies.ForEach(ragdollRigidbody =>
        {
            ragdollRigidbody.solverIterations = rigidbodySolverIterations;
            ragdollRigidbody.solverVelocityIterations = rigidbodySolverVelocityIterations;
            ragdollRigidbody.maxAngularVelocity = rigidbodyMaxAngularSpeed;
        });

        SetAsAnimated();
    }

    private void Update()
    {
        if (controller.enabled)
        {
            controller.SimpleMove(transform.forward * walkSpeed);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.PlayerVehicle.ToString()) || collider.CompareTag(Tags.NpcVehicle.ToString()))
        {
            SetAsRagdoll();
        }
    }

    public void SetAsAnimated()
    {
        animator.enabled = true;
        controller.enabled = true;
        ragdollColliders.ForEach(ragdollCollider => ragdollCollider.enabled = false);
        ragdollRigidbodies.ForEach(ragdollRigidbody => ragdollRigidbody.isKinematic = true);
    }

    public void SetAsRagdoll()
    {
        animator.enabled = false;
        controller.enabled = false;
        ragdollColliders.ForEach(ragdollCollider => ragdollCollider.enabled = true);
        ragdollRigidbodies.ForEach(ragdollRigidbody => ragdollRigidbody.isKinematic = false);
    }
}
