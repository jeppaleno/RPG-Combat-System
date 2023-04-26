using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
public class HeavyAttackAction : ItemAction
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
            HandleJumpingAttack(player);
            return;
        }
        if (player.canDoCombo)
        {
            player.inputManager.comboFlag = true;
            HandleHeavyWeaponCombo(player);
            player.inputManager.comboFlag = false;
        }
        else
        {
            if (player.isInteracting)
                return;
            if (player.canDoCombo)
                return;

            HandleHeavyAttack(player);
        }
    }

    private void HandleHeavyAttack(PlayerManager player)
    {
        if (player.isUsingLeftHand)
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true, true, false, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
        }
        else if (player.isUsingRightHand)
        {
            if (player.inputManager.twohandFlag)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_01, true, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_01;
            }
            else
            {

                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
            }
        }
    }

    private void HandleJumpingAttack(PlayerManager player)
    {
        if (player.isUsingLeftHand)
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_jumping_attack_01, true, true, false, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_jumping_attack_01;
        }
        else if (player.isUsingRightHand)
        {
            if (player.inputManager.twohandFlag)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_jumping_attack_01, true, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_jumping_attack_01;
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_jumping_attack_01, true, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_jumping_attack_01;
            }
        }
    }

    private void HandleHeavyWeaponCombo(PlayerManager player)
    {
        if (player.inputManager.comboFlag)
        {
            player.animator.SetBool("canDoCombo", false);

            if (player.isUsingLeftHand)
            {
                if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_heavy_attack_01)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_02, true, true, false, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_02;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true, true, false, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
                }
            }
            else if (player.isUsingRightHand)
            {
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_heavy_attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_02, true, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_02;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_01, true, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_01;
                    }
                }
                else
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_heavy_attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_02, true, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_02;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
                    }
                }
            }
        }
    }
}
