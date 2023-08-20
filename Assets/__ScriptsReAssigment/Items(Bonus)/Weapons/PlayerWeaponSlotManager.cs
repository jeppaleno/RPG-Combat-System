using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                //player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true, true);
            }
            else
            {
                if (player.inputManager.twohandFlag)
                {
                    //Move current left hand weapon to the back or disable it
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    player.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true, true);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                player.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
        else
        {
            weaponItem = unarmedWeapon;

            if (isLeft)
            {
                player.playerInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem); //weaponItem
                LoadLeftWeaponDamageCollider();
                player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                //player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                player.playerInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                player.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
    }

    public void SucessfullyThrowFireBomb()
    {
        Destroy(player.playerEffectsManager.instantiatedFXModel);
        BombConsumableItem fireBombItem = player.playerInventoryManager.currentConsumable as BombConsumableItem;

        GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, player.cameraManager.cameraPivotTransform.rotation);
        activeModelBomb.transform.rotation = Quaternion.Euler(player.cameraManager.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
        BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

        damageCollider.explosionDamage = fireBombItem.baseDamage;
        damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
        damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
        damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
        damageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
        LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false); //Reload RH sword after throw
        //Check for friendly fire
    }
}
