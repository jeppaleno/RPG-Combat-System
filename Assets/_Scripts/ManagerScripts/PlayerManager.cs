using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    Animator animator;

    public CameraManager cameraManager;
    public InputManager inputManager;
    public UIManager uiManager;
    public PlayerStatsManager playerStatsManager;
    public PlayerWeaponSlotManager playerWeaponSlotManager;
    public PlayerEquipmentManager playerEquipmentManager;
    public PlayerCombatManager playerCombatManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerEffectsManager playerEffectsManager;

    InteractableUI interactableUI;
    public GameObject interactbleUIGameObject; // Shows the player there is a pick up 
    public GameObject itemInteractableGameObject; // Shows what item that was picked up

    protected override void Awake()
    {
        base.Awake();
        inputManager = GetComponent<InputManager>();
        uiManager = FindObjectOfType<UIManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
    }

    void Start()
    {
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        inputManager.HandleAllInputs();

        inputManager.isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        canRotate = animator.GetBool("canRotate");
        isFiringSpell = animator.GetBool("isFiringSpell");
        isHoldingArrow = animator.GetBool("isHoldingArrow");
        animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        animator.SetBool("isBlocking", isBlocking);
        animator.SetBool("isDead", playerStatsManager.isDead);

        CheckForInteractableObject();
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        playerLocomotionManager.HandleAllMovement();
        playerLocomotionManager.HandleRotation();
        playerEffectsManager.HandleAllBuildEffects();

    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        playerLocomotionManager.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotionManager.isGrounded);

       
        inputManager.d_Pad_Up = false;
        inputManager.d_Pad_Down = false;
        inputManager.d_Pad_Left = false;
        inputManager.d_Pad_Right = false;
        inputManager.a_Input = false;
        inputManager.inventory_Input = false;
    }

    #region Player Interactions
    public void CheckForInteractableObject()
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f)) //BUG Fix later - add ignore camera layers
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.interactbleText;
                    interactableUI.interactableText.text = interactableText;  //Set the ui text to the interactebla objects text
                    interactbleUIGameObject.SetActive(true); //set the text pop to true
                    if (inputManager.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if (interactbleUIGameObject != null)
            {
                interactbleUIGameObject.SetActive(false);
            }

            if (itemInteractableGameObject != null && inputManager.a_Input)
            {
                itemInteractableGameObject.SetActive(false);
            }
        }
    }

    public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
    {
        playerLocomotionManager.playerRigidbody.velocity = Vector3.zero; // Stops the player from skating
        transform.position = playerStandsHereWhenOpeningChest.transform.position;
        playerAnimatorManager.PlayTargetAnimation("Open Chest", true, true);
    }

    public void PassThroughFogWallInteraction(Transform fogWallEntrance)
    {
        //Make sure we facing the wall first
        playerLocomotionManager.playerRigidbody.velocity = Vector3.zero;

        Vector3 rotationDirection = fogWallEntrance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;
        //Rotate over time so it does not look rigid

        playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
    }

    #endregion
}
