using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Magic Spell Action")]
public class MagicSpellAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.isInteracting)
            return;

        if (character.characterInventoryManager.currentSpell != null && character.characterInventoryManager.currentSpell.isMagicSpell)
        {
            if (character.characterStatsManager.currentFocusPoints >= character.characterInventoryManager.currentSpell.focusPointCost)
            {
                character.characterInventoryManager.currentSpell.AttemptToCastSpell(character);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation("shrug", true, true);
            }
        }
    }
}
