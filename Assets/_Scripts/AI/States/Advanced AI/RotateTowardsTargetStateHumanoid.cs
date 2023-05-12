using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetStateHumanoid : State
{
    public CombatStanceStateHumanoid combatStanceState;

    private void Awake()
    {
        combatStanceState = GetComponent<CombatStanceStateHumanoid>();
    }
    public override State Tick(AICharacterManager enemy)
    {
        enemy.animator.SetFloat("Vertical", 0);
        enemy.animator.SetFloat("Horizontal", 0);



        if (enemy.isInteracting)
            return this; //When we enter the state we will still be interacting from the attack animation so we pause here until it has finished

        if (enemy.viewableAngle >= 100 && enemy.viewableAngle <= 180 && !enemy.isInteracting)
        {
            enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
            return combatStanceState;
        }
        else if (enemy.viewableAngle <= -101 && enemy.viewableAngle >= -180 && !enemy.isInteracting)
        {
            enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
            return combatStanceState;
        }
        else if (enemy.viewableAngle <= -45 && enemy.viewableAngle >= -100 && !enemy.isInteracting)
        {
            enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
            return combatStanceState;
        }
        else if (enemy.viewableAngle >= 45 && enemy.viewableAngle <= 100 && !enemy.isInteracting)
        {
            enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
            return combatStanceState;
        }

        return combatStanceState;
    }
}
