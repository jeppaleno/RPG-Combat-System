using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateFollowHost : State
{
    // EP 118 12:11
    public override State Tick(AICharacterManager aiCharacter)
    {
        HandleRotateTowardstarget(aiCharacter);

        if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
        {

        }

        if (aiCharacter.isInteracting)
            return this;

        if (aiCharacter.isPerformingAction)
        {

            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
        {
            aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if (aiCharacter.distanceFromTarget <= aiCharacter.maximumAggroRadius)
        {
            
        }
        else
        {
            return this;
        }
        return this;
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
