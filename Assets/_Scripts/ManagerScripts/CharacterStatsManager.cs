using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    public string characterName = "";

    [Header("Team I.D")] 
    public int teamIDNumber = 0;

   
    public int maxHealth;
    public int currentHealth;

    public float maxStamina;
    public float currentStamina;

    public int currentSoulCount = 0;
    public int soulsAwardedOnDeath = 50;

    
    public int maxFocusPoints;
    public int currentFocusPoints;

    [Header("CHARACTER LEVEL")]
    public int playerLevel = 1;

    [Header("STAT LEVELS")]
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int focusLevel = 10;
    public int poiseLevel = 10;
    public int strengthLevel = 10;
    public int dexterityLevel = 10;
    public int intelligenceLevel = 10;
    public int faithLevel = 10;

    [Header("Arnmor Absorptions")]
    public float physicalDamageAbsoptionHead;
    public float physicalDamageAbsoptionBody;
    public float physicalDamageAbsoptionLegs;
    public float physicalDamageAbsoptionHands;

    [Header("Resistances")]
    public float poisonResistance;

    [Header("Poise")]
    public float totalPoiseDefence; //The TOTAL poise during damage calculation
    public float offensivePoiseBonus; //The poise you gain during an attack with a weapon
    public float armorPoiseBonus; // The Poise you GAIN from wearing what ever you have equipped
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    public float fireDamageAbsorptionHead;
    public float fireDamageAbsorptionBody;
    public float fireDamageAbsorptionLegs;
    public float fireDamageAbsorptionHands;

    [Header("Blocking Absorpions")]
    public float blockingPhysicalDamageAbsorption;
    public float blockingFireDamageAbsorption;
    public float blockingStabilityRating;

    // Any damage dealt by this player is modified by these amounts
    [Header("Damage Type Modifiers")]
    public float physicalDamagePercentageModifier = 100;
    public float fireDamagePercentageModifier = 100;

    // Incoming damage after armor calculation is modified by these values
    [Header("Damage Absorption Modifiers")]
    public float physicalAbsorptionPercentageModifier = 0;
    public float fireAbsorptionPercentageModifier = 0;

    [Header("Poison")]
    public bool isPoisoned;
    public float poisonBuildup = 0; //The build up over time that poisons the player after reaching 100
    public float poisonAmount = 100; //The amount of poison the player has to process before becoming unpoisoned


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    public virtual void TakeDamageNoAnimation(int damage, int fireDamage)
    {
        if (character.isDead)
            return;

        float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsorptionHead / 100) *
            (1 - fireDamageAbsorptionBody / 100) *
            (1 - fireDamageAbsorptionLegs / 100) *
            (1 - fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

        float finalDamage = damage + fireDamage; //add physical damage later

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            character.isDead = true;
        }
    }

    public virtual void TakePoisonDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            character.isDead = true;
        }
    }

    public virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }

    public virtual void DeductStamina(float staminaToDeduct)
    {
        currentStamina = currentStamina - staminaToDeduct;
    }

    public int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public int SetMaxFocusPointsFromFocusLevel()
    {
        maxFocusPoints = focusLevel * 10;
        return maxFocusPoints;
    }

    public virtual void HealCharacter(int healAmount)
    {
        currentHealth = currentHealth + healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
