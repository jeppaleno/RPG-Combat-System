using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateRotateTowardsTarget : State
{
    CompanionStateCombatStance combatStanceState;

    private void Awake()
    {
        combatStanceState = GetComponent<CompanionStateCombatStance>();
    }
    public override State Tick(AICharacterManager aiCharacter)
    {
        aiCharacter.animator.SetFloat("Vertical", 0);
        aiCharacter.animator.SetFloat("Horizontal", 0);



        if (aiCharacter.isInteracting)
            return this; //When we enter the state we will still be interacting from the attack animation so we pause here until it has finished

        if (aiCharacter.viewableAngle >= 100 && aiCharacter.viewableAngle <= 180 && !aiCharacter.isInteracting)
        {
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
            return combatStanceState;
        }
        else if (aiCharacter.viewableAngle <= -101 && aiCharacter.viewableAngle >= -180 && !aiCharacter.isInteracting)
        {
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
            return combatStanceState;
        }
        else if (aiCharacter.viewableAngle <= -45 && aiCharacter.viewableAngle >= -100 && !aiCharacter.isInteracting)
        {
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
            return combatStanceState;
        }
        else if (aiCharacter.viewableAngle >= 45 && aiCharacter.viewableAngle <= 100 && !aiCharacter.isInteracting)
        {
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
            return combatStanceState;
        }

        return combatStanceState;
    }
}
