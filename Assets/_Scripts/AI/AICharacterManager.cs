using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    public AICharacterBossManager aiCharacterBossManager;
    public AICharacterLocomotionManager aiCharacterLocomotionManager;
    public AICharacterAnimatorManager aiCharacterAnimatorManager;
    public AICharacterStatsManager aiCharacterStatsManager;
    public AICharacterEffectsManager aiCharacterEffectsManager;
    
    public State currentState;
    public CharacterManager currentTarget;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public bool isPerformingAction;
    public float rotationSpeed = 100; // OG is 15
    public float maximumAggroRadius = 1.5f;

    
    [Header("A.I Settings")]
    public float detectionRadius = 20;
    //The higher, and lower, respectively these angles are, the greater detection field of view (like eye sight)
    public float maximumDetectionAngle = 80;
    public float minimumDetectionAngle = -80;
    public float currentRecoveryTime = 1;
    public float stoppingDistance = 2.5f; //Halting FORWARD movement

    //These setting only effect AI with the humanoid states
    [Header("Advanced AI Settings")]
    public bool allowAIToPerformBlock;
    public int blockLikelyHood = 50;    //Numer from 0-100. 100 will generate a block every time, 0 will generate a block 0% of the time.
    public bool allowAIToPerformDodge;
    public int dodgeLikelyHood = 50;    //Numer from 0-100. 100 will generate a dodge every time, 0 will generate a dodge 0% of the time.
    public bool allowAIToPeformParry;
    public int parryLikelyHood = 50;    //Numer from 0-100. 100 will generate a parry every time, 0 will generate a parry 0% of the time.

    [Header("A.I Combat Settings")]
    public bool allowAIToPerformCombos;
    public bool isPhaseShifting;
    public float comboLikelyHood;
    public AICombatStyle combatStyle;

    [Header("A.I Archer Settings")]
    public bool isStationaryArcher;
    public float minimumTimeToAimAtTarget = 3;
    public float maximumTimeToAimAtTarget = 6;

    [Header("A.I Companion Settings")]
    public float maxDistanceFromCompanion;      // Max distance we can go from our companion
    public float minimumDistanceFromCompanion;  // Minimum distance we have to be from our companion
    public float returnDistanceFromCompanion = 2;   // How CLose we get to our companion when we return to them
    public float distanceFromCompanion;
    public CharacterManager companion;

    [Header("A.I Target Information")]
    public float distanceFromTarget;
    public Vector3 targetDirection;
    public float viewableAngle;

    protected override void Awake()
    {
        base.Awake();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        aiCharacterBossManager = GetComponent<AICharacterBossManager>();
        aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();
        aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();
        aiCharacterEffectsManager = GetComponent<AICharacterEffectsManager>();
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
        if (currentTarget != null)
        {
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            targetDirection = currentTarget.transform.position - transform.position;
            viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        }

        if (companion != null)
        {
            distanceFromCompanion = Vector3.Distance(companion.transform.position, transform.position);
        }

        HandleRecoveryTimer();
        HandleStateMachine();



        isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
        isInteracting = animator.GetBool("isInteracting");
        isPhaseShifting = animator.GetBool("isPhaseShifting");
        //isInvulnerable = animator.GetBool("isInvulnerable");
        isHoldingArrow = animator.GetBool("isHoldingArrow");
        canDoCombo = animator.GetBool("canDoCombo");
        canRotate = animator.GetBool("canRotate");
        animator.SetBool("isDead", isDead);
        animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        animator.SetBool("isBlocking", isBlocking);


    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        aiCharacterEffectsManager.HandleAllBuildEffects();
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
            State nextState = currentState.Tick(this);

            if (nextState != null)
            {
                /*if (currentState != nextState)
                {
                    Debug.Log(nextState);
                }*/
            
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
