using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Buff FX")]
    [SerializeField] GameObject physicalBuffFX;
    [SerializeField] GameObject fireBuffFX;

    [Header("Trail FX")]
    [SerializeField] ParticleSystem defaultTrailFX;
    [SerializeField] ParticleSystem fireTrailFX;

    private bool weaponIsBuffed;
    private BuffClass weaponBuffClass;

    [HideInInspector] public MeleeWeaponDamageCollider damageCollider;
    public AudioSource audioSource;

    private void Awake()
    {
        damageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void BuffWeapon(BuffClass buffClass, float physicalBuffDamage, float fireBuffDamage, float poiseBuffDamage)
    {
        // RESET ANY ACTIVE BUFF
        DebuffWeapon();
        weaponIsBuffed = true;
        weaponBuffClass = buffClass;
        audioSource.Play();

        switch (buffClass)
        {
            case BuffClass.Physical: physicalBuffFX.SetActive(true);
                break;

            case BuffClass.Fire: fireBuffFX.SetActive(true);
                break;

            default:
                break;
        }

        damageCollider.physicalBuffDamage = physicalBuffDamage;
        damageCollider.fireBuffDamage = fireBuffDamage;
        damageCollider.poiseBuffDamage = poiseBuffDamage;    
    }

    public void DebuffWeapon()
    {
        weaponIsBuffed = false;
        audioSource.Stop();
        physicalBuffFX.SetActive(false);
        fireBuffFX.SetActive(false);

        damageCollider.physicalBuffDamage = 0;
        damageCollider.fireBuffDamage = 0;
        damageCollider.poiseBuffDamage = 0;
    }

    public void PlayWeaponTrailFX()
    {
        if (weaponIsBuffed)
        {
            switch (weaponBuffClass)
            {
                // IF THE WEAPON IS PHYSICALLY BUFFED, PLAY THE DEFAULT TRAIL
                case BuffClass.Physical:
                    if (defaultTrailFX == null)
                        return;
                    defaultTrailFX.Play();
                    break;

                // IF THE WEAPON IS FIRE BUFFED, PLAY THE FIRE TRAIL
                case BuffClass.Fire:
                    if (fireTrailFX == null)
                        return;
                    fireTrailFX.Play();
                    break;

                default:
                    break;
            }
        }
    }
}
