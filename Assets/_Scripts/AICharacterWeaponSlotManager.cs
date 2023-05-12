using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterWeaponSlotManager : CharacterWeaponSlotManager
{
   
    public override void GrantWeaponAttackingPoiseBonus()
    {
        character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.totalPoiseDefence + character.characterStatsManager.offensivePoiseBonus;
    }

    public override void ResetWeaponAttackingPoiseBonus()
    {
        character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.armorPoiseBonus;
    }
}
