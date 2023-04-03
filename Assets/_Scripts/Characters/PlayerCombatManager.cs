using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    CameraManager cameraManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerInventoryManager playerInventoryManager;
    InputManager inputManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    PlayerEffectsManager playerEffectsManager;

    public string lastAttack;

    LayerMask backStabLayer = 1 << 12;
    LayerMask riposteLayer = 1 << 13;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        if (inputManager.comboFlag)
        {
            playerAnimatorManager.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true, true); // Attack with root motion
            }
            else if (lastAttack == weapon.th_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twohandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true, true);
            lastAttack = weapon.th_light_attack_01;
        }
        else
        {
            
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true, true); // attack with root motion
            lastAttack = weapon.OH_Light_Attack_1;
        }
        
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twohandFlag)
        {

        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true, true); // Attack with root motion
            lastAttack = weapon.OH_Light_Attack_1;
        }
        
    }

    #region Input Actions
    public void HandleAttackAction()
    {
        if (playerInventoryManager.rightWeapon.isMeleeWeapon)
        {
            PerformAttackMeleeAction();
        }
        else if (playerInventoryManager.rightWeapon.isSpellCaster || playerInventoryManager.rightWeapon.isFaithCaster || playerInventoryManager.rightWeapon.isPyroCaster)
        {
            PerformAttackMagicAction(playerInventoryManager.rightWeapon);
        }
    }

    public void HandleLBAction()
    {
        performLBBlockingAction(); 
    }

    public void HandleLTAction()
    {
        if (playerInventoryManager.leftWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(inputManager.twohandFlag);
        }
        else if (playerInventoryManager.leftWeapon.isMeleeWeapon)
        {
            //do a light attack
        }
    }
    #endregion

    #region Attack Actions
    private void PerformAttackMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputManager.comboFlag = true;
            HandleWeaponCombo(playerInventoryManager.rightWeapon);
            inputManager.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;
            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventoryManager.rightWeapon);
        }

        playerEffectsManager.PlayWeaponFX(false);
    }

    private void PerformAttackMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting)
            return;

        if (weapon.isFaithCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("shrug", true, true);
                }
            }
        }
        else if (weapon.isPyroCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
            {
                if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("shrug", true, true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting)
            return;

        if (isTwoHanding)
        {
            // If we are two handing perform weapon art for right weapon
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(playerInventoryManager.leftWeapon.weapon_art, true, true);
        }
    }

    private void SucessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SucessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraManager, playerWeaponSlotManager);
        playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }

    #endregion

    #region Defense Actions
    private void performLBBlockingAction()
    {
        if (playerManager.isInteracting)
            return;

        if (playerManager.isBlocking)
            return;

        playerAnimatorManager.PlayTargetAnimation("Block_Start", false, true, true);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;
    }
    #endregion
    public void AttemptBackStabOrRiposte()
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(inputManager.criticalRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                // Check for team I.D (So you cant back stab friends or yourself?)
                // Pull is into a transform behind the enemy so the backstab looks clean
                playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                // rotate us towards that transform
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                // play animation
                playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                
                // make enemy play animation
                // do damage
            }
        }
        else if (Physics.Raycast(inputManager.criticalRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            // Check for team I.D (future task)
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }
}
