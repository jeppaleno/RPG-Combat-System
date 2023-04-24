using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    
    CameraManager cameraManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerAnimatorManager playerAnimatorManager;
    InputManager inputManager;
    
    Vector3 moveDirection;
    Transform cameraObject;
    public Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;
    public float groundDistance = 0.5f;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isJumping;
    public bool isSprinting;

    [Header("Movement Speeds")]
    public float walkingSpeed = 3;
    public float runningSpeed = 6;
    public float sprintingSpeed = 8;
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    [Header("Stamina Costs")]
    [SerializeField]
    //int rollStaminaCost = 15;
    //int backstepStaminaCost = 12;
    int sprintStaminaCost = 1;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        cameraManager = FindObjectOfType<CameraManager>();
    }

    void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraObject.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraObject.right, Vector3.up).normalized;
        moveDirection = cameraForward * inputManager.verticalInput;  //Forward Movement
        moveDirection = moveDirection + cameraRight * inputManager.horizontalInput; //Left/Right Movement
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            moveDirection = moveDirection * sprintingSpeed;
            playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f && !playerManager.isHoldingArrow)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }

        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    public void HandleRotation()
    {
        if (playerManager.canRotate)
        {
            if (playerManager.isAiming)
            {
                Quaternion targetRotation = Quaternion.Euler(0, cameraManager.cameraTransform.eulerAngles.y, 0);
                Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = playerRotation;
            }
            else
            {
                if (inputManager.lockOnFlag)
                {
                    if (inputManager.sprint_Input)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraManager.cameraTransform.forward * inputManager.verticalInput;
                        targetDirection += cameraManager.cameraTransform.right * inputManager.horizontalInput;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraManager.currentLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }

                }
                else
                {
                    Vector3 targetDirection = Vector3.zero;

                    targetDirection = cameraObject.forward * inputManager.verticalInput;
                    targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
                    targetDirection.Normalize();
                    targetDirection.y = 0;


                    // Keeps the looking rotation after the player has stopped
                    if (targetDirection == Vector3.zero)
                        targetDirection = transform.forward;

                    // Rotates towards where the player is looking
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    transform.rotation = playerRotation;
                }
            }
        }
       
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        targetPosition = transform.position;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                playerAnimatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer)) 
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                playerAnimatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        if (isGrounded)
        {
            
            playerAnimatorManager.animator.SetBool("isJumping", true);
            playerAnimatorManager.PlayTargetAnimation("Jump", false);
            playerAnimatorManager.EraseHandIKWeapon();

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity; //Sets the velocity of the rigidbody to the new velocity 
        }
    }

    /*public void HandleDodge()
    {
        if (playerManager.isInteracting) // Can't dodge if we're doing something else
            return;

        animatorManager.PlayTargetAnimation("Dodge", true, true);
        playerStats.TakeStaminaDamage(backstepStaminaCost);
    }*/

    public void HandleRolling()
    {
        if (playerAnimatorManager.animator.GetBool("isInteracting"))
            return;

        if (inputManager.rollFlag)
        {
            moveDirection = cameraObject.forward * inputManager.verticalInput;
            moveDirection += cameraObject.right * inputManager.horizontalInput;

            if (inputManager.moveAmount > 0)
            {
                playerAnimatorManager.PlayTargetAnimation("Rolling", true, true);
                playerAnimatorManager.EraseHandIKWeapon();
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = rollRotation;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Dodge", true, true);
                playerAnimatorManager.EraseHandIKWeapon();
            }
        }
    }

}
