using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Weapon Buff Damage")]
    public float physicalBuffDamage;
    public float fireBuffDamage;
    public float poiseBuffDamage;

    protected override void DealDamage(CharacterStatsManager enemyStats)
    {
        float finalPhysicalDamage = physicalDamage + physicalBuffDamage;
        float finalFireDamage = fireDamage + fireBuffDamage;
        float finalDamage = 0;

        //IF WE ARE USING THE RIGHT WEAPON, WE COMPARE THE RIGHT WEAPON MODIFIERS
        if (characterManager.isUsingRightHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                finalDamage += finalFireDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                finalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                finalDamage += finalFireDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
            }
        }
        //OTHERWISE WE COMPARE THE LEFT WEAPON MODIFIERS
        else if (characterManager.isUsingLeftHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                finalDamage += finalFireDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                finalDamage += finalFireDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
            }
        }

        //DEAL MODIFIED DAMAGE
        if (enemyStats.totalPoiseDefence > poiseBreak)
        {
            enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalDamage), Mathf.RoundToInt(finalFireDamage));
            //Debug.Log("Enemy Poise is currently" + playerStats.totalPoiseDefence);
        }
        else
        {
            enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), Mathf.RoundToInt(finalFireDamage), currentDamageAnimation, characterManager);
        }
    }
}
