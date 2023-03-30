using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject SpellCastFX;
    public string spellAnimation;

    [Header("Spell Cost")]
    public int focusPointCost;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(AnimatorManager animatorManager, 
        PlayerStatsManager playerStats, 
        PlayerWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("You attempt to cast a spell!");
    }

    public virtual void SucessfullyCastSpell(
        AnimatorManager animatorManager, 
        PlayerStatsManager playerStats,
        CameraManager cameraManager,
        PlayerWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("You sucessfully cast a spell!");
        playerStats.DeductFocusPoints(focusPointCost);
    }
}
