using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;

   
    public Transform targetTransform; // The transform the camera will follow
    public Transform targetTransformWhileAiming;
    public Transform cameraTransform;
    public Camera cameraObject;
    public Transform cameraPivot;     //The transform the camera uses to pivot (Look up and down)
    public LayerMask environmentLayer;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public float cameraFollowSpeed = 1f;
    public float leftAndRightLookSpeed = 250f;
    public float leftAndRightAimingLookSpeed = 25f;
    public float upAndDownLookSpeed = 250f;
    public float upAndDownAimingLookSpeed = 25f;

    public float leftAndRightAngle; 
    public float upAndDownAngle; 
    public float minimumLookUpAngle = -35;
    public float maximumLookUpAngle = 35;
    public float lockedPivotPosition = 2.25f; // Height on camera angle while locked on target
    public float unlockedPivotPosition = 1.65f;

    public CharacterManager currentLockOnTarget;

    List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager rightLockOnTarget;
    public float maximumLockOnDistance = 30;


    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        playerManager = FindObjectOfType<PlayerManager>();
        cameraObject = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        environmentLayer = LayerMask.NameToLayer("Environment");
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        HandleCameraRotation();
    }

    //FOLLOW THE PLAYER
    private void FollowTarget()
    {
        if (playerManager.isAiming)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileAiming.position, ref cameraFollowVelocity, Time.deltaTime * cameraFollowSpeed);
            transform.position = targetPosition;
        }
        else
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, Time.deltaTime * cameraFollowSpeed);
            transform.position = targetPosition;
        }
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
        Vector3 rotation;

        leftAndRightAngle = leftAndRightAngle + (inputManager.cameraInputX * leftAndRightLookSpeed) * Time.deltaTime;
        upAndDownAngle = upAndDownAngle - (inputManager.cameraInputY * upAndDownLookSpeed) * Time.deltaTime;
        upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimumLookUpAngle, maximumLookUpAngle);

        rotation = Vector3.zero;
        rotation.y = leftAndRightAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = upAndDownAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

    }

    private void HandleLockedCameraRotation()
    {
        Vector3 dir = currentLockOnTarget.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = targetRotation;

        dir = currentLockOnTarget.transform.position - cameraPivot.position;
        dir.Normalize();

        targetRotation = Quaternion.LookRotation(dir);
        Vector3 eulerAngle = targetRotation.eulerAngles;
        eulerAngle.y = 0;
        cameraPivot.localEulerAngles = eulerAngle;
    }

    private void HandleAimedCameraRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraPivot.rotation = Quaternion.Euler(0, 0, 0);

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

                if (character.transform.root != targetTransform.transform.transform.root 
                    && viewableAngle > -50 && viewableAngle < 50 
                    && distanceFromTarget <= maximumLockOnDistance)
                {
                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                        if (hit.transform.gameObject.layer == environmentLayer)
                        {
                            //Can't lock on to target when objects in the way
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
                    leftLockOnTarget = availableTargets[k];
                }

                else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                    && availableTargets[k] != currentLockOnTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockOnTarget = availableTargets[k];
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
            cameraPivot.transform.localPosition = Vector3.SmoothDamp(cameraPivot.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivot.transform.localPosition = Vector3.SmoothDamp(cameraPivot.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}
