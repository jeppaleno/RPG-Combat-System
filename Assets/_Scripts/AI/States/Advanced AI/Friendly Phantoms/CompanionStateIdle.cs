using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateIdle : State
{
    CompanionStatePursueTarget pursueTargetState;
    CompanionStateFollowHost followHostState;

    public LayerMask detectionLayer;
    public LayerMask layersThatBlockLineOfSight;

    private void Awake()
    {
        followHostState = GetComponent<CompanionStateFollowHost>();
        pursueTargetState = GetComponent<CompanionStatePursueTarget>();
    }

    public override State Tick(AICharacterManager aiCharacter)
    {
        aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

        if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
        {
            return followHostState;
        }

        // Searches for a potential target within the detection radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            //If a potentential target is found, that is not on the sam team as the A:I we proceed to the next step
            if (targetCharacter != null)
            {
                if (targetCharacter.characterStatsManager.teamIDNumber != aiCharacter.aiCharacterStatsManager.teamIDNumber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //if a potential target is found, it has to be standing infront of the AI's field of view
                    if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
                    {
                        if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                        {
                            return this;
                        }
                        else
                        {
                            aiCharacter.currentTarget = targetCharacter;
                        }
                    }
                }
            }
        }

        //Switch to the pursue target state if target is found
        if (aiCharacter.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this; //If not return state
        }
    }
}
