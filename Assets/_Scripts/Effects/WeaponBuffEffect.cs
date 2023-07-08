using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Weapon Buff Effects")]
public class WeaponBuffEffect : CharacterEffect
{
    [Header("Buff Info")]
    [SerializeField] BuffClass buffClass;
    [SerializeField] float lengthOfBuff;
    public float timeRemainingOnBuff;
    [HideInInspector] public bool isRightHandedBuff;

    [Header("Buff SFX")]
    [SerializeField] AudioClip buffAmbientSound;
    [SerializeField] float ambientSoundVolume = 0.3f;

    [Header("Poise Buff")]
    [SerializeField] bool buffPoiseDamage;
    [SerializeField] float buffBasePoiseDamagePercentageMultiplier = 15;

    [Header("General")]
    [SerializeField] bool buffHasStarted = false;
    private WeaponManager weaponManager;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (!buffHasStarted)
        {
            timeRemainingOnBuff = lengthOfBuff;
            buffHasStarted = true;

            weaponManager = character.characterWeaponSlotManager.rightHandDamageCollider.GetComponentInParent<WeaponManager>();
            weaponManager.audioSource.loop = true;
            weaponManager.audioSource.clip = buffAmbientSound;
            weaponManager.audioSource.volume = ambientSoundVolume;

            float baseWeaponDamage =
                weaponManager.damageCollider.physicalDamage +
                weaponManager.damageCollider.fireDamage;

            float physicalBuffDamage = 0;
            float fireBuffDamage = 0;
            float poiseBuffDamage = 0;

            if (buffPoiseDamage)
            {
                poiseBuffDamage = weaponManager.damageCollider.poiseBreak * (buffBasePoiseDamagePercentageMultiplier / 100);
            }

            switch (buffClass)
            {
                case BuffClass.Physical: physicalBuffDamage = baseWeaponDamage * (buffBasePoiseDamagePercentageMultiplier / 100);
                    break;
                case BuffClass.Fire: fireBuffDamage = baseWeaponDamage * (buffBasePoiseDamagePercentageMultiplier / 100);
                    break;
                default:
                    break;
            }

            weaponManager.BuffWeapon(buffClass, physicalBuffDamage, fireBuffDamage, poiseBuffDamage);
        }

        if (buffHasStarted)
        {
            timeRemainingOnBuff = timeRemainingOnBuff - 1;

            Debug.Log("Time reamaining on buff: " + timeRemainingOnBuff);

            if (timeRemainingOnBuff <= 0)
            {
                weaponManager.DebuffWeapon();

                if (isRightHandedBuff)
                {
                    character.characterEffectsManager.rightWeaponBuffEffect = null;
                }
            }
        }
    }

}
