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
    public override State Tick(AICharacterManager enemy)
    {
        if (isSleeping && enemy.isInteracting == false)
        {
            enemy.aiCharacterAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Handle Target Detection

        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager potentialTarget = colliders[i].transform.GetComponent<CharacterManager>();

            if (potentialTarget != null)
            {
                Vector3 targetDirection = potentialTarget.transform.position - enemy.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                if (viewableAngle > enemy.minimumDetectionAngle
                    && viewableAngle < enemy.maximumDetectionAngle)
                {
                    enemy.currentTarget = potentialTarget;
                    isSleeping = false;
                    enemy.aiCharacterAnimatorManager.PlayTargetAnimation(wakeAnimation, true); 
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
