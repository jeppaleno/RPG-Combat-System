using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Static Effects/Modify Damage Type")]
public class ModifyDamageTypeStaticEffect : StaticCharacterEffect
{
    [Header("Damage Type Effected")]
    [SerializeField] DamageType damageType;
    [SerializeField] int modifiedValue = 0;

    // WHEN ADDING THE EFFECT WE ADD THE MODIFIED VALUE AMOUNT TO OUR PERSPECTIVE DAMAGE TYPE MODIFIER
    public override void AddStaticEffect(CharacterManager character)
    {
        base.AddStaticEffect(character);

        switch (damageType)
        {
            case DamageType.Physical: character.characterStatsManager.physicalDamagePercentageModifier += modifiedValue;
                break;
            case DamageType.Fire: character.characterStatsManager.fireDamagePercentageModifier += modifiedValue;
                break;
            default:
                break;
        }
    }

    // WHEN REMOVING THE EFFECT, WE SUBTRACT THE AMOUNT WE ADDED
    public override void RemoveStaticEffect(CharacterManager character)
    {
        base.RemoveStaticEffect(character);

        switch (damageType)
        {
            case DamageType.Physical:
                character.characterStatsManager.physicalDamagePercentageModifier -= modifiedValue;
                break;
            case DamageType.Fire:
                character.characterStatsManager.fireDamagePercentageModifier -= modifiedValue;
                break;
            default:
                break;
        }
    }
}
