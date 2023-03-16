using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(
        AnimatorManager animatorManager,
        PlayerStats playerStats,
        WeaponSlotManager weaponSlotManager)
    {
        base.AttemptToCastSpell(animatorManager, playerStats, weaponSlotManager);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorManager.transform);
        animatorManager.PlayTargetAnimation(spellAnimation, true);
        Debug.Log("Attempting to cast spell...");
    }

    public override void SucessfullyCastSpell(
        AnimatorManager animatorManager, 
        PlayerStats playerStats,
        CameraManager cameraManager,
        WeaponSlotManager weaponSlotManager)
    {
        base.SucessfullyCastSpell(animatorManager, playerStats, cameraManager, weaponSlotManager);
        GameObject instantiatedSpellFX = Instantiate(SpellCastFX, animatorManager.transform);
        playerStats.HealPlayer(healAmount);
        Debug.Log("successfully cast spell...");
    }
}
