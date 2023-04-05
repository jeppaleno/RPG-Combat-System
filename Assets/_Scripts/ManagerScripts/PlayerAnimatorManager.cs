using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    InputManager inputManager;
    PlayerLocomotionManager playerLocomotionManager;

    int horizontal;
    int vertical;
    
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        // Animation snapping
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion

        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion 

        if (isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void DisableCollision()
    {
        playerLocomotionManager.characterCollider.enabled = false;
        playerLocomotionManager.characterCollisionBlockerCollider.enabled = false;
    }

    public void EnableCollision()
    {
        playerLocomotionManager.characterCollider.enabled = true;
        playerLocomotionManager.characterCollisionBlockerCollider.enabled = true;
    }

    private void OnAnimatorMove()
    {
        if (characterManager.isUsingRootMotion)
        {
            //Moves player gameobject in direction of players model, usefull for anims with rootmotions
            playerLocomotionManager.playerRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            playerLocomotionManager.playerRigidbody.velocity = velocity;
        }
    }

}
