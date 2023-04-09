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

    [Header("Attack Animations")]
    string oh_light_attack_01 = "OH_Light_Attack_01";
    string oh_light_attack_02 = "OH_Light_Attack_02";
    string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    //string oh_heavy_attack_02 = "OH_Heavy_Attack_02"; //ADD LATER

    string th_light_attack_01 = "TH_Light_Attack_01";
    string th_light_attack_02 = "TH_Light_Attack_02";
    //string th_heavy_attack_01 = "TH_Heavy_Attack_01"; //ADD LATER
    //string oh_heavy_attack_02 = "OH_Heavy_Attack_02"; //ADD LATER

    string weapon_art = "Weapon_Art";

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

    public void HandleHoldRBAction()
    {
        if (playerManager.isTwoHandingWeapon)
        {
            PerformRBRangedAction();
        }
        else
        {
            //Do a melee attack (Bow Bash...)
        }
    }

    public void HandleAttackAction()
    {
        playerAnimatorManager.EraseHandIKWeapon();

        if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformAttackMeleeAction();
        }
        else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
        {
            PerformMagicAction(playerInventoryManager.rightWeapon, false); //HUHUHHHHHHHHHHHHHHHH
        }
    }

    public void HandleLBAction()
    {
        if (playerManager.isTwoHandingWeapon)
        {
            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
            {
                PerformLBAimingAction();
            }
        }
        else
        {
            if (playerInventoryManager.leftWeapon.weaponType == WeaponType.shield ||
                playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
            {
                performLBBlockingAction();
            }
            else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster ||
                playerInventoryManager.leftWeapon.weaponType == WeaponType.PyromancyCaster)
            {
                PerformMagicAction(playerInventoryManager.leftWeapon, true);
                playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
            }
        }
    }

    public void HandleLTAction()
    {
        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.shield
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLTWeaponArt(inputManager.twohandFlag);
        }
        else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
        {
            //do a light attack
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        if (inputManager.comboFlag)
        {
            playerAnimatorManager.animator.SetBool("canDoCombo", false);

            if (lastAttack == oh_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true, true); // Attack with root motion
            }
            else if (lastAttack == th_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true, true);
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
            playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true, true);
            lastAttack = th_light_attack_01;
        }
        else
        {
            
            playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true, true); // attack with root motion
            lastAttack = oh_light_attack_01;
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
            playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true, true); // Attack with root motion
            lastAttack = oh_light_attack_01;
        }
        
    }

    private void DrawArrowAction()
    {
        playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
        playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true, true);
        GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManager.leftHandSlot.transform);
        //ANIMATE THE BOW
        playerEffectsManager.currentRangeFX = loadedArrow;
    }


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

    private void PerformRBRangedAction()
    {
        if (playerStatsManager.currentStamina <= 0) 
            return;

        playerAnimatorManager.EraseHandIKWeapon();
        playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

        if (!playerManager.isHoldingArrow)
        {
            // If we have ammo
            if (!playerManager.isHoldingArrow)
            {
                if (playerInventoryManager.currentAmmo != null)
                {
                    DrawArrowAction();
                }
                else
                {
                    //Otherwise play an animation to indicate we are out of ammo
                    playerAnimatorManager.PlayTargetAnimation("shrug", true, true);
                }
            }
            
            //fire the arrow when we realease RB
            
        }
    }

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

    private void PerformLBAimingAction()
    {
        //playerAnimatorManager.animator.SetBool("isAiming", true);
    }

    private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
    {
        if (playerManager.isInteracting)
            return;

        if (weapon.weaponType == WeaponType.FaithCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("shrug", true, true);
                }
            }
        }
        else if (weapon.weaponType == WeaponType.PyromancyCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
            {
                if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
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
            playerAnimatorManager.PlayTargetAnimation(weapon_art, true, true);
        }
    }

    private void SucessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SucessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraManager, playerWeaponSlotManager, playerManager.isUsingLeftHand);
        playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }
   
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
                playerAnimatorManager.PlayTargetAnimation("Back Stab", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true, true);
                
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

                playerAnimatorManager.PlayTargetAnimation("Riposte", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true, true);
            }
        }
    }
}
