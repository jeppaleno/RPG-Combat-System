using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
public class LightAttackAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        player.playerAnimatorManager.EraseHandIKWeapon();
        player.playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
        player.playerEffectsManager.PlayWeaponFX(false);

        //If we can perform a running attack, we do that if not, continue
        if (player.isSprinting)
        {
            HandleRunningAttack(player.playerInventoryManager.rightWeapon, player);
            return;
        }
        if (player.canDoCombo)
        {
            player.inputManager.comboFlag = true;
            HandleLightWeaponCombo(player.playerInventoryManager.rightWeapon, player);
            player.inputManager.comboFlag = false;
        }
        else
        {
            if (player.isInteracting)
                return;
            if (player.canDoCombo)
                return;

            HandleLightAttack(player.playerInventoryManager.rightWeapon, player);
        }
    }

    private void HandleLightAttack(WeaponItem weapon, PlayerManager player)
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        player.playerWeaponSlotManager.attackingWeapon = weapon;

        if (player.inputManager.twohandFlag)
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
        }
        else
        {

            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, true); // attack with root motion
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
        }

    }

    private void HandleRunningAttack(WeaponItem weapon, PlayerManager player)
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        player.playerWeaponSlotManager.attackingWeapon = weapon;

        if (player.inputManager.twohandFlag)
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_running_attack_01, true, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.th_running_attack_01;
        }
        else
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true, true); // attack with root motion
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
        }
    }

    private void HandleLightWeaponCombo(WeaponItem weapon, PlayerManager player)
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        if (player.inputManager.comboFlag)
        {
            player.playerAnimatorManager.animator.SetBool("canDoCombo", false);

            if (player.isTwoHandingWeapon)
            {
                if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_light_attack_01)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_02, true, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_02;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
                }
            }
            else
            {
                if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_01)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_02, true, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_02;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
                }
            }
        }
    }
}


