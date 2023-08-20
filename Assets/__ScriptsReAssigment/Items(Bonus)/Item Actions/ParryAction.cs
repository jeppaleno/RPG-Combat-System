using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.isInteracting)
            return;

        character.characterAnimatorManager.EraseHandIKWeapon();

        WeaponItem parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;

        //Check if parrying weapon is a fast parry weapon or a medium parry weapon
        if (parryingWeapon.weaponType == WeaponType.SmallShield)
        {
            //Fast parry anim (ADD LATER)
            character.characterAnimatorManager.PlayTargetAnimation("Parry", true, true);
        }
        else if (parryingWeapon.weaponType != WeaponType.shield)
        {
            // Regular Parry
            character.characterAnimatorManager.PlayTargetAnimation("Parry", true, true);
        }
    }
}
