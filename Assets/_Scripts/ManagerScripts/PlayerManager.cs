using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputManager inputManager;
    CameraManager cameraManager;
    Animator animator;
    AnimatorManager animatorManager;
    PlayerLocomotionManager character;
    PlayerStatsManager playerStatsManager;

    InteractableUI interactableUI;
    public GameObject interactbleUIGameObject; // Shows the player there is a pick up 
    public GameObject itemInteractableGameObject; // Shows what item that was picked up

    public bool isInteracting;
    public bool isUsingRootMotion;

    [Header("Player Flags")]
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        character = GetComponent<PlayerLocomotionManager>();
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
        isUsingRightHand = animator.GetBool("isUsingRightHand");
        isUsingLeftHand = animator.GetBool("isUsingLeftHand");
        isFiringSpell = animator.GetBool("isFiringSpell");
        animator.SetBool("isBlocking", isBlocking);
        animator.SetBool("isDead", playerStatsManager.isDead);

        animatorManager.canRotate = animator.GetBool("canRotate");

        CheckForInteractableObject();
    }


    private void FixedUpdate()
    {
        character.HandleAllMovement();
        character.HandleRotation();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        character.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", character.isGrounded);

        inputManager.attack_Input = false;
        inputManager.Heavy_attack_Input = false;
        inputManager.lt_Input = false;
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
        character.playerRigidbody.velocity = Vector3.zero; // Stops the player from skating
        transform.position = playerStandsHereWhenOpeningChest.transform.position;
        animatorManager.PlayTargetAnimation("Open Chest", true, true);
    }

    public void PassThroughFogWallInteraction(Transform fogWallEntrance)
    {
        //Make sure we facing the wall first
        character.playerRigidbody.velocity = Vector3.zero;

        Vector3 rotationDirection = fogWallEntrance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;
        //Rotate over time so it does not look rigid

        animatorManager.PlayTargetAnimation("Pass Through Fog", true);
    }

    #endregion
}
