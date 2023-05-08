using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateHumanoid : State
{
    public RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
    public CombatStanceStateHumanoid combatStanceState;
    public PursueTargetStateHumanoid pursueTargetState;
    public ItemBasedAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;

    public override State Tick(EnemyManager enemy)
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

        ResetStateFlags();
        return rotateTowardsTargetState;
    }

    private void AttackTarget(EnemyManager enemy)
    {
        currentAttack.PerformAttackAction(enemy);
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemy)
    {
        currentAttack.PerformAttackAction(enemy);
        willDoComboOnNextAttack = false;
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
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

    private void RollForComboChance(EnemyManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
        {
            if (currentAttack.actionCanCombo)
            {
                willDoComboOnNextAttack = true;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }
        }
    }

    private void ResetStateFlags()
    {
        willDoComboOnNextAttack = false;
        hasPerformedAttack = false;
    }
}
