using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfLocomotionManager : CharacterLocomotionManager
{
    // Additional properties or methods specific to Dwarf locomotion can be added here

    [Header("Dwarf Movement Stats")]
    [SerializeField]
    float dwarfMovementSpeed = 4; // Slower movement speed for the Dwarf

    protected override void Awake()
    {
        base.Awake();
        // Additional initialization specific to Dwarf if needed
        Debug.Log("I'm the Dwarf!");
    }

    // Override the HandleGroundedMovement method to use Dwarf's movement speed
    public override void HandleGroundedMovement()
    {
        // Custom implementation for Dwarf's grounded movement
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
            player.characterController.Move(moveDirection * dwarfMovementSpeed * Time.deltaTime);
            player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
        }
        else
        {
            if (player.inputManager.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * dwarfMovementSpeed * Time.deltaTime);
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

    // Add a new Dwarf-specific ability: Hammer Slam
    public void HammerSlam()
    {
        // Your custom Hammer Slam logic here
    }

    // Add other methods or properties specific to Dwarf locomotion here
}
