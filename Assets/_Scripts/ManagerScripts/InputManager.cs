using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerLocomotionManager playerLocomotionManager;
    PlayerCombatManager playerCombatManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager playerManager;
    PlayerEffectsManager playerEffectsManager;
    BlockingCollider blockingCollider;
    PlayerWeaponSlotManager weaponSlotManager;
    CameraManager cameraManager;
    public UIManager uiManager;
    
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount; 
    public float verticalInput;
    public float horizontalInput;

    public bool a_Input;
    public bool hold_rb_Input;
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
    public bool rb_Input;
    public bool lb_Input; 
    public bool lt_Input; 
    public bool rt_input;

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
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        uiManager = FindObjectOfType<UIManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.PlayerActions.A_Input.performed += i => a_Input = true;
            playerControls.PlayerActions.X.performed += i => x_Input = true;
            playerControls.PlayerActions.Y.performed += i => y_Input = true;

            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false; 

            playerControls.PlayerActions.Attack.performed += i => rb_Input = true;
            playerControls.PlayerActions.HeavyAttack.performed += i => rt_input = true;

            playerControls.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
            playerControls.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;
            playerControls.PlayerActions.HoldRB.canceled += i => fireFlag = true;

            playerControls.PlayerActions.LB.performed += i => lb_Input = true;
            playerControls.PlayerActions.LB.canceled += i => lb_Input = false;

            playerControls.PlayerActions.LT.performed += i => lt_Input = true;

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

        HandleFireBowInput();
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
            playerAnimatorManager.UpdateAnimatorValues(horizontalInput, verticalInput, playerLocomotionManager.isSprinting);
        }
        else
        {
            playerAnimatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotionManager.isSprinting);
        }
        
    }

    private void HandleSprintingInput() 
    {
        if (playerManager.isHoldingArrow)
            return;

        // Sprints when input is being pressed and aleady is running. 
        if (sprint_Input && moveAmount > 0.5f)
        {
            playerLocomotionManager.isSprinting = true;
            playerManager.isSprinting = true;
        }
        else
        {
            playerLocomotionManager.isSprinting = false;
            playerManager.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotionManager.HandleJumping();
        }
    }

    private void HandleTapRBInput() 
    {
       if(rb_Input)
       {
            playerManager.UpdateWhichHandCharacterIsUsing(true);
            playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
            playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(playerManager); 
       }
    }

    private void HandleTapRTInput()
    {
        if (rt_input)
        {
            playerManager.UpdateWhichHandCharacterIsUsing(true);
            playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
            playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(playerManager);
        }
    }

    private void HandleTapLTInput()
    {
        if (lt_Input)
        {
            if (playerManager.isTwoHandingWeapon)
            {
                //It will be the right handed weapon
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(playerManager);
            }
            else
            {
                playerManager.UpdateWhichHandCharacterIsUsing(false);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(playerManager);
            }
        }
    }

    private void HandleHoldLBInput()
    {
        if (playerManager.isFiringSpell) //ADD IN AIR AND SPRINTING BOOLS TOO 
        {
            lb_Input = false;
            return;
        }

        if (lb_Input)
        {
            playerCombatManager.HandleLBAction();
        }
        else if (lb_Input == false)
        {
            if (playerManager.isAiming)
            {
                playerManager.isAiming = false;
                uiManager.crossHair.SetActive(false);
                cameraManager.ResetAimCameraRotations();
            }
            
            if (blockingCollider.blockingCollider.enabled)
            {
                playerManager.isBlocking = false;
                blockingCollider.DisableBlockingCollider();
            }
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (d_Pad_Right)
        {
            playerInventoryManager.changeRightWeapon();
        }
        else if(d_Pad_Left)
        {
            playerInventoryManager.changeLeftWeapon();
        }
    }

    private void HandleInventoryInput()
    {
        playerControls.PlayerActions.Inventory.performed += i => inventory_Input = true;

        if (inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                uiManager.OpenSelectWindow();
                uiManager.UpdateUI();
                uiManager.hudWindow.SetActive(false);
            }
            else
            {
                uiManager.CloseSelectWindow();
                uiManager.CloseAllInventoryWindows();
                uiManager.hudWindow.SetActive(true);
            }
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            cameraManager.HandleLockOn();
            if (cameraManager.nearestLockOnTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)
        {
            lockOnInput = false;
            lockOnFlag = false;
            cameraManager.ClearLockOnTargets();
        }

        if (lockOnFlag && right_Stick_Left_Input)
        {
            right_Stick_Left_Input = false;
            cameraManager.HandleLockOn();
            if (cameraManager.leftLockOnTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.leftLockOnTarget;
            }
        }

        if (lockOnFlag && right_Stick_Right_Input)
        {
            right_Stick_Right_Input = false;
            cameraManager.HandleLockOn();
            if(cameraManager.rightLockOnTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.rightLockOnTarget; 
            }
        }

        cameraManager.SetCameraHeight();
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
                playerManager.isTwoHandingWeapon = true;
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                weaponSlotManager.LoadTwoHandIKTargets(true); //CALLED FROM CHARACTER WEAPON SLOT MANAGER INSTEAD
            }
            else
            {
                playerManager.isTwoHandingWeapon = false;
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                weaponSlotManager.LoadTwoHandIKTargets(false);
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
            playerLocomotionManager.HandleRolling();
        }
    }

    private void HandleHoldRBInput()
    {
        if (hold_rb_Input)
        {
            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
            {
                playerCombatManager.HandleHoldRBAction();
            }
            else
            {
                hold_rb_Input = false;
                playerCombatManager.AttemptBackStabOrRiposte();
            }
        }
    }

    private void HandleFireBowInput()
    {
        if (fireFlag)
        {
            if (playerManager.isHoldingArrow)
            {
                fireFlag = false;
                playerCombatManager.FireArrowAction();
            }
        }
    }

    private void HandleUseConsumableInput()
    {
        if (x_Input)
        {
            x_Input = false;
            // Use Current consumable
            playerInventoryManager.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        }
    }

}
