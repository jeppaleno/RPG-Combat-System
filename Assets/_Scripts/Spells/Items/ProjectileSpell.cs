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
        PlayerWeaponSlotManager weaponSlotManager,
        bool isLeftHanded)
    {
        base.AttemptToCastSpell(animatorManager, playerStats, weaponSlotManager, isLeftHanded);
        if(isLeftHanded)
        {
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.leftHandSlot.transform);
            instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(1, 1, 1); //change size here
            animatorManager.PlayTargetAnimation(spellAnimation, true, true, false, isLeftHanded);
        }
        else
        {
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
            instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(1, 1, 1); //change size here
            animatorManager.PlayTargetAnimation(spellAnimation, true, true, false, isLeftHanded);
        }

        // Instantiate the spell in the casting hand of the player
        
        // Play animation
    }

    public override void SucessfullyCastSpell(
        PlayerAnimatorManager animatorManager, 
        PlayerStatsManager playerStats, 
        CameraManager cameraManager,
        PlayerWeaponSlotManager weaponSlotManager,
        bool isLeftHanded)
    {
        base.SucessfullyCastSpell(animatorManager, playerStats, cameraManager, weaponSlotManager, isLeftHanded);
        if (isLeftHanded)
        {
            GameObject instantiatedSpellFX = Instantiate(SpellCastFX, weaponSlotManager.leftHandSlot.transform.position, cameraManager.cameraPivot.rotation);
            SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
            spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
            rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();

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
            // IN THE FUTURE, ADD AN INSTATIATION LOCATION ON THE CASTER WEAPON ITSELF
        }
        else
        {
            GameObject instantiatedSpellFX = Instantiate(SpellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraManager.cameraPivot.rotation);
            SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
            spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
            rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();

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
        //spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
    }
}
