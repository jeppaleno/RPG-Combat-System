using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetStateHumanoid : State
{
    public CombatStanceStateHumanoid combatStanceState;

    private void Awake()
    {
        combatStanceState = GetComponent<CombatStanceStateHumanoid>();
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

    private State ProcessArcherCombatStyle(EnemyManager enemy)
    {
        HandleRotateTowardstarget(enemy);

        if (enemy.isInteracting)
            return this;

        if (enemy.isPerformingAction)
        {

            enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
        {
            if (!enemy.isStationaryArcher)
            {
                enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
        }

        if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }

    private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
    {
        HandleRotateTowardstarget(enemy);

        if (enemy.isInteracting)
            return this;

        if (enemy.isPerformingAction)
        {

            enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
        {
            enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardstarget(EnemyManager enemyManager)
    {
        // rotate manually
        if (enemyManager.isPerformingAction)
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
        //rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

            enemyManager.navmeshAgent.enabled = true;
            enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidBody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
