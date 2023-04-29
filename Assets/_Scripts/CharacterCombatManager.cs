using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    [Header("Attack Type")]
    public AttackType currentAttackType;

    public virtual void DrainStaminaBasedOnAttack()
    {
        //ADD FOR AI LATER
    }
}
