using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [Header("Movement Stats")]
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float walkingSpeed = 1;
    [SerializeField]
    float sprintSpeed = 7;
    [SerializeField]
    float rotationSpeed = 10;

    [Header("Stamina Costs")]
    [SerializeField]
    int rollStaminaCost = 15;
    int backstepStaminaCost = 12;
    int sprintStaminaCost = 1;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {

    }

    public void HandleRotation()
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

    public void HandleGroundedMovement()
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
            player.playerAnimatorManager.UpdateAnimatorValues(player.inputManager.verticalInput, player.inputManager.horizontalInput, player.isSprinting);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorValues(player.inputManager.moveAmount, 0, player.isSprinting);
        }
    }

    public void HandleRollingAndSprinting()
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

    public void HandleJumping()
    {
        if (player.isInteracting)
            return;

        if (player.playerStatsManager.currentStamina <= 0)
            return;

        if (player.inputManager.jump_Input)
        {
            player.inputManager.jump_Input = false;

            if (player.inputManager.moveAmount > 0)
            {
                moveDirection = player.cameraManager.cameraObject.transform.forward * player.inputManager.verticalInput;
                moveDirection += player.cameraManager.cameraObject.transform.right * player.inputManager.horizontalInput;
                player.playerAnimatorManager.PlayTargetAnimation("Jump", true);
                player.playerAnimatorManager.EraseHandIKWeapon();
                moveDirection.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = jumpRotation;
            }
        }
    }
}
