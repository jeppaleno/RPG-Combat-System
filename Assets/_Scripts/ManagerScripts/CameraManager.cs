using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;

    public Transform targetTransform;               //The transform the camera follows (The Player)
    public Transform targetTransformWhileAiming;    //The transform the camera follows while aiming
    public Transform cameraTransform;
    public Camera cameraObject;
    public Transform cameraPivotTransform;

    private Vector3 cameraTransformPosition;
    public LayerMask ignoreLayers;
    public LayerMask enviromentLayer;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public float leftAndRightLookSpeed = 250f;
    public float leftAndRightAimingLookSpeed = 25f;
    public float groundedFollowSpeed = 20f;
    public float aerialFollowSpeed = 1f;    //  THE LOWER THIS IS, THE FASTER IT WILL FOLLOW THE PLAYER
    public float upAndDownLookSpeed = 250f;
    public float upAndDownAimingLookSpeed = 25f;

    private float targetPosition;
    private float defaultPosition;

    private float leftAndRightAngle;
    private float upAndDownAngle;

    public float minimumLookUpAngle = -35;
    public float maximumLookUpAngle = 35;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public float lockedPivotPosition = 2.25f;
    public float unlockedPivotPosition = 1.65f;

    public CharacterManager currentLockOnTarget;

    List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockTarget;
    public CharacterManager rightLockTarget;
    public float maximumLockOnDistance = 30;


    private void Awake()
    {
        defaultPosition = cameraTransform.localPosition.z;
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        cameraObject = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        enviromentLayer = LayerMask.NameToLayer("Environment");
    }

    //FOLLOW THE PLAYER
    public void FollowTarget()
    {
        if (playerManager.isAiming)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileAiming.position, ref cameraFollowVelocity, groundedFollowSpeed * Time.deltaTime);
            transform.position = targetPosition;
        }
        else
        {
            if (playerManager.isGrounded)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, groundedFollowSpeed * Time.deltaTime);
                transform.position = targetPosition;
            }
            else
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, aerialFollowSpeed * Time.deltaTime);
                transform.position = targetPosition;
            }
        }

        HandleCameraCollisions();
    }


    //ROTATE THE CAMERA
    public void HandleCameraRotation()
    {
        if (inputManager.lockOnFlag && currentLockOnTarget != null)
        {
            HandleLockedCameraRotation();
        }
        else if (playerManager.isAiming)
        {
            HandleAimedCameraRotation();
        }
        else
        {
            HandleStandardCameraRotation();
        }
    }

    public void HandleStandardCameraRotation()
    {
        leftAndRightAngle += inputManager.cameraInputX * leftAndRightLookSpeed * Time.deltaTime;
        upAndDownAngle -= inputManager.cameraInputY * upAndDownLookSpeed * Time.deltaTime;
        upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimumLookUpAngle, maximumLookUpAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = leftAndRightAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = upAndDownAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleLockedCameraRotation()
    {
        Vector3 dir = currentLockOnTarget.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = targetRotation;

        dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
        dir.Normalize();

        targetRotation = Quaternion.LookRotation(dir);
        Vector3 eulerAngle = targetRotation.eulerAngles;
        eulerAngle.y = 0;
        cameraPivotTransform.localEulerAngles = eulerAngle;
    }

    private void HandleAimedCameraRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);

        Quaternion targetRotationX;
        Quaternion targetRotationY;

        Vector3 cameraRotationX = Vector3.zero;
        Vector3 cameraRotationY = Vector3.zero;

        leftAndRightAngle += (inputManager.cameraInputX * leftAndRightAimingLookSpeed) * Time.deltaTime;
        upAndDownAngle -= (inputManager.cameraInputY * upAndDownAimingLookSpeed) * Time.deltaTime;

        cameraRotationY.y = leftAndRightAngle;
        targetRotationY = Quaternion.Euler(cameraRotationY);
        targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
        transform.localRotation = targetRotationY;

        cameraRotationX.x = upAndDownAngle;
        targetRotationX = Quaternion.Euler(cameraRotationX);
        targetRotationX = Quaternion.Slerp(cameraTransform.localRotation, targetRotationX, 1);
        cameraTransform.localRotation = targetRotationX;
    }

    public void ResetAimCameraRotations()
    {
        cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);
    }


    //HANDLE COLLISIONS
    private void HandleCameraCollisions()
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition)
            , ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffSet);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }


    //HANDLE LOCK ON
    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;

                if (character.transform.root != targetTransform.transform.root
                    && viewableAngle > -50 && viewableAngle < 50
                    && distanceFromTarget <= maximumLockOnDistance)
                {
                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                        if (hit.transform.gameObject.layer == enviromentLayer)
                        {
                            //Cannot lock onto target, object in the way
                        }
                        else
                        {
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        for (int k = 0; k < availableTargets.Count; k++)
        {
            float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

            if (distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[k];
            }

            if (inputManager.lockOnFlag)
            {
                //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
                //var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                //var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;
                Vector3 relativeEnemyPosition = inputManager.transform.InverseTransformPoint(availableTargets[k].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;

                if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget
                    && availableTargets[k] != currentLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTargets[k];
                }

                else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                    && availableTargets[k] != currentLockOnTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = availableTargets[k];
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

        if (currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }


}
