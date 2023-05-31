using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateCombatStance : State
{
    public override State Tick(AICharacterManager aiCharacter)
    {
        return this;
    }
}
