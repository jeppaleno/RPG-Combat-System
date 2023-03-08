using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    InputManager inputManager;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;
    LayerMask backStabLayer = 1 << 12;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;

        if (inputManager.comboFlag)
        {
            animatorManager.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true, true); // Attack with root motion
            }
            else if (lastAttack == weapon.th_light_attack_01)
            {
                animatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;

        weaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twohandFlag)
        {
            animatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true, true);
            lastAttack = weapon.th_light_attack_01;
        }
        else
        {
            
            animatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true, true); // attack with root motion
            lastAttack = weapon.OH_Light_Attack_1;
        }
        
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;

        weaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twohandFlag)
        {

        }
        else
        {
            animatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true, true); // Attack with root motion
            lastAttack = weapon.OH_Light_Attack_1;
        }
        
    }

    #region Input Actions
    public void HandleAttackAction()
    {
        if (playerInventory.rightWeapon.isMeleeWeapon)
        {
            PerformAttackMeleeAction();
        }
        else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
        {
            PerformAttackMagicAction(playerInventory.rightWeapon);
        }
    }
    #endregion

    #region Attack Actions
    private void PerformAttackMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputManager.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
            inputManager.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;
            animatorManager.animator.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }

    private void PerformAttackMagicAction(WeaponItem weapon)
    {
        if (weapon.isFaithCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
            {
                //Check for fop
                playerInventory.currentSpell.AttemptToCastSpell(animatorManager, playerStats);
            }
        }
    }

    private void SucessfullyCastSpell()
    {
        playerInventory.currentSpell.SucessfullyCastSpell(animatorManager, playerStats);
    }

    #endregion

    public void AttemptBackStabOrRiposte()
    {
        if (playerStats.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(inputManager.criticalRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                // Check for team I.D (So you cant back stab friends or yourself?)
                // Pull is into a transform behind the enemy so the backstab looks clean
                playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabberStandPoint.position;
                // rotate us towards that transform
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                // play animation
                animatorManager.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<PreAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                
                // make enemy play animation
                // do damage
            }
        }
    }
}
