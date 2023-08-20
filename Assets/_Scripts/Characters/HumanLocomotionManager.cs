using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLocomotionManager : CharacterLocomotionManager
{
    // Additional properties or methods specific to Human locomotion can be added here

    [Header("Human Movement Stats")]
    [SerializeField]
    float humanMovementSpeed = 6; // Normal movement speed for the Human

    protected override void Awake()
    {
        base.Awake();
        // Additional initialization specific to Human if needed
        Debug.Log("I'm the Human!");
    }

    // Override the HandleGroundedMovement method to use Human's movement speed
    public override void HandleGroundedMovement()
    {
        // Custom implementation for Human's grounded movement
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
            player.characterController.Move(moveDirection * humanMovementSpeed * Time.deltaTime);
            player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
        }
        else
        {
            if (player.inputManager.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * humanMovementSpeed * Time.deltaTime);
            }
            else if (player.inputManager.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        if (player.inputManager.lockOnFlag && player.inputManager.sprintFlag == false)
        {
            player.playerAnimatorManager.UpdateAnimatorValues(player.inputManager.horizontalInput, player.inputManager.verticalInput, player.isSprinting);
        }
    }

    // Add a new Human-specific ability: Shield Bash
    public void ShieldBash()
    {
        // Your custom Shield Bash logic here
    }

    // Add other methods or properties specific to Human locomotion here
}
