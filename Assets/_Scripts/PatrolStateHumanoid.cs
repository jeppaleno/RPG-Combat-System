using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStateHumanoid : State
{
    public PursueTargetStateHumanoid pursueTargetState;

    public LayerMask detectionLayer;
    public LayerMask layersThatBlockLineOfSight;

    public bool patrolComplete;
    public bool repeatPatrol;

    [Header("Patrol Rest Time")]
    public float endOfPatrolResetTime;
    public float endOfPatrolTimer;

    [Header("Patrol Position")]
    public int patrolDestinationIndex;
    public bool hasPatrolDestination;
    public Transform currentPatrolDestination;
    public float distanceFromCurrentPatrolPoint;
    public List<Transform> listOfPatrolDestinations = new List<Transform>();

    public override State Tick(AICharacterManager aiCharacter)
    {

        SearchForTargetWhilstPatroling(aiCharacter);

        if (aiCharacter.isInteracting)
        {
            aiCharacter.animator.SetFloat("Vertical", 0);
            aiCharacter.animator.SetFloat("Horizontal", 0);
            return this;
        }

        if (aiCharacter.currentTarget != null)
        {
            return pursueTargetState;
        }

        // If we completed our patrol and we do want to repeat it, we do so
        if (patrolComplete && repeatPatrol)
        {
            // We count down our rest time, and reset all of our patrol flags
            if (endOfPatrolResetTime > endOfPatrolTimer)
            {
                aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                endOfPatrolTimer = endOfPatrolTimer + Time.deltaTime;
                return this;
            }
            else if (endOfPatrolTimer >= endOfPatrolResetTime)
            {
                patrolDestinationIndex = -1;
                hasPatrolDestination = false;
                currentPatrolDestination = null;
                patrolComplete = false;
                endOfPatrolTimer = 0;
            }
        }
        else if (patrolComplete && !repeatPatrol)
        {
            aiCharacter.navmeshAgent.enabled = false;
            aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
            return this;
        }

        if (hasPatrolDestination)
        {
            if (currentPatrolDestination != null)
            {
                distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.transform.position);

                if (distanceFromCurrentPatrolPoint > 1)
                {
                    aiCharacter.navmeshAgent.enabled = true;
                    aiCharacter.navmeshAgent.destination = currentPatrolDestination.transform.position;
                    Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navmeshAgent.transform.rotation, 0.5f);
                    aiCharacter.transform.rotation = targetRotation;
                    aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime); //Slowing down to make it more patrol like. Set to 1 for normal movement

                }
                else
                {
                    currentPatrolDestination = null;
                    hasPatrolDestination = false;
                }
            }
        }

        if (!hasPatrolDestination)
        {
            patrolDestinationIndex = patrolDestinationIndex + 1;

            if (patrolDestinationIndex > listOfPatrolDestinations.Count - 1)
            {
                patrolComplete = true;
                return this;
            }

            currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
            hasPatrolDestination = true;
        }

        return this;
    }

    private void SearchForTargetWhilstPatroling(AICharacterManager aiCharacter)
    {
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
                            return;
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
            return;
        }
        else
        {
            return; 
        }
    }
}
