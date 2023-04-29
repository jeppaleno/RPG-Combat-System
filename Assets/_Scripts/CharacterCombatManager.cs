using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public virtual void DrainStaminaBasedOnAttack()
    {
        //ADD FOR AI LATER
    }

    public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDAmage, float fireDamage, string blockAnimation)
    {
        //DEDUCT STAMINA FROM BLOCKING
        if (character.characterStatsManager.currentStamina <= 0)
        {
            //Guard break
        }
        else
        {
            character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true, true);
        }
    }
}
