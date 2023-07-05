using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCharacterEffect : ScriptableObject
{
    public int effectID;
    // STATIC EFFECTS ARE TYPICALLY USED TO ADD AN EFFECT TO A PLAYER WHEN EQUIPPING AN ITEM, AND REMOVING THE EFFECT AFTER THE ITEM HAS BEEN REMOVED
    public virtual void AddStaticEffect(CharacterManager character)
    {

    }

    public virtual void RemoveStaticEffect(CharacterManager character)
    {

    }
}
