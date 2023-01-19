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
    
    #region Values
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount; 
    public float verticalInput;
    public float horizontalInput;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool jump_Input;
    public bool sprint_Input;
    public bool attack_Input;
    public bool Heavy_attack_Input;
    public bool comboFlag;
    #endregion

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        character = GetComponent<Character>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
     
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false; //cancels when released

            playerControls.PlayerActions.Attack.performed += i => attack_Input = true;
            playerControls.PlayerActions.HeavyAttack.performed += i => Heavy_attack_Input = true;
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
    }

    private void HandleMovementInput()
    {
        
        verticalInput = movementInput.y; //Vertical value of joystick
        horizontalInput = movementInput.x; //Horizontal value of joystick

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        // combines value of horizontal and vertical inputs from the joystick
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, character.isSprinting);
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
        playerControls.PlayerActions.DPadRight.performed += i => d_Pad_Right = true;
        playerControls.PlayerActions.DPadLeft.performed += i => d_Pad_Left = true;
        if (d_Pad_Right)
        {
            playerInventory.changeRightWeapon();
        }
        else if(d_Pad_Left)
        {
            playerInventory.changeLeftWeapon();
        }
    }

}
