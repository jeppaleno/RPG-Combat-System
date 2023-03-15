using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile spell")]
public class ProjectileSpell : SpellItem
{
    public float baseDamage;
    public float projectileVelocity;
    Rigidbody rigidBody;

    public override void AttemptToCastSpell(
        AnimatorManager animatorManager,
        PlayerStats playerStats,
        WeaponSlotManager weaponSlotManager)
    {
        base.AttemptToCastSpell(animatorManager, playerStats, weaponSlotManager);
        // Instantiate the spell in the casting hand of the player
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
        instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(1, 1, 1); //change size here
        animatorManager.PlayTargetAnimation(spellAnimation, true, true);
        // Play animation
    }

    public override void SucessfullyCastSpell(
        AnimatorManager animatorManager, 
        PlayerStats playerStats)
    {
        base.SucessfullyCastSpell(animatorManager, playerStats);
    }
}
