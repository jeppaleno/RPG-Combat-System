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
        if(inputManager.comboFlag)
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
}
