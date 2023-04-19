using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterAnimatorManager characterAnimatorManager;

    [Header("Team I.D")]
    public int teamIDNumber = 0;

   
    public int maxHealth;
    public int currentHealth;

    public int maxStamina;
    public int currentStamina;

    public int soulCount = 0;
    public int soulsAwardedOnDeath = 50;

    
    public int maxFocusPoints;
    public int currentFocusPoints;

    [Header("LEVELS")]
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int focusLevel = 10;
    public int poiseLevel = 10;
    public int strengthLevel = 10;
    public int dexterityLevel = 10;
    public int intelligenceLevel = 10;
    public int faithLevel = 10;


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

    public bool isDead;

    protected virtual void Awake()
    {
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
    }

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    public virtual void TakeDamage(int damage, int fireDamage, string damageAnimation)
    {
        if (isDead)
            return;

        characterAnimatorManager.EraseHandIKWeapon();

        float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsorptionHead / 100) *
            (1 - fireDamageAbsorptionBody / 100) *
            (1 - fireDamageAbsorptionLegs / 100) *
            (1 - fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

        float finalDamage = damage + fireDamage; //add physical damage later

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakeDamageNoAnimation(int damage, int fireDamage)
    {
        if (isDead)
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
            isDead = true;
        }
    }

    public virtual void TakePoisonDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
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

    public void DrainStaminaBasedOnAttackType()
    {

    }
}
