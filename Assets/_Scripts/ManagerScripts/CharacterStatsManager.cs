using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public int maxStamina;
    public int currentStamina;

    public int soulCount = 0;
    public int soulsAwardedOnDeath = 50;

    public int focusLevel = 10;
    public int maxFocusPoints;
    public int currentFocusPoints;

    [Header("Poise")]
    public float totalPoiseDefence; //The TOTAL poise during damage calculation
    public float offensivePoiseBonus; //The poise you gain during an attack with a weapon
    public float armorPoiseBonus; // The Poise you GAIN from wearing what ever you have equipped
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    public bool isDead;

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }
    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        
    }

    public virtual void TakeDamageNoAnimation(int damage)
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
}
