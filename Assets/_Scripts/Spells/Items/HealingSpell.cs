using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(CharacterManager character)
    {
        base.AttemptToCastSpell(character);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.transform);
        character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, true, false, character.isUsingLeftHand);
        Debug.Log("Attempting to cast spell...");
    }

    public override void SucessfullyCastSpell(CharacterManager character)
    {
        base.SucessfullyCastSpell(character);
        GameObject instantiatedSpellFX = Instantiate(SpellCastFX, character.transform);
        character.characterStatsManager.HealCharacter(healAmount);
        Debug.Log("successfully cast spell...");
    }
}
