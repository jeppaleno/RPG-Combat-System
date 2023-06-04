using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateCombatStance : State
{
    public ItemBasedAttackAction[] enemyAttacks;

    CompanionStateFollowHost followHostState;
    CompanionStatePursueTarget pursueTargetState;
    CompanionStateAttackTarget attackState;

    protected bool randomDestinationSet = false;
    protected float verticalMovementValue = 0;
    protected float horizontalMovementValue = 0;

    [Header("State Flags")]
    public bool willPerformBlock = false;
    public bool willPerformDodge = false;
    public bool willPerformParry = false;

    public bool hasPerformedDodge = false;
    public bool hasPerformedParry = false;
    public bool hasRandomDodgeDirection = false;
    public bool hasAmmoLoaded = false;

    Quaternion targetDodgeDirection;

    private void Awake()
    {
        attackState = GetComponent<CompanionStateAttackTarget>();
        pursueTargetState = GetComponent<CompanionStatePursueTarget>();
        followHostState = GetComponent<CompanionStateFollowHost>();
    }

    public override State Tick(AICharacterManager aiCharacter)
    {
        // If we are too far away from our companion, we return to them
        if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
        {
            return followHostState;
        }

        if (aiCharacter.combatStyle == AICombatStyle.swordAndShield)
        {
            return ProcessSwordAndShieldCombatStyle(aiCharacter);
        }
        else if (aiCharacter.combatStyle == AICombatStyle.archer)
        {
            return ProcessArcherCombatStyle(aiCharacter);
        }
        else
        {
            return this;
        }
    }

    private State ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacter)
    {
        //If the AAI is falling, or is performing some sort of action STOP all movement
        if (aiCharacter.isInteracting) //ADD !enemy.isGrounded 
        {
            aiCharacter.animator.SetFloat("Vertical", 0);
            aiCharacter.animator.SetFloat("Horizontal", 0);
            return this;
        }

        //If the AI has gotten too far from it's target, return the AI to it's pursue target state 
        if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
        {
            return pursueTargetState;
        }

        //Randomizes the walking pattern of our AI so they circle the player
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager);
        }

        if (aiCharacter.allowAIToPeformParry)
        {
            if (aiCharacter.currentTarget.canBeRiposted)
            {
                CheckForRiposte(aiCharacter);
                return this;
            }
        }

        if (aiCharacter.allowAIToPerformBlock)
        {
            RollForBlockChance(aiCharacter);
        }

        if (aiCharacter.allowAIToPerformDodge)
        {
            RollForDodgeChance(aiCharacter);
        }

        if (aiCharacter.allowAIToPeformParry)
        {
            RollForParryChance(aiCharacter);
        }

        if (aiCharacter.currentTarget.isAttacking)
        {
            if (willPerformParry && !hasPerformedParry)
            {
                ParryCurrentTarget(aiCharacter);
                return this;
            }
        }

        if (willPerformBlock)
        {
            BlockUsingOffHand(aiCharacter);
        }

        if (willPerformDodge && aiCharacter.currentTarget.isAttacking)
        {
            //IF ENEMY IS ATTACKING THIS AI
            Dodge(aiCharacter);
        }

        HandleRotateTowardstarget(aiCharacter);

        if (aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
        {
            ResetStateFlags();
            return attackState;
        }
        else
        {
            GetNewAttack(aiCharacter);
        }

        HandleMovement(aiCharacter);

        return this;
    }

    private State ProcessArcherCombatStyle(AICharacterManager aiCharacter)
    {
        //If the AAI is falling, or is performing some sort of action STOP all movement
        if (aiCharacter.isInteracting) //ADD !enemy.isGrounded 
        {
            aiCharacter.animator.SetFloat("Vertical", 0);
            aiCharacter.animator.SetFloat("Horizontal", 0);
            return this;
        }

        //If the AI has gotten too far from it's target, return the AI to it's pursue target state 
        if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
        {
            ResetStateFlags();
            return pursueTargetState;
        }

        //Randomizes the walking pattern of our AI so they circle the player
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager);
        }

        if (aiCharacter.allowAIToPerformDodge)
        {
            RollForDodgeChance(aiCharacter);
        }


        if (willPerformDodge && aiCharacter.currentTarget.isAttacking)
        {
            //IF ENEMY IS ATTACKING THIS AI
            Dodge(aiCharacter);
        }

        HandleRotateTowardstarget(aiCharacter);

        if (!hasAmmoLoaded)
        {
            DrawArrow(aiCharacter);
            AimAtTargetBeforeFiring(aiCharacter);
        }

        if (aiCharacter.currentRecoveryTime <= 0 && hasAmmoLoaded)
        {
            ResetStateFlags();
            return attackState;
        }

        if (aiCharacter.isStationaryArcher)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
        }
        else
        {
            HandleMovement(aiCharacter);
        }

        return this;
    }

    protected void HandleRotateTowardstarget(AICharacterManager aiCharacter)
    {
        Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
        {
            direction = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed); // / Time.deltaTime?
    }

    protected void DecideCirclingAction(AICharacterAnimatorManager enemyAnimatorManager)
    {
        // Circle with only forward vertical movement
        // circle with running 
        WalkAroundTarget(enemyAnimatorManager);
    }

    protected void WalkAroundTarget(AICharacterAnimatorManager enemyAnimatorManager)
    {
        verticalMovementValue = 0.5f;

        horizontalMovementValue = Random.Range(-1, 1);

        if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
        {
            horizontalMovementValue = 0.5f;
        }
        else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
        {
            horizontalMovementValue = -0.5f;
        }
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

            if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

            if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (attackState.currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        attackState.currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }

    //AI CHANCE ROLLS
    private void RollForBlockChance(AICharacterManager aiCharacter)
    {
        int blockChance = Random.Range(0, 100);

        if (blockChance <= aiCharacter.blockLikelyHood)
        {
            willPerformBlock = true;
        }
        else
        {
            willPerformBlock = false;
        }
    }

    private void RollForDodgeChance(AICharacterManager aiCharacter)
    {
        int dodgeChance = Random.Range(0, 100);

        if (dodgeChance <= aiCharacter.dodgeLikelyHood)
        {
            willPerformDodge = true;
        }
        else
        {
            willPerformDodge = false;
        }
    }

    private void RollForParryChance(AICharacterManager aiCharacter)
    {
        int parryChance = Random.Range(0, 100);

        if (parryChance <= aiCharacter.parryLikelyHood)
        {
            willPerformParry = true;
        }
        else
        {
            willPerformParry = false;
        }
    }

    //CALLED WHEN EVER WE EXIT THIS STATE, SO WHEN WE RETURN ALL FLAGS ARE RESET AND CAN BE RE-ROLLED
    private void ResetStateFlags()
    {
        hasRandomDodgeDirection = false;
        hasPerformedDodge = false;
        hasAmmoLoaded = false;
        hasPerformedParry = false;

        randomDestinationSet = false;

        willPerformBlock = false;
        willPerformDodge = false;
        willPerformParry = false;
    }

    //AI ACTIONS
    private void BlockUsingOffHand(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isBlocking == false)
        {
            if (aiCharacter.allowAIToPerformBlock)
            {
                aiCharacter.isBlocking = true;
                aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.leftWeapon;
                aiCharacter.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
            }
        }
    }

    private void Dodge(AICharacterManager aiCharacter)
    {
        if (!hasPerformedDodge)
        {
            if (!hasRandomDodgeDirection)
            {
                float randomDodgeDirection;

                hasRandomDodgeDirection = true;
                randomDodgeDirection = Random.Range(0, 360);
                targetDodgeDirection = Quaternion.Euler(aiCharacter.transform.eulerAngles.x, randomDodgeDirection, aiCharacter.transform.eulerAngles.z);
            }

            if (aiCharacter.transform.rotation != targetDodgeDirection)
            {
                Quaternion targetRotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetDodgeDirection, 1f);
                aiCharacter.transform.rotation = targetRotation;

                float targetYRotation = targetDodgeDirection.eulerAngles.y;
                float currentYRotation = aiCharacter.transform.eulerAngles.y;
                float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                if (rotationDifference <= 5)
                {
                    hasPerformedDodge = true;
                    aiCharacter.transform.rotation = targetDodgeDirection;
                    aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Rolling", true, true);
                }
            }
        }
    }

    private void DrawArrow(AICharacterManager aiCharacter)
    {
        //Must two hand the bow to fire and load it
        if (!aiCharacter.isTwoHandingWeapon)
        {
            aiCharacter.isTwoHandingWeapon = true;
            aiCharacter.characterWeaponSlotManager.LoadBothWeaponOnSlot();
        }
        else
        {
            hasAmmoLoaded = true;
            aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.rightWeapon;
            aiCharacter.characterInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(aiCharacter);
        }
    }

    private void AimAtTargetBeforeFiring(AICharacterManager aiCharacter)
    {
        float timeUntilAmmoIsShotAtTarget = Random.Range(aiCharacter.minimumTimeToAimAtTarget, aiCharacter.maximumTimeToAimAtTarget);
        aiCharacter.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
    }

    private void ParryCurrentTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.currentTarget.canBeParried)
        {
            if (aiCharacter.distanceFromTarget <= 2)
            {
                hasPerformedParry = true;
                aiCharacter.isParrying = true;
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Parry", true, true);
            }
        }
    }

    private void CheckForRiposte(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isInteracting)
        {
            aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return;
        }
        if (aiCharacter.distanceFromTarget >= 1.0)
        {
            HandleRotateTowardstarget(aiCharacter);
            aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
        }
        else
        {
            aiCharacter.isBlocking = false;

            if (!aiCharacter.isInteracting && !aiCharacter.currentTarget.isBeingRiposted && !aiCharacter.currentTarget.isBeingRiposted)
            {
                aiCharacter.enemyRigidBody.velocity = Vector3.zero;
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.characterCombatManager.AttemptBackStabOrRiposte();
            }
        }
    }

    private void HandleMovement(AICharacterManager aiCharacter)
    {
        if (aiCharacter.distanceFromTarget <= aiCharacter.stoppingDistance)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime); //Halts forward movement
            aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime); //Continue side strafing
        }
        else
        {
            aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
        }
    }
}
