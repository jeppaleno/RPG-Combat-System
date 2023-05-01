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

    public override void AttemptToCastSpell(CharacterManager character)
    {
        base.AttemptToCastSpell(character);
        if (character.isUsingLeftHand)
        {
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.leftHandSlot.transform);
            instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(1, 1, 1); //change size here
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, true, false, character.isUsingLeftHand);
        }
        else
        {
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.rightHandSlot.transform);
            instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(1, 1, 1); //change size here
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, true, false, character.isUsingLeftHand);
        }

        // Instantiate the spell in the casting hand of the player
        
        // Play animation
    }

    public override void SucessfullyCastSpell(CharacterManager character)
    {
        base.SucessfullyCastSpell(character);

        PlayerManager player = character as PlayerManager;

        //HANDLE THE PROCESS IF THE CASTER IS A PLAYER
        if (player != null)
        {
            if (player.isUsingLeftHand)
            {
                GameObject instantiatedSpellFX = Instantiate(SpellCastFX, player.playerWeaponSlotManager.leftHandSlot.transform.position, player.cameraManager.cameraPivot.rotation);
                SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
                rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();

                if (player.cameraManager.currentLockOnTarget != null)
                {
                    instantiatedSpellFX.transform.LookAt(player.cameraManager.currentLockOnTarget.transform);
                }
                else
                {
                    instantiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraManager.cameraPivot.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
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
                GameObject instantiatedSpellFX = Instantiate(SpellCastFX, player.playerWeaponSlotManager.rightHandSlot.transform.position, player.cameraManager.cameraPivot.rotation);
                SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
                rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();

                if (player.cameraManager.currentLockOnTarget != null)
                {
                    instantiatedSpellFX.transform.LookAt(player.cameraManager.currentLockOnTarget.transform);
                }
                else
                {
                    instantiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraManager.cameraPivot.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                }

                rigidBody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                rigidBody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                rigidBody.useGravity = isEffectedByGravity;
                rigidBody.mass = projectileMass;
                instantiatedSpellFX.transform.parent = null;
            }
        }
        //HANDLE THE PROCESS IF THE CASTER IS AN AI
        else
        {

        }
        
    }
}
