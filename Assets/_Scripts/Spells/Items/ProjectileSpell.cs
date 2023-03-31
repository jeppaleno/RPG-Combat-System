using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile spell")]
public class ProjectileSpell : SpellItem
{
    [Header("Projectile Damage")]
    public float baseDamage;

    [Header("Projectile Physics")]
    public float projectileForwardVelocity;
    public float projectileUpwardVelocity;
    public float projectileMass;
    public bool isEffectedByGravity;
    Rigidbody rigidBody;

    public override void AttemptToCastSpell(
        PlayerAnimatorManager animatorManager,
        PlayerStatsManager playerStats,
        PlayerWeaponSlotManager weaponSlotManager)
    {
        base.AttemptToCastSpell(animatorManager, playerStats, weaponSlotManager);
        // Instantiate the spell in the casting hand of the player
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
        instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(1, 1, 1); //change size here
        animatorManager.PlayTargetAnimation(spellAnimation, true, true);
        // Play animation
    }

    public override void SucessfullyCastSpell(
        PlayerAnimatorManager animatorManager, 
        PlayerStatsManager playerStats, 
        CameraManager cameraManager,
        PlayerWeaponSlotManager weaponSlotManager)
    {
        base.SucessfullyCastSpell(animatorManager, playerStats, cameraManager, weaponSlotManager);
        GameObject instantiatedSpellFX = Instantiate(SpellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraManager.cameraPivot.rotation);
        rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();
        //spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();

        if (cameraManager.currentLockOnTarget != null)
        {
            instantiatedSpellFX.transform.LookAt(cameraManager.currentLockOnTarget.transform);
        }
        else
        {
            instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraManager.cameraPivot.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
        }

        rigidBody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
        rigidBody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
        rigidBody.useGravity = isEffectedByGravity;
        rigidBody.mass = projectileMass;
        instantiatedSpellFX.transform.parent = null;
    }
}
