using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(
        PlayerAnimatorManager animatorManager,
        PlayerStatsManager playerStats,
        PlayerWeaponSlotManager weaponSlotManager,
        bool isLeftHanded)
    {
        base.AttemptToCastSpell(animatorManager, playerStats, weaponSlotManager, isLeftHanded);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorManager.transform);
        animatorManager.PlayTargetAnimation(spellAnimation, true, true, false, isLeftHanded);
        Debug.Log("Attempting to cast spell...");
    }

    public override void SucessfullyCastSpell(
        PlayerAnimatorManager animatorManager, 
        PlayerStatsManager playerStats,
        CameraManager cameraManager,
        PlayerWeaponSlotManager weaponSlotManager,
        bool isLeftHanded)
    {
        base.SucessfullyCastSpell(animatorManager, playerStats, cameraManager, weaponSlotManager, isLeftHanded);
        GameObject instantiatedSpellFX = Instantiate(SpellCastFX, animatorManager.transform);
        playerStats.HealPlayer(healAmount);
        Debug.Log("successfully cast spell...");
    }
}
