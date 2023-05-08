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
        enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
        enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

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

        if (willPerformBlock)
        {
            BlockUsingOffHand(enemy);
        }

        if (willPerformDodge)
        {
            //IF ENEMY IS ATTACKING THIS AI
            //PERFORM DODGE
        }

        if (willPerformParry)
        {
            //IF WE THINK THE ENEMY IS GOIND TO ATTACK AND THEY ARE PARRYABLE
            //PERFORM PARRY
        }

        //Randomizes the walking pattern of our AI so they circle the player
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemy.enemyAnimatorManager);
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
        return this;
    }

    private State ProcessArcherCombatStyle(EnemyManager enemy)
    {
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
}
