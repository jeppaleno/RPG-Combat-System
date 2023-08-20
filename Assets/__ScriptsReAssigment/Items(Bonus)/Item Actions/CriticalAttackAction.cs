using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Critical Attack Action")]
public class CriticalAttackAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.isInteracting)
            return;

        character.characterCombatManager.AttemptBackStabOrRiposte();
    }
}
