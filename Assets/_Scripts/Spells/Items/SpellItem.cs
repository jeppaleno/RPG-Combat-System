using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject SpellCastFX;
    public string spellAnimation;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(AnimatorManager animatorManager, 
        PlayerStats playerStats, 
        WeaponSlotManager weaponSlotManager)
    {
        Debug.Log("You attempt to cast a spell!");
    }

    public virtual void SucessfullyCastSpell(
        AnimatorManager animatorManager, PlayerStats playerStats)
    {
        Debug.Log("You sucessfully cast a spell!");
    }
}
