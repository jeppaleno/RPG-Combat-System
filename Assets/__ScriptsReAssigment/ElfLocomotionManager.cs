using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfLocomotionManager : CharacterLocomotionManager
{
    // Additional properties or methods specific to Elf locomotion can be added here

    [Header("Elf Movement Stats")]
    [SerializeField]
    float elfMovementSpeed = 8; // Faster movement speed for the Elf

    protected override void Awake()
    {
        base.Awake();
        // Additional initialization specific to Elf if needed
        Debug.Log("I'm the Elf!");
    }

    // Override the HandleGroundedMovement method to use Elf's movement speed
    public override void HandleGroundedMovement()
    {
        // Custom implementation for Elf's grounded movement
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
            player.characterController.Move(moveDirection * elfMovementSpeed * Time.deltaTime);
            player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
        }
        else
        {
            if (player.inputManager.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * elfMovementSpeed * Time.deltaTime);
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

    // Add a new Elf-specific ability: Forest Dash
    public void ForestDash()
    {
        // Your custom Forest Dash logic here
    }

    // Add other methods or properties specific to Elf locomotion here
}
