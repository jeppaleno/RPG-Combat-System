using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterStatsManager characterStatsManager;

    [Header("Damage FX")]
    public GameObject bloodSplatterFX;

    [Header("Weapon FX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;

    [Header("Poison")]
    public GameObject defaultPoisonParticleFX; //Instantiate this
    private GameObject currentPoisonParticleFX; //Destroy this
    public Transform buildUpTransform; // The location build up particle FX will spawn

    public bool isPoisoned;
    public float poisonBuildup = 0; //The build up over time that poisons the player after reaching 100
    public float poisonAmount = 100; //The amount of poison the player has to process before becoming unpoisoned
    public float defaultPoisonAmount; //The default amount of poison a player has to process once they become poisoned
    public float poisonTimer = 2; //The amount of time between each poison damage Tick
    public int poisonDamage = 1; //Choose number that fits later
    float timer;

    protected virtual void Awake()
    {
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }
    public virtual void PlayWeaponFX(bool isLeft)
    {
        if (isLeft == false)
        {
            if (rightWeaponFX != null)
            {
                rightWeaponFX.PlayWeaponFX();
            }
        }
        else
        {
            if (leftWeaponFX != null)
            {
                leftWeaponFX.PlayWeaponFX();
            }
        }
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }

    public virtual void HandleAllBuildEffects()
    {
        if (characterStatsManager.isDead)
            return;

        HandlePoisonBuildUp();
        HandleIsPoisonedEffect();
    }

    protected virtual void HandlePoisonBuildUp()
    {
        if (isPoisoned) //Do not assign build up if we already poisoned
            return;

        if (poisonBuildup > 0 && poisonBuildup < 100)
        {
            poisonBuildup = poisonBuildup - 1 * Time.deltaTime;
        }
        else if (poisonBuildup >= 100)
        {
            isPoisoned = true;
            poisonBuildup = 0;

            if (buildUpTransform != null)
            {
                currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
            }
            else
            {
                currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, characterStatsManager.transform);
            }
        }
    }

    protected virtual void HandleIsPoisonedEffect()
    {
        if (isPoisoned)
        {
            if (poisonAmount > 0)
            {
                timer += Time.deltaTime;

                if (timer >= poisonTimer)
                {
                    characterStatsManager.TakePoisonDamage(poisonDamage);
                    timer = 0;
                }
                
                poisonAmount = poisonAmount - 1 * Time.deltaTime;
            }
            else
            {
                isPoisoned = false;
                poisonAmount = defaultPoisonAmount;
                Destroy(currentPoisonParticleFX);
            }
        }
    }
}
