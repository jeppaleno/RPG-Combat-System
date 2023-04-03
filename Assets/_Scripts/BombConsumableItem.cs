using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumeables/Bomb Item")]
public class BombConsumableItem : ConsumableItem
{
    [Header("Velocity")]
    public int upwardVelocity = 50;
    public int forwardVelocity = 50;
    public int bombMass = 1;

    [Header("Live Bomb Model")]
    public GameObject liveBombModel;

    [Header("Base Damage")]
    public int baseDamage = 200;
    public int explosiveDamage = 75;

    public override void AttemptToConsumeItem(PlayerAnimatorManager animatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if (currentItemAmount > 0)
        {
            weaponSlotManager.rightHandSlot.UnloadWeapon(); //Hide sword
            animatorManager.PlayTargetAnimation(consumeAnimation, true, true);
            GameObject bombModel = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform.position, Quaternion.identity, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.instantiatedFXModel = bombModel;
        }
        else
        {
            animatorManager.PlayTargetAnimation("shrug", true);
        }
    }
}
