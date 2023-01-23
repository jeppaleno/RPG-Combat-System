using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    Character character;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    WeaponSlotManager weaponSlotManager;
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
    public bool attack_Input;
    public bool Heavy_attack_Input;

    public bool comboFlag;
    public bool twohandFlag;
    public bool lockOnFlag;
    public bool inventoryFlag;
    #endregion

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        character = GetComponent<Character>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
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

            playerControls.PlayerActions.DPadRight.performed += i => d_Pad_Right = true;
            playerControls.PlayerActions.DPadLeft.performed += i => d_Pad_Left = true;

            playerControls.PlayerActions.Inventory.performed += i => inventory_Input = true;

            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;

            playerControls.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            playerControls.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
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
        HandleAttackInput();
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleDodgeInput();
        HandleTwoHandInput();
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

    private void HandleAttackInput()
    {
       if(attack_Input)
        {
            if(playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }         
        }

       if(Heavy_attack_Input)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (d_Pad_Right)
        {
            playerInventory.changeRightWeapon();
        }
        else if(d_Pad_Left)
        {
            playerInventory.changeLeftWeapon();
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

    private void HandleDodgeInput()
    {
        if (x_Input)
        {
            x_Input = false;
            character.HandleDodge();
        }
    }

    private void HandleTwoHandInput()
    {
        if (y_Input)
        {
            y_Input = false;

            twohandFlag = !twohandFlag;

            if (twohandFlag)
            {
                //Enable two handing
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                //Disable two handing
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            }
        }
    }

}
