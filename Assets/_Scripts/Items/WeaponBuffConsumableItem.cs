using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuffConsumableItem : ConsumableItem
{
    [Header("Effect")]
    [SerializeField] WeaponBuffEffect weaponBuffEffect;

    [Header("Buff SFX")]
    [SerializeField] AudioClip buffTriggerSound;

    public override void AttemptToConsumeItem(PlayerManager player)
    {
        // IF I CANNOT USE THIS ITEM, RETURN WITHOUT DOING ANYTHING
        if (!CanIUseThisItem(player))
            return;

        if (currentItemAmount > 0)
        {
            player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true, true);
        }
        else
        {
            player.playerAnimatorManager.PlayTargetAnimation("shrug", true);
        }
    }

    public override void SucessfullyToConsumeItem(PlayerManager player)
    {
        base.SucessfullyToConsumeItem(player);

        player.characterSoundFXManager.PlaySoundFX(buffTriggerSound);

        WeaponBuffEffect weaponBuff = Instantiate(weaponBuffEffect);
        weaponBuff.isRightHandedBuff = true;
        player.playerEffectsManager.rightWeaponBuffEffect = weaponBuff;
        player.playerEffectsManager.ProcessWeaponBuffs();
    }

    public override bool CanIUseThisItem(PlayerManager player)
    {
        if (player.playerInventoryManager.currentConsumable.currentItemAmount <= 0)
            return false;

        MeleeWeaponItem meleeWeapon = player.playerInventoryManager.rightWeapon as MeleeWeaponItem;

        if (meleeWeapon != null && meleeWeapon.canBeBuffed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
