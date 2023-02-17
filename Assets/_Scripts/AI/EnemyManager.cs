using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimationManager;
    EnemyStats enemyStats;
    
    public State currentState;
    public CharacterStats currentTarget;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public bool isPerformingAction;
    public bool isInteracting;   
    public float rotationSpeed = 15;
    public float maximumAttackRange = 1.5f;

    [Header("A.I Settings")]
    public float detectionRadius = 20;
    //The higher, and lower, respectively these angles are, the greater detection field of view (like eye sight)
    public float maximumDetectionAngle = 80;
    public float minimumDetectionAngle = -80;

    public float currentRecoveryTime = 0;

    public void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimationManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
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

        isInteracting = enemyAnimationManager.animator.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
       if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);

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
