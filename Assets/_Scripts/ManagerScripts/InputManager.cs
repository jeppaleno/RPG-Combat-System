using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager player;
    
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount; 
    public float verticalInput;
    public float horizontalInput;

    public bool a_Input;
    public bool b_input;
    public bool x_Input;
    public bool y_Input;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool inventory_Input;
    public bool lockOnInput;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;
    public bool jump_Input;
    public bool sprint_Input;

    public bool tap_rb_Input;
    public bool hold_rb_Input;

    public bool lb_Input;
    public bool tap_lb_Input;

    public bool tap_lt_Input; 
    public bool tap_rt_input;

    public bool rollFlag;
    public bool isInteracting;
    public bool comboFlag;
    public bool twohandFlag;
    public bool lockOnFlag;
    public bool fireFlag;
    public bool inventoryFlag;

    public Transform criticalRayCastStartPoint;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerActions.Inventory.performed += i => inventory_Input = true;

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.PlayerActions.A_Input.performed += i => a_Input = true;
            playerControls.PlayerActions.X.performed += i => x_Input = true;
            playerControls.PlayerActions.Y.performed += i => y_Input = true;

            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false; 

            playerControls.PlayerActions.Attack.performed += i => tap_rb_Input = true;
            playerControls.PlayerActions.Attack.canceled += i => tap_rb_Input = false;
            playerControls.PlayerActions.HeavyAttack.performed += i => tap_rt_input = true;

            playerControls.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
            playerControls.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;

            playerControls.PlayerActions.TapLB.performed += i => tap_lb_Input = true;

            playerControls.PlayerActions.LB.performed += i => lb_Input = true;
            playerControls.PlayerActions.LB.canceled += i => lb_Input = false;

            playerControls.PlayerActions.LT.performed += i => tap_lt_Input = true;

            playerControls.PlayerActions.DPadRight.performed += i => d_Pad_Right = true;
            playerControls.PlayerActions.DPadLeft.performed += i => d_Pad_Left = true;

            playerControls.PlayerActions.Inventory.performed += i => inventory_Input = true;

            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;

            playerControls.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            playerControls.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;

            playerControls.PlayerActions.Roll.performed += i => b_input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpingInput();
        HandleSprintingInput();
        HandleRollInput();

        HandleHoldLBInput();        
        HandleTapRBInput();
        HandleTapRTInput();
        HandleTapLTInput();
        HandleTapLBInput();

        HandleHoldRBInput();

        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        //HandleDodgeInput();
        HandleTwoHandInput();
        HandleUseConsumableInput();
    }

    private void HandleMovementInput()
    {
      
        verticalInput = movementInput.y; //Vertical value of joystick
        horizontalInput = movementInput.x; //Horizontal value of joystick
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        if (lockOnFlag && sprint_Input == false)
        {
            player.playerAnimatorManager.UpdateAnimatorValues(horizontalInput, verticalInput, player.playerLocomotionManager.isSprinting);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorValues(0, moveAmount, player.playerLocomotionManager.isSprinting);
        }
        
    }

    private void HandleSprintingInput() 
    {
        if (player.isHoldingArrow)
            return;

        // Sprints when input is being pressed and aleady is running. 
        if (sprint_Input && moveAmount > 0.5f)
        {
            player.playerLocomotionManager.isSprinting = true;
            player.isSprinting = true;
        }
        else
        {
            player.playerLocomotionManager.isSprinting = false;
            player.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            player.playerLocomotionManager.HandleJumping();
        }
    }

    private void HandleTapRBInput() 
    {
       if (tap_rb_Input)
       {
            tap_rb_Input = false;

            if (player.playerInventoryManager.rightWeapon.tap_RB_Action != null)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(player);
            }
       }
    }

    private void HandleHoldRBInput()
    {
        if (hold_rb_Input)
        {
            if (player.playerInventoryManager.rightWeapon.hold_RB_Action != null)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(player);
            }
        }
        else if (hold_rb_Input == false) //Makes arrow shoot when letting go of button
        {
            if (player.isHoldingArrow)
            {
                if (player.playerInventoryManager.rightWeapon.tap_RB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(player);
                }
            }
        }
    }

    private void HandleTapRTInput()
    {
        if (tap_rt_input)
        {
            tap_rt_input = false;

            if (player.playerInventoryManager.rightWeapon.tap_RT_Action != null)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(player);
            }
        }
    }

    private void HandleTapLTInput()
    {
        if (tap_lt_Input)
        {
            tap_lt_Input = false;

            if (player.isTwoHandingWeapon)
            {
                //It will be the right handed weapon
                if (player.playerInventoryManager.rightWeapon.tap_LT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(player);
                }
            }
            else
            {
                if (player.playerInventoryManager.leftWeapon.tap_LT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(player);
                }
            }
        }
    }

    private void HandleHoldLBInput()
    {
        if (player.isFiringSpell) //ADD IN AIR AND SPRINTING BOOLS TOO 
        {
            lb_Input = false;
            return;
        }

        if (lb_Input)
        {
            if (player.isTwoHandingWeapon)
            {
                if (player.playerInventoryManager.rightWeapon.hold_LB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(player);
                } 
            }
            else
            {
                if (player.playerInventoryManager.leftWeapon.hold_LB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(player);
                }
            }
        }
        else if (lb_Input == false)
        {
            if (player.isAiming)
            {
                player.isAiming = false;
                player.uiManager.crossHair.SetActive(false);
                player.cameraManager.ResetAimCameraRotations();
            }
            
            if (player.blockingCollider.blockingCollider.enabled)
            {
                player.isBlocking = false;
                player.blockingCollider.DisableBlockingCollider();
            }
        }
    }

    private void HandleTapLBInput()
    {
        if (tap_lb_Input)
        {
            tap_lb_Input = false;

            if (player.isTwoHandingWeapon)
            {
                if (player.playerInventoryManager.rightWeapon.tap_LB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(player);
                }
            }
            else
            {
                if (player.playerInventoryManager.leftWeapon.tap_LB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(player);
                }
            }
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (d_Pad_Right)
        {
            player.playerInventoryManager.changeRightWeapon();
        }
        else if(d_Pad_Left)
        {
            player.playerInventoryManager.changeLeftWeapon();
        }
    }

    private void HandleInventoryInput()
    {
        if (inventoryFlag)
        {
            player.uiManager.UpdateUI();
        }

        if (inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                player.uiManager.OpenSelectWindow();
                player.uiManager.hudWindow.SetActive(false);
            }
            else
            {
                player.uiManager.CloseSelectWindow();
                player.uiManager.CloseAllInventoryWindows();
                player.uiManager.hudWindow.SetActive(true);
            }
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            player.cameraManager.HandleLockOn();
            if (player.cameraManager.nearestLockOnTarget != null)
            {
                player.cameraManager.currentLockOnTarget = player.cameraManager.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)
        {
            lockOnInput = false;
            lockOnFlag = false;
            player.cameraManager.ClearLockOnTargets();
        }

        if (lockOnFlag && right_Stick_Left_Input)
        {
            right_Stick_Left_Input = false;
            player.cameraManager.HandleLockOn();
            if (player.cameraManager.leftLockOnTarget != null)
            {
                player.cameraManager.currentLockOnTarget = player.cameraManager.leftLockOnTarget;
            }
        }

        if (lockOnFlag && right_Stick_Right_Input)
        {
            right_Stick_Right_Input = false;
            player.cameraManager.HandleLockOn();
            if(player.cameraManager.rightLockOnTarget != null)
            {
                player.cameraManager.currentLockOnTarget = player.cameraManager.rightLockOnTarget; 
            }
        }

        player.cameraManager.SetCameraHeight();
    }

    //private void HandleDodgeInput()
    //{
        //if (x_Input)
        //{
           // x_Input = false;
            //character.HandleDodge();
       // }
    //}

    private void HandleTwoHandInput()
    {
        if (y_Input)
        {
            y_Input = false;

            twohandFlag = !twohandFlag;

            if (twohandFlag)
            {
                player.isTwoHandingWeapon = true;
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                player.playerWeaponSlotManager.LoadTwoHandIKTargets(true); 
            }
            else
            {
                player.isTwoHandingWeapon = false;
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                player.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
            }
        }
    }


    private void HandleRollInput()
    {
        //rollInputTimer += Time.deltaTime;
        if (b_input)
        {
            rollFlag = true;
            b_input = false;
            player.playerLocomotionManager.HandleRolling();
        }
    }

    private void HandleUseConsumableInput()
    {
        if (x_Input)
        {
            x_Input = false;
            // Use Current consumable
            player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player.playerAnimatorManager, player.playerWeaponSlotManager, player.playerEffectsManager);
        }
    }
}
