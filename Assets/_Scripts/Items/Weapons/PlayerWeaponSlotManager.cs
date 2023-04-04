using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    PlayerManager playerManager;
    PlayerInventoryManager playerInventoryManager;
    Animator animator;
    QuickSlotsUI quickSlotsUI;
    PlayerStatsManager playerStatsManager;
    InputManager inputManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerAnimatorManager playerAnimatorManager;
    CameraManager cameraManager;

    [Header("Attacking Weapon")]
    public WeaponItem attackingWeapon;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        inputManager = GetComponent<InputManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerManager = GetComponent<PlayerManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        LoadWeaponHolderSlot();
    }

    private void LoadWeaponHolderSlot()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadBothWeaponOnSlot()
    {
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true, true);
            }
            else
            {
                if (inputManager.twohandFlag)
                {
                    //Move current left hand weapon to the back or disable it
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true, true);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
        else
        {
            weaponItem = unarmedWeapon;

            if (isLeft)
            {
                playerInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem); //weaponItem
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                playerInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
    }

    public void SucessfullyThrowFireBomb()
    {
        Destroy(playerEffectsManager.instantiatedFXModel);
        BombConsumableItem fireBombItem = playerInventoryManager.currentConsumable as BombConsumableItem;

        GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, cameraManager.cameraPivot.rotation);
        activeModelBomb.transform.rotation = Quaternion.Euler(cameraManager.cameraPivot.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
        BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

        damageCollider.explosionDamage = fireBombItem.baseDamage;
        damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
        damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
        damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
        damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false); //Reload RH sword after throw
        //Check for friendly fire
    }

    #region Handle Weapon's Damage Colliders

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        leftHandDamageCollider.physicalDamage = playerInventoryManager.leftWeapon.physicalDamage;
        leftHandDamageCollider.fireDamage = playerInventoryManager.leftWeapon.fireDamage;

        leftHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

        leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
        playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        rightHandDamageCollider.physicalDamage = playerInventoryManager.rightWeapon.physicalDamage;
        rightHandDamageCollider.fireDamage = playerInventoryManager.rightWeapon.fireDamage;

        rightHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

        rightHandDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
        playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    public void OpenDamageCollider()
    {
        if (playerManager.isUsingRightHand)
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        else if (playerManager.isUsingLeftHand)
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
    }


    public void CloseDamageCollider()
    {
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        if (leftHandDamageCollider != null)
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }

    #endregion

    #region Handle Weapon's Stamina Drainage
    public void DrainStaminaLightAttack()
    {
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion

    #region Handle Weapon's Poise Bonus

    public void GrantWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefence = playerStatsManager.totalPoiseDefence + attackingWeapon.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefence = playerStatsManager.armorPoiseBonus;
    }
    #endregion

}
