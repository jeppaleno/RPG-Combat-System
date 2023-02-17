using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : MonoBehaviour
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

    public virtual void AttemptToCastSpell()
    {
        Debug.Log("You attempt to cast a spell!");
    }

    public virtual void SucessfullyCastSpell()
    {
        Debug.Log("You sucessfully cast a spell!");
    }
}
