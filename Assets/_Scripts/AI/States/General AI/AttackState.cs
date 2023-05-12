using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public RotateTowardsTargetState rotateTowardsTargetState;
    public CombatStanceState combatStanceState;
    public PursueTargetState pursueTargetState;
    public AICharacterAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;

    public override State Tick(AICharacterManager enemy)
    {
        float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

        RotateTowardsTargetWhilstAttacking(enemy);

        if (distanceFromTarget > enemy.maximumAggroRadius)
        {
            return pursueTargetState;
        }

        if (willDoComboOnNextAttack && enemy.canDoCombo)
        {
            AttackTargetWithCombo(enemy);
        }

        if (!hasPerformedAttack)
        {
            AttackTarget(enemy);
            RollForComboChance(enemy);
        }

        if (willDoComboOnNextAttack && hasPerformedAttack)
        {
            return this; //Goes back up to perform the combo
        }

        return rotateTowardsTargetState;
    }

    private void AttackTarget(AICharacterManager enemy)
    {
        enemy.isUsingRightHand = currentAttack.isRightHandedAction;
        enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;
        enemy.aiCharacterAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemy.aiCharacterAnimatorManager.PlayWeaponTrailFX();
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }
    
    private void AttackTargetWithCombo(AICharacterManager enemy)
    {
        enemy.isUsingRightHand = currentAttack.isRightHandedAction;
        enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;
        willDoComboOnNextAttack = false;
        enemy.aiCharacterAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemy.aiCharacterAnimatorManager.PlayWeaponTrailFX();
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhilstAttacking(AICharacterManager enemyManager)
    {
        // rotate manually
        if (enemyManager.canRotate && enemyManager.isInteracting)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }

    private void RollForComboChance(AICharacterManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
        {
            if (currentAttack.comboAction != null)
            {
                willDoComboOnNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }
        }
    }
}
