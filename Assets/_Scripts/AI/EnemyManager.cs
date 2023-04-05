using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimationManager;
    EnemyStatsManager enemyStatsManager;
    EnemyEffectsManager enemyEffectsManager;
    
    public State currentState;
    public CharacterStatsManager currentTarget;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public bool isPerformingAction;
    public float rotationSpeed = 15;
    public float maximumAggroRadius = 1.5f;

    
    [Header("A.I Settings")]
    public float detectionRadius = 20;
    //The higher, and lower, respectively these angles are, the greater detection field of view (like eye sight)
    public float maximumDetectionAngle = 80;
    public float minimumDetectionAngle = -80;
    public float currentRecoveryTime = 0;

    [Header("A.I Combat Settings")]
    public bool allowAIToPerformCombos;
    public bool isPhaseShifting;
    public float comboLikelyHood;

    public void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimationManager = GetComponent<EnemyAnimatorManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        navmeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidBody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isUsingLeftHand = enemyAnimationManager.animator.GetBool("isUsingLeftHand");
        isUsingRightHand = enemyAnimationManager.animator.GetBool("isUsingRightHand");
        isRotatingWithRootMotion = enemyAnimationManager.animator.GetBool("isRotatingWithRootMotion");
        isInteracting = enemyAnimationManager.animator.GetBool("isInteracting");
        isPhaseShifting = enemyAnimationManager.animator.GetBool("isPhaseShifting");
        //isInvulnerable = enemyAnimationManager.animator.GetBool("isInvulnerable");
        canDoCombo = enemyAnimationManager.animator.GetBool("canDoCombo");
        canRotate = enemyAnimationManager.animator.GetBool("canRotate");
        enemyAnimationManager.animator.SetBool("isDead", enemyStatsManager.isDead);
    }

    private void FixedUpdate()
    {
        enemyEffectsManager.HandleAllBuildEffects();
    }

    private void LateUpdate()
    {
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
       if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimationManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }
    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }
}
