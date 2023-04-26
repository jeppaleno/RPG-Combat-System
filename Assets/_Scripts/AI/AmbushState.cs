using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;

    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemy)
    {
        if (isSleeping && enemy.isInteracting == false)
        {
            enemy.enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Handle Target Detection

        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - enemy.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                if (viewableAngle > enemy.minimumDetectionAngle
                    && viewableAngle < enemy.maximumDetectionAngle)
                {
                    enemy.currentTarget = characterStats;
                    isSleeping = false;
                    enemy.enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true); 
                }
            }
        }

        #endregion

        #region Handle State Change

        if (enemy.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }

        #endregion
    }
}
