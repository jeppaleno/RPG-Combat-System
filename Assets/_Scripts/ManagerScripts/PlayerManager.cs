using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    Animator animator;
    Character character;

    public bool isInteracting;

    public bool canDoCombo;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
    }

    void Update()
    {
        inputManager.HandleAllInputs();

        canDoCombo = animator.GetBool("canDoCombo");

        CheckForInteractableObject();
    }


    private void FixedUpdate()
    {
        character.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        character.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", character.isGrounded);

        inputManager.attack_Input = false;
        inputManager.Heavy_attack_Input = false;
        inputManager.d_Pad_Up = false;
        inputManager.d_Pad_Down = false;
        inputManager.d_Pad_Left = false;
        inputManager.d_Pad_Right = false;
        inputManager.a_Input = false;
    }

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
                    //Set the ui text to the interactebla objects text
                    //set the text pop to true
                    if(inputManager.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
    }
}
