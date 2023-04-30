using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
    {
        if (character.isUsingRightHand)
        {
            character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
            character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorption;
            character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightWeapon.stability;
        }
        else if (character.isUsingLeftHand)
        {
            character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
            character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorption;
            character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftWeapon.stability;
        }
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        //ADD FOR AI LATER
    }

    public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDAmage, float fireDamage, string blockAnimation)
    {
        //DEDUCT STAMINA FROM BLOCKING
        float StaminaDamageAbsorption = ((physicalDAmage + fireDamage) * attackingWeapon.guardBreakModifier)
            * (character.characterStatsManager.blockingStabilityRating / 100);

        float staminaDamage = ((physicalDAmage + fireDamage) * attackingWeapon.guardBreakModifier) - StaminaDamageAbsorption;

        character.characterStatsManager.currentStamina = character.characterStatsManager.currentStamina - staminaDamage;

        if (character.characterStatsManager.currentStamina <= 0)
        {
            character.isBlocking = false;
            character.characterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true, true);   
        }
        else
        {
            character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true, true);
        }
    }
}
