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
        #region Vertical
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        player.animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        player.animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void DisableCollision()
    {
        player.characterController.enabled = false;
    }

    public void EnableCollision()
    {
        player.characterController.enabled = true;
    }

    public virtual void SuccesfullyUseCurrentConsumable()
    {
        if (character.characterInventoryManager.currentConsumable != null)
        {
            character.characterInventoryManager.currentConsumable.SucessfullyToConsumeItem(player);
        }
    }

}
