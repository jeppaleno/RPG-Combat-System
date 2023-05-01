using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Attack Animations")]
    public string oh_light_attack_01 = "OH_Light_Attack_01";
    public string oh_light_attack_02 = "OH_Light_Attack_02";
    public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    public string oh_heavy_attack_02 = "OH_Heavy_Attack_02"; //ADD LATER
    public string oh_running_attack_01 = "OH_Running_Attack_01";
    public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    public string oh_charge_attack_01 = "OH_Charging_Attack_Charge_01";
    public string oh_charge_attack_02 = "OH_Charging_Attack_Charge_02";

    public string th_light_attack_01 = "TH_Light_Attack_01";
    public string th_light_attack_02 = "TH_Light_Attack_02";
    public string th_heavy_attack_01 = "TH_Heavy_Attack_01"; //ADD LATER
    public string th_heavy_attack_02 = "TH_Heavy_Attack_02"; //ADD LATER
    public string th_running_attack_01 = "TH_Running_Attack_01";
    public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    public string th_charge_attack_01 = "TH_Charging_Attack_Charge_01";
    public string th_charge_attack_02 = "TH_Charging_Attack_Charge_02";

    public string weapon_art = "Weapon_Art";

    public string lastAttack;

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
