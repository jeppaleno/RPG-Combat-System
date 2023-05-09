using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{

    CharacterManager character;

    [Header("Comat Transforms")]
    public Transform backStabReceiverTransform;

    public LayerMask characterLayer;
    public float criticalAttackRange = 0.7f;

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
    public int pendingCriticalDamage;
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

    IEnumerator ForceMoveCharacterToEnemyBackStabPosition(CharacterManager characterPerformingBackStab)
    {
        for (float timer = 0.05f; timer < 0.5f; timer = timer + 0.05f)
        {
            Quaternion backstabRotation = Quaternion.LookRotation(characterPerformingBackStab.transform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
            transform.parent = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform;
            transform.localPosition = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform.localPosition;
            transform.parent = null;
            //Debug.Log("Running corountine");
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void GetBackStabbed(CharacterManager characterPerformingBackStab)
    {
        //Play Sound FX
        character.isBeingBackstabbed = true;

        //FORCE LOCK POSITION
        StartCoroutine(ForceMoveCharacterToEnemyBackStabPosition(characterPerformingBackStab));

        character.characterAnimatorManager.PlayTargetAnimation("Back Stabbed", true);
    }
    public void AttemptBackStabOrRiposte()
    {
        if (character.isInteracting)
            return;

        if (character.characterStatsManager.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(character.criticalRayCastStartPoint.transform.position, character.transform.TransformDirection(Vector3.forward), out hit, criticalAttackRange, characterLayer))
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
            Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
            float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

            //Debug.Log("Current dot value is" + dotValue);

            if (enemyCharacter.canBeRiposted)
            {
                if (dotValue <= 1.2f && dotValue >= 0.6f)
                {
                    //Attempt riposte
                }
            }

            if (dotValue >= -1.4 && dotValue <= -0.2f)
            {
                AttemptBackStab(hit);
            }
        }
    }

    private void AttemptBackStab(RaycastHit hit)
    {
        CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

        if (enemyCharacter != null)
        {
            if (!enemyCharacter.isBeingBackstabbed || !enemyCharacter.isBeingRiposted)
            {
                //We make it so the enemy cannot be damaged whilst being critically damaged
                //EnableIsInvulnerable();
                character.isPerformingBackstab = true;
                character.characterAnimatorManager.EraseHandIKWeapon();

                character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);

                float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * 
                    (character.characterInventoryManager.rightWeapon.physicalDamage +
                    character.characterInventoryManager.rightWeapon.fireDamage));

                int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
                enemyCharacter.characterCombatManager.GetBackStabbed(character);
            }
        }
    }

    /*private void EnableIsInvulnerable()
    {
        character.animator.SetBool("isInvulnerable", true);
    }*/

    public void ApplyPendingDamage()
    {
        character.characterStatsManager.TakeDamageNoAnimation(pendingCriticalDamage, 0);
    }
}
