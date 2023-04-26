using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
public class LightAttackAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        player.playerAnimatorManager.EraseHandIKWeapon();
        player.playerEffectsManager.PlayWeaponFX(false);

        //If we can perform a running attack, we do that if not, continue
        if (player.isSprinting)
        {
            HandleRunningAttack(player);
            return;
        }
        if (player.canDoCombo)
        {
            player.inputManager.comboFlag = true;
            HandleLightWeaponCombo(player);
            player.inputManager.comboFlag = false;
        }
        else
        {
            if (player.isInteracting)
                return;
            if (player.canDoCombo)
                return;

            HandleLightAttack(player);
        }
    }

    private void HandleLightAttack(PlayerManager player)
    {
        if (player.isUsingLeftHand)
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, true, false, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
        }
        else if (player.isUsingRightHand)
        {
            if (player.inputManager.twohandFlag)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
            }
            else
            {

                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, true); 
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
            }
        }
    }

    private void HandleRunningAttack(PlayerManager player)
    {
        if (player.isUsingLeftHand)
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true, true, false, true); 
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
        }
        else if (player.isUsingRightHand)
        {
            if (player.inputManager.twohandFlag)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_running_attack_01, true, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_running_attack_01;
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true, true); 
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
            }
        }
    }

    private void HandleLightWeaponCombo(PlayerManager player)
    {
        if (player.inputManager.comboFlag)
        {
            player.animator.SetBool("canDoCombo", false);

            if (player.isUsingLeftHand)
            {
                if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_01)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_02, true, true, false, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_02;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, true, false, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
                }
            }
            else if (player.isUsingRightHand)
            {
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
}


