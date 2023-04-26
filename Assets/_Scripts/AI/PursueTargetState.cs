using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public RotateTowardsTargetState rotateTowardsTargetState;

    public override State Tick(EnemyManager enemy)
    {
        Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
        float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

        HandleRotateTowardstarget(enemy);

        if (enemy.isInteracting)
            return this;

        if (enemy.isPerformingAction)
        {
           
            enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }
            
        if (distanceFromTarget > enemy.maximumAggroRadius)
        {
            enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if (distanceFromTarget <= enemy.maximumAggroRadius)
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
