using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceStateHumanoid : State
{
    public AttackStateHumanoid attackState;
    public ItemBasedAttackAction[] enemyAttacks;
    public PursueTargetStateHumanoid pursueTargetState;

    protected bool randomDestinationSet = false;
    protected float verticalMovementValue = 0;
    protected float horizontalMovementValue = 0;

    [Header("State Flags")]
    bool willPerformBlock = false;
    bool willPerformDodge = false;
    bool willPerformParry = false;

    bool hasPerformedDodge = false;
    public bool hasPerformedParry = false;
    bool hasRandomDodgeDirection = false;
    bool hasAmmoLoaded = false;

    Quaternion targetDodgeDirection;

    private void Awake()
    {
        attackState = GetComponent<AttackStateHumanoid>();
        pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
    }

    public override State Tick(EnemyManager enemy)
    {
        if (enemy.combatStyle == AICombatStyle.swordAndShield)
        {
            return ProcessSwordAndShieldCombatStyle(enemy);
        }
        else if (enemy.combatStyle == AICombatStyle.archer)
        {
            return ProcessArcherCombatStyle(enemy);
        }
        else
        {
            return this;
        }
    }

    private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
    {
        //If the AAI is falling, or is performing some sort of action STOP all movement
        if (enemy.isInteracting) //ADD !enemy.isGrounded 
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);
            return this;
        }

        //If the AI has gotten too far from it's target, return the AI to it's pursue target state 
        if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
        {
            return pursueTargetState;
        }

        //Randomizes the walking pattern of our AI so they circle the player
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemy.enemyAnimatorManager);
        }

        if (enemy.allowAIToPeformParry)
        {
            if (enemy.currentTarget.canBeRiposted)
            {
                CheckForRiposte(enemy);
                return this;
            }
        }

        if (enemy.allowAIToPerformBlock)
        {
            RollForBlockChance(enemy);
        }

        if (enemy.allowAIToPerformDodge)
        {
            RollForDodgeChance(enemy);
        }

        if (enemy.allowAIToPeformParry)
        {
            RollForParryChance(enemy);
        }

        if (enemy.currentTarget.isAttacking)
        {
            if (willPerformParry && !hasPerformedParry)
            {
                ParryCurrentTarget(enemy);
                return this;
            }
        }

        if (willPerformBlock)
        {
            BlockUsingOffHand(enemy);
        }

        if (willPerformDodge && enemy.currentTarget.isAttacking)
        {
            //IF ENEMY IS ATTACKING THIS AI
            Dodge(enemy);
        }

        HandleRotateTowardstarget(enemy);

        if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null)
        {
            ResetStateFlags();
            return attackState;
        }
        else
        {
            GetNewAttack(enemy);
        }

        CheckIfWeAreTooClose(enemy);

        return this;
    }

    private State ProcessArcherCombatStyle(EnemyManager enemy)
    {
        //If the AAI is falling, or is performing some sort of action STOP all movement
        if (enemy.isInteracting) //ADD !enemy.isGrounded 
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);
            return this;
        }

        //If the AI has gotten too far from it's target, return the AI to it's pursue target state 
        if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
        {
            ResetStateFlags();
            return pursueTargetState;
        }

        //Randomizes the walking pattern of our AI so they circle the player
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemy.enemyAnimatorManager);
        }

        if (enemy.allowAIToPerformDodge)
        {
            RollForDodgeChance(enemy);
        }


        if (willPerformDodge && enemy.currentTarget.isAttacking)
        {
            //IF ENEMY IS ATTACKING THIS AI
            Dodge(enemy);
        }

        HandleRotateTowardstarget(enemy);

        if (!hasAmmoLoaded)
        {
            DrawArrow(enemy);
            AimAtTargetBeforeFiring(enemy);
        }

        if (enemy.currentRecoveryTime <= 0 && hasAmmoLoaded)
        {
            ResetStateFlags();
            return attackState;
        }

        CheckIfWeAreTooClose(enemy);

        return this;
    }

    protected void HandleRotateTowardstarget(EnemyManager enemy)
    {
        // rotate manually
        if (enemy.isPerformingAction)
        {
            Vector3 direction = enemy.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
        }
        //rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemy.navmeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemy.enemyRigidBody.velocity;

            enemy.navmeshAgent.enabled = true;
            enemy.navmeshAgent.SetDestination(enemy.currentTarget.transform.position);
            enemy.enemyRigidBody.velocity = targetVelocity;
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.navmeshAgent.transform.rotation, enemy.rotationSpeed / Time.deltaTime);
        }
    }

    protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
    {
        // Circle with only forward vertical movement
        // circle with running 
        WalkAroundTarget(enemyAnimatorManager);
    }

    protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
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

    protected virtual void GetNewAttack(EnemyManager enemy)
    {
        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

            if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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
    private void RollForBlockChance(EnemyManager enemy)
    {
        int blockChance = Random.Range(0, 100);

        if (blockChance <= enemy.blockLikelyHood)
        {
            willPerformBlock = true;
        }
        else
        {
            willPerformBlock = false;
        }
    }

    private void RollForDodgeChance(EnemyManager enemy)
    {
        int dodgeChance = Random.Range(0, 100);

        if (dodgeChance <= enemy.dodgeLikelyHood)
        {
            willPerformDodge = true;
        }
        else
        {
            willPerformDodge = false;
        }
    }

    private void RollForParryChance(EnemyManager enemy)
    {
        int parryChance = Random.Range(0, 100);

        if (parryChance <= enemy.parryLikelyHood)
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
    private void BlockUsingOffHand(EnemyManager enemy)
    {
        if (enemy.isBlocking == false)
        {
            if (enemy.allowAIToPerformBlock)
            {
                enemy.isBlocking = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.leftWeapon;
                enemy.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
            }
        }
    }

    private void Dodge(EnemyManager enemy)
    {
        if (!hasPerformedDodge)
        {
            if (!hasRandomDodgeDirection)
            {
                float randomDodgeDirection;

                hasRandomDodgeDirection = true;
                randomDodgeDirection = Random.Range(0, 360);
                targetDodgeDirection = Quaternion.Euler(enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
            }

            if (enemy.transform.rotation != targetDodgeDirection)
            {
                Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, targetDodgeDirection, 1f);
                enemy.transform.rotation = targetRotation;

                float targetYRotation = targetDodgeDirection.eulerAngles.y;
                float currentYRotation = enemy.transform.eulerAngles.y;
                float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                if (rotationDifference <= 5)
                {
                    hasPerformedDodge = true;
                    enemy.transform.rotation = targetDodgeDirection;
                    enemy.enemyAnimatorManager.PlayTargetAnimation("Rolling", true, true);
                }
            }
        }
    }

    private void DrawArrow(EnemyManager enemy)
    {
        //Must two hand the bow to fire and load it
        if (!enemy.isTwoHandingWeapon)
        {
            enemy.isTwoHandingWeapon = true;
            enemy.characterWeaponSlotManager.LoadBothWeaponOnSlot();
        }
        else
        {
            hasAmmoLoaded = true;
            enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
            enemy.characterInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(enemy);
        }
    }

    private void AimAtTargetBeforeFiring(EnemyManager enemy)
    {
        float timeUntilAmmoIsShotAtTarget = Random.Range(enemy.minimumTimeToAimAtTarget, enemy.maximumTimeToAimAtTarget);
        enemy.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
    }

    private void ParryCurrentTarget(EnemyManager enemy)
    {
        if (enemy.currentTarget.canBeParried)
        {
            if (enemy.distanceFromTarget <= 2)
            {
                hasPerformedParry = true;
                enemy.isParrying = true;
                enemy.enemyAnimatorManager.PlayTargetAnimation("Parry", true, true);
            }
        }
    }

    private void CheckForRiposte(EnemyManager enemy)
    {
        if (enemy.isInteracting)
        {
            enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return;
        }
        if (enemy.distanceFromTarget >= 1.0)
        {
            HandleRotateTowardstarget(enemy);
            enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
        }
        else
        {
            enemy.isBlocking = false;

            if (!enemy.isInteracting && !enemy.currentTarget.isBeingRiposted && !enemy.currentTarget.isBeingRiposted)
            {
                enemy.enemyRigidBody.velocity = Vector3.zero;
                enemy.animator.SetFloat("Vertical", 0);
                enemy.characterCombatManager.AttemptBackStabOrRiposte();
            }
        }
    }

    private void CheckIfWeAreTooClose(EnemyManager enemy)
    {
        if (enemy.distanceFromTarget <= enemy.stoppingDistance)
        {
            enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime); //Halts forward movement
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime); //Continue side strafing
        }
        else
        {
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
        }
    }
}
