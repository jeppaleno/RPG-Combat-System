using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocomotionManager character;
    PlayerCombatManager playerCombatManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager playerManager;
    PlayerEffectsManager playerEffectsManager;
    BlockingCollider blockingCollider;
    PlayerWeaponSlotManager weaponSlotManager;
    CameraManager cameraManager;
    UIManager uiManager;
    
    #region Variables
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
    public bool critical_Attack_Input;
    public bool jump_Input;
    public bool sprint_Input;
    public bool attack_Input;
    public bool lb_Input; // Left shoulder/Q
    public bool lt_Input; //Left trigger
    public bool Heavy_attack_Input;

    public bool rollFlag;
    public bool isInteracting;
    public bool comboFlag;
    public bool twohandFlag;
    public bool lockOnFlag;
    public bool inventoryFlag;

    public Transform criticalRayCastStartPoint;
    #endregion

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        character = GetComponent<PlayerLocomotionManager>();
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
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false; //cancels when released

            playerControls.PlayerActions.Attack.performed += i => attack_Input = true;
            playerControls.PlayerActions.HeavyAttack.performed += i => Heavy_attack_Input = true;

            playerControls.PlayerActions.LB.performed += i => lb_Input = true;
            playerControls.PlayerActions.LB.canceled += i => lb_Input = false;

            playerControls.PlayerActions.LT.performed += i => lt_Input = true;

            playerControls.PlayerActions.DPadRight.performed += i => d_Pad_Right = true;
            playerControls.PlayerActions.DPadLeft.performed += i => d_Pad_Left = true;

            playerControls.PlayerActions.Inventory.performed += i => inventory_Input = true;

            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;

            playerControls.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            playerControls.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;

            playerControls.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;

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
        HandleCombatInput();
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        //HandleDodgeInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
        HandleRollInput();
        HandleUseConsumableInput();
    }

    private void HandleMovementInput()
    {
        
        verticalInput = movementInput.y; //Vertical value of joystick
        horizontalInput = movementInput.x; //Horizontal value of joystick

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        // combines value of horizontal and vertical inputs from the joystick
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        if (lockOnFlag && sprint_Input == false)
        {
            animatorManager.UpdateAnimatorValues(horizontalInput, verticalInput, character.isSprinting);
        }
        else
        {
            animatorManager.UpdateAnimatorValues(0, moveAmount, character.isSprinting);
        }
        
    }

    private void HandleSprintingInput() 
    {
        // Sprints when input is being pressed and aleady is running. 
        if (sprint_Input && moveAmount > 0.5f)
        {
            character.isSprinting = true;
        }
        else
        {
            character.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            character.HandleJumping();
        }
    }

    private void HandleCombatInput() // (float delta)?
    {
       if(attack_Input)
        {
            playerCombatManager.HandleAttackAction();     
        }

       if(Heavy_attack_Input)
        {
            playerCombatManager.HandleHeavyAttack(playerInventoryManager.rightWeapon);
        }

       if (lb_Input)
        {
            playerCombatManager.HandleLBAction();
        }
       else
        {
            playerManager.isBlocking = false;

            if (blockingCollider.blockingCollider.enabled)
            {
                blockingCollider.DisableBlockingCollider();
            }
        }

       if (lt_Input)
        {
            if (twohandFlag)
            {
                // if two handing handle weapon art
            }
            else
            {
                // else handle light attack if melee weapon
                playerCombatManager.HandleLTAction();
            }
            //handle weapon art if shield
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
                //Enable two handing
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
            }
            else
            {
                //Disable two handing
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if (critical_Attack_Input)
        {
            critical_Attack_Input = false;
            playerCombatManager.AttemptBackStabOrRiposte();
        }
    }

    private void HandleRollInput()
    {
        if (b_input)
        {
            rollFlag = true;
            b_input = false;
            character.HandleRolling();
        }
    }

    private void HandleUseConsumableInput()
    {
        if (x_Input)
        {
            x_Input = false;
            // Use Current consumable
            playerInventoryManager.currentConsumable.AttemptToConsumeItem(animatorManager, weaponSlotManager, playerEffectsManager);
        }
    }

}
