using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

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

    LayerMask backStabLayer = 1 << 12;
    LayerMask riposteLayer = 1 << 13;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }


    private void SucessfullyCastSpell()
    {
        player.playerInventoryManager.currentSpell.SucessfullyCastSpell(player.playerAnimatorManager, player.playerStatsManager, player.cameraManager, player.playerWeaponSlotManager, player.isUsingLeftHand);
        player.animator.SetBool("isFiringSpell", true);
    }
   
    public void AttemptBackStabOrRiposte()
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(player.inputManager.criticalRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = player.playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                // Check for team I.D (So you cant back stab friends or yourself?)
                // Pull is into a transform behind the enemy so the backstab looks clean
                player.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                // rotate us towards that transform
                Vector3 rotationDirection = player.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
                player.transform.rotation = targetRotation;

                int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                // play animation
                player.playerAnimatorManager.PlayTargetAnimation("Back Stab", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true, true);
                
                // make enemy play animation
                // do damage
            }
        }
        else if (Physics.Raycast(player.inputManager.criticalRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            // Check for team I.D (future task)
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = player.playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                player.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = player.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
                player.transform.rotation = targetRotation;

                int criticalDamage = player.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                player.playerAnimatorManager.PlayTargetAnimation("Riposte", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true, true);
            }
        }
    }

    public override void DrainStaminaBasedOnAttack()
    {
        if (player.isUsingRightHand)
        {
            if (currentAttackType == AttackType.light)
            {
                player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.lightAttackStaminaMultiplier);
            }
            else if (currentAttackType == AttackType.heavy)
            {
                player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);
            }
        }
        else if (player.isUsingLeftHand)
        {
            if (currentAttackType == AttackType.light)
            {
                player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.leftWeapon.lightAttackStaminaMultiplier);
            }
            else if (currentAttackType == AttackType.heavy)
            {
                player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.leftWeapon.heavyAttackStaminaMultiplier);
            }
        }
    }
}
