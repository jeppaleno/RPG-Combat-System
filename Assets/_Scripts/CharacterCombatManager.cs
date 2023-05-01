using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    LayerMask backStabLayer = 1 << 12;
    LayerMask riposteLayer = 1 << 13;

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

    private void SucessfullyCastSpell()
    {
        character.characterInventoryManager.currentSpell.SucessfullyCastSpell(character);
    }

    public void AttemptBackStabOrRiposte()
    {
        if (character.characterStatsManager.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(character.criticalRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                // Check for team I.D (So you cant back stab friends or yourself?)
                // Pull is into a transform behind the enemy so the backstab looks clean
                character.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                // rotate us towards that transform
                Vector3 rotationDirection = character.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - character.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                character.transform.rotation = targetRotation;

                int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                // play animation
                character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true, true);

                // make enemy play animation
                // do damage
            }
        }
        else if (Physics.Raycast(character.criticalRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            // Check for team I.D (future task)
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                character.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = character.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - character.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                character.transform.rotation = targetRotation;

                int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                character.characterAnimatorManager.PlayTargetAnimation("Riposte", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true, true);
            }
        }
    }

    

    
}
