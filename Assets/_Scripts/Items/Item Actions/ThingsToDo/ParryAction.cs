using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.isInteracting)
            return;

        player.playerAnimatorManager.EraseHandIKWeapon();

        WeaponItem parryingWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;

        //Check if parrying weapon is a fast parry weapon or a medium parry weapon
        if (parryingWeapon.weaponType == WeaponType.SmallShield)
        {
            //Fast parry anim (ADD LATER)
            player.playerAnimatorManager.PlayTargetAnimation("Parry", true, true);
        }
        else if (parryingWeapon.weaponType != WeaponType.shield)
        {
            // Regular Parry
            player.playerAnimatorManager.PlayTargetAnimation("Parry", true, true);
        }
    }
}
