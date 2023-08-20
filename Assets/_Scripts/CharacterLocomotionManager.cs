using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;
    PlayerManager player;

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

    [Header("Movement Stats")]
    [SerializeField]
    protected float movementSpeed = 5;
    [SerializeField]
    protected float walkingSpeed = 1;
    [SerializeField]
    protected float sprintSpeed = 7;
    [SerializeField]
    protected float rotationSpeed = 10;

    [Header("Stamina Costs")]
    [SerializeField]
    protected int rollStaminaCost = 15;
    protected int backstepStaminaCost = 12;
    protected int sprintStaminaCost = 1;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        player = GetComponent<PlayerManager>();
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

    public virtual void HandleRotation()
    {
        if (player.canRotate)
        {
            if (player.isAiming)
            {
                Quaternion targetRotation = Quaternion.Euler(0, player.cameraManager.cameraTransform.eulerAngles.y, 0);
                Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = playerRotation;
            }
            else
            {
                if (player.inputManager.lockOnFlag)
                {
                    if (player.inputManager.sprintFlag || player.inputManager.rollFlag)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = player.cameraManager.cameraTransform.forward * player.inputManager.verticalInput;
                        targetDirection += player.cameraManager.cameraTransform.right * player.inputManager.horizontalInput;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = player.cameraManager.currentLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    Vector3 targetDir = Vector3.zero;
                    float moveOverride = player.inputManager.moveAmount;

                    targetDir = player.cameraManager.cameraObject.transform.forward * player.inputManager.verticalInput;
                    targetDir += player.cameraManager.cameraObject.transform.right * player.inputManager.horizontalInput;

                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                        targetDir = player.transform.forward;

                    float rs = rotationSpeed;

                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * Time.deltaTime);

                    player.transform.rotation = targetRotation;
                }
            }
        }
    }

    public virtual void HandleGroundedMovement()
    {
        if (player.inputManager.rollFlag)
            return;

        if (player.isInteracting)
            return;

        if (!player.isGrounded)
            return;

        moveDirection = player.cameraManager.transform.forward * player.inputManager.verticalInput;
        moveDirection = moveDirection + player.cameraManager.transform.right * player.inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.isSprinting)
        {
            player.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
            player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
        }
        else
        {
            if (player.inputManager.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

            }
            else if (player.inputManager.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        if (player.inputManager.lockOnFlag && player.inputManager.sprintFlag == false)
        {
            player.playerAnimatorManager.UpdateAnimatorValues( player.inputManager.horizontalInput, player.inputManager.verticalInput, player.isSprinting);
        }
    }

    public virtual void HandleRollingAndSprinting()
    {
        if (player.animator.GetBool("isInteracting"))
            return;

        if (player.playerStatsManager.currentStamina <= 0)
            return;

        if (player.inputManager.rollFlag)
        {
            player.inputManager.rollFlag = false;

            moveDirection = player.cameraManager.cameraObject.transform.forward * player.inputManager.verticalInput;
            moveDirection += player.cameraManager.cameraObject.transform.right * player.inputManager.horizontalInput;

            if (player.inputManager.moveAmount > 0)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Rolling", true); //Add root
                player.playerAnimatorManager.EraseHandIKWeapon();
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = rollRotation;
                player.playerStatsManager.DeductStamina(rollStaminaCost);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation("Dodge", true); //Add root
                player.playerAnimatorManager.EraseHandIKWeapon();
                player.playerStatsManager.DeductStamina(backstepStaminaCost);
            }
        }
    }
    public virtual void HandleJumping()
    {
        if (player.isInteracting)
            return;

        if (player.playerStatsManager.currentStamina <= 0)
            return;
        Debug.Log("Jump");
        moveDirection += player.cameraManager.cameraObject.transform.forward * player.inputManager.verticalInput;
        moveDirection += player.cameraManager.cameraObject.transform.right * player.inputManager.horizontalInput;

        player.playerAnimatorManager.PlayTargetAnimation("Jump", false, false, true);
        player.playerAnimatorManager.EraseHandIKWeapon();
        moveDirection.y = 0;
        Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
        player.transform.rotation = jumpRotation;
        player.inputManager.jump_Input = false;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
    }
}
