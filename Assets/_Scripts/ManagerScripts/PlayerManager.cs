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
    }
}
