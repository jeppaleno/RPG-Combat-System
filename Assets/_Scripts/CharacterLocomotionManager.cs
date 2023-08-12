using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    public Vector3 moveDirection;
    public LayerMask groundLayer;

    [Header("Gravity Settings")]
    public float inAirTimer;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -20;   // THE FORCE APPLIED TO YOU WHILST GROUNDED
    [SerializeField] protected float fallStartYVelocity = -7;   // THE FORCE APPLIED TO YOU WHEN YOU BEGIN TO FALL (INCREASES OVER TIME)
    [SerializeField] protected float jumpHeight = 1.0f;
    [SerializeField] protected float gravityForce = -9.81f;
    [SerializeField] float groundCheckSphereRadius = 1f;
    protected bool fallingVelocitySet = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        character.animator.SetBool("isGrounded", character.isGrounded);
        HandleGroundCheck();
    }

    public virtual void HandleGroundCheck()
    {
        if (character.isGrounded)
        {
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!fallingVelocitySet)
            {
                fallingVelocitySet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            yVelocity.y += gravityForce * Time.deltaTime;
        }

        character.animator.SetFloat("inAirTimer", inAirTimer);
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
    }
}
