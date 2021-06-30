using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Character))]
public class RagdollCharacterControllerExtension : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float walkSpeed = 0.5f;
    [SerializeField]
    private float rotationSpeed = 5f;

    [Header("Ragdoll Rigidbodies Properties")]
    [SerializeField]
    private int rigidbodySolverIterations = 12;
    [SerializeField]
    private int rigidbodySolverVelocityIterations = 12;
    [SerializeField]
    private int rigidbodyMaxAngularSpeed = 30;
    
    public CharacterController CoreController { get; private set; }

    private Character character;
    private List<Collider> ragdollColliders = new List<Collider>();
    private List<Rigidbody> ragdollRigidbodies;

    private void Awake()
    {
        CoreController = GetComponent<CharacterController>();
        character = GetComponent<Character>();

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
        if (CoreController.enabled)
        {
            Vector3 desiredMovement = new Vector3(
                character.GetDesiredMovement().x,
                0f,
                character.GetDesiredMovement().y);
            CoreController.SimpleMove(desiredMovement * walkSpeed);

            transform.rotation = Quaternion.Slerp(transform.rotation, character.GetDesiredRotation(), rotationSpeed * Time.deltaTime);
        }
    }

    public void SetAsAnimated()
    {
        animator.enabled = true;
        CoreController.enabled = true;
        ragdollColliders.ForEach(ragdollCollider => ragdollCollider.enabled = false);
        ragdollRigidbodies.ForEach(ragdollRigidbody => ragdollRigidbody.isKinematic = true);
    }

    public void SetAsRagdoll()
    {
        animator.enabled = false;
        CoreController.enabled = false;
        ragdollColliders.ForEach(ragdollCollider => ragdollCollider.enabled = true);
        ragdollRigidbodies.ForEach(ragdollRigidbody => ragdollRigidbody.isKinematic = false);
    }
}
