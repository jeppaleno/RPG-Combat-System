using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;

    public override State Tick(EnemyManager enemy)
    {
        #region Handle Target Detection
        // Look for a potential target
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if (characterStats != null)
            {
                if (characterStats.teamIDNumber != enemy.enemyStatsManager.teamIDNumber)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                    {
                        enemy.currentTarget = characterStats;

                    }
                }
            }
        }
        #endregion

        #region Handle Switching To Next State
        //Switch to the pursue target state if target is found
        if (enemy.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this; //If not return state
        }
        #endregion 
    }
}
