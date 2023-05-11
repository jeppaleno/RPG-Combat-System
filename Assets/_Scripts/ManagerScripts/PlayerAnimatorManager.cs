using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;

    int horizontal;
    int vertical;
    
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        // Animation snapping
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55)
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

        player.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        player.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

   /* public void DisableCollision()
    {
        player.playerLocomotionManager.characterCollider.enabled = false;
        player.playerLocomotionManager.characterCollisionBlockerCollider.enabled = false;
    }

    public void EnableCollision()
    {
        player.playerLocomotionManager.characterCollider.enabled = true;
        player.playerLocomotionManager.characterCollisionBlockerCollider.enabled = true;
    }*/

    private void OnAnimatorMove()
    {
        if (character.isUsingRootMotion)
        {
            //Moves player gameobject in direction of players model, usefull for anims with rootmotions
            player.playerLocomotionManager.playerRigidbody.drag = 0;
            Vector3 deltaPosition = player.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            player.playerLocomotionManager.playerRigidbody.velocity = velocity;
        }
    }

}
