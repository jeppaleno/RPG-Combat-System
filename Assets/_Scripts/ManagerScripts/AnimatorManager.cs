using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : PreAnimatorManager
{
    
    PlayerManager playerManager;
    InputManager inputManager;
    PlayerStats playerStats;
    Character character;
    int horizontal;
    int vertical;
    //public bool canRotate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        character = GetComponent<Character>();
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

    public void CanRotate()
    {
        animator.SetBool("canRotate", true);
    }

    public void stopRotation()
    {
        animator.SetBool("canRotate", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.isUsingRootMotion)
        {
            //Moves player gameobject in direction of players model, usefull for anims with rootmotions
            character.playerRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            character.playerRigidbody.velocity = velocity;
        }

    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public void EnableIsParrying()
    {
        playerManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        playerManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        playerManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        playerManager.canBeRiposted = false;
    }
    public override void TakeCriticalDamageAnimationEvent()
    {
        playerStats.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
        playerManager.pendingCriticalDamage = 0;
    }

    public void DisableCollision()
    {
        character.characterCollider.enabled = false;
        character.characterCollisionBlockerCollider.enabled = false;
    }

    public void EnableCollision()
    {
        character.characterCollider.enabled = true;
        character.characterCollisionBlockerCollider.enabled = true;
    }

}
