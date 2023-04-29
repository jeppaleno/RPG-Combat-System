using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    PlayerManager player;

    Vector3 moveDirection;
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
        player = GetComponent<PlayerManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (player.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        Vector3 cameraForward = Vector3.ProjectOnPlane(player.cameraManager.cameraObject.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(player.cameraManager.cameraObject.transform.right, Vector3.up).normalized;
        moveDirection = cameraForward * player.inputManager.verticalInput;  
        moveDirection = moveDirection + cameraRight * player.inputManager.horizontalInput; 
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            moveDirection = moveDirection * sprintingSpeed;
            player.playerStatsManager.DeductStamina(sprintStaminaCost);
        }
        else
        {
            if (player.inputManager.moveAmount >= 0.5f && !player.isHoldingArrow)
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
        if (player.canRotate)
        {
            if (player.isAiming)
            {
                Quaternion targetRotation = Quaternion.Euler(0, player.cameraManager.cameraTransform.eulerAngles.y, 0);
                Quaternion playerRotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                player.transform.rotation = playerRotation;
            }
            else
            {
                if (player.inputManager.lockOnFlag)
                {
                    if (player.inputManager.sprint_Input)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = player.cameraManager.cameraTransform.forward * player.inputManager.verticalInput;
                        targetDirection += player.cameraManager.cameraTransform.right * player.inputManager.horizontalInput;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = player.transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        player.transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = player.cameraManager.currentLockOnTarget.transform.position - player.transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        player.transform.rotation = targetRotation;
                    }

                }
                else
                {
                    Vector3 targetDirection = Vector3.zero;

                    targetDirection = player.cameraManager.cameraObject.transform.forward * player.inputManager.verticalInput;
                    targetDirection = targetDirection + player.cameraManager.cameraObject.transform.right * player.inputManager.horizontalInput;
                    targetDirection.Normalize();
                    targetDirection.y = 0;


                    // Keeps the looking rotation after the player has stopped
                    if (targetDirection == Vector3.zero)
                        targetDirection = player.transform.forward;

                    // Rotates towards where the player is looking
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion playerRotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    player.transform.rotation = playerRotation;
                }
            }
        }
       
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = player.transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        targetPosition = player.transform.position;

        if (!isGrounded && !isJumping)
        {
            if (!player.isInteracting)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(player.transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer)) 
        {
            if (!isGrounded && !player.isInteracting)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Land", true);
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
            if (player.isInteracting || player.inputManager.moveAmount > 0)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                player.transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        if (isGrounded)
        {
            player.inputManager.jump_Input = false;
            player.animator.SetBool("isJumping", true);
            player.playerAnimatorManager.PlayTargetAnimation("Jump", false);
            player.playerAnimatorManager.EraseHandIKWeapon();

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
        if (player.animator.GetBool("isInteracting"))
            return;

        if (player.inputManager.rollFlag)
        {
            moveDirection = player.cameraManager.cameraObject.transform.forward * player.inputManager.verticalInput;
            moveDirection += player.cameraManager.cameraObject.transform.right * player.inputManager.horizontalInput;

            if (player.inputManager.moveAmount > 0)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Rolling", true, true);
                player.playerAnimatorManager.EraseHandIKWeapon();
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = rollRotation;
                player.playerStatsManager.DeductStamina(2); //Make a rollstaminacost
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation("Dodge", true, true);
                player.playerAnimatorManager.EraseHandIKWeapon();
                player.playerStatsManager.DeductStamina(2); //Make a backstepStaminaCost
            }
        }
    }

}
