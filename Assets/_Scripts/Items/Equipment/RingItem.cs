using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ring")]
public class RingItem : Item
{
    [SerializeField] StaticCharacterEffect effect;
    private StaticCharacterEffect effectClone;

    // FOR USER UI
    [Header("Item Effect Description")]
    [TextArea] public string itemEffectInformation;

    // CALLED WHEN EQUIPPING A RING, ADDS THE RINGS EFFECT TO OUR CHARACTER
    public void EquipRing(CharacterManager character)
    {
        // WE CREATE A CLONDE SO THE BASE SCRIPTABLE OBJECT IS NOT EFFECTED IF IN THE FUTURE WE CHANGE ANY OF ITS VARIABLE
        effectClone = Instantiate(effect);

        character.characterEffectsManager.AddStaticEffect(effectClone);
    }

    // CALLED WHEN UNEQUIPPING A RING, REMOVES EFFECT FROM OUR CHARACTER
    public void UnEquipRing(CharacterManager character)
    {
        character.characterEffectsManager.RemoveStaticEffect(effect.effectID);
    }
}
