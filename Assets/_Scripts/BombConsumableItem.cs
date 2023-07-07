using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumeables/Bomb Item")]
public class BombConsumableItem : ConsumableItem
{
    [Header("Velocity")]
    public int upwardVelocity = 50;
    public int forwardVelocity = 500;
    public int bombMass = 1;

    [Header("Live Bomb Model")]
    public GameObject liveBombModel;

    [Header("Base Damage")]
    public int baseDamage = 200;
    public int explosiveDamage = 75;

    public override void AttemptToConsumeItem(PlayerManager player)
    {
        if (currentItemAmount > 0)
        {
            player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon(); //Hide sword
            player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, true, true);
            GameObject bombModel = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform.position, Quaternion.identity, player.playerWeaponSlotManager.rightHandSlot.transform);
            player.playerEffectsManager.instantiatedFXModel = bombModel;
        }
        else
        {
            player.playerAnimatorManager.PlayTargetAnimation("shrug", true);
        }
    }
}
