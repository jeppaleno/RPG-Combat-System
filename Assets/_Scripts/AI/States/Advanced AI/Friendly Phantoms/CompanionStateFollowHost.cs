using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateFollowHost : State
{
    CompanionStateIdle idleState;

    private void Awake()
    {
        idleState = GetComponent<CompanionStateIdle>();
    }

    public override State Tick(AICharacterManager aiCharacter)
    {

        if (aiCharacter.isInteracting)
            return this;

        if (aiCharacter.isPerformingAction)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        HandleRotateTowardstarget(aiCharacter);

        if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
        {
            aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if (aiCharacter.distanceFromCompanion <= aiCharacter.returnDistanceFromCompanion)
        {
            return idleState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardstarget(AICharacterManager aiCharacter)
    {
        Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navmeshAgent.desiredVelocity);
        Vector3 targetVelocity = aiCharacter.enemyRigidBody.velocity;

        aiCharacter.navmeshAgent.enabled = true;
        aiCharacter.navmeshAgent.SetDestination(aiCharacter.companion.transform.position);
        aiCharacter.enemyRigidBody.velocity = targetVelocity;
        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navmeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
    }
}
