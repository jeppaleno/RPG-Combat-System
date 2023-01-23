using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    
    public HealthBar healthbar;
    StaminaBar staminaBar;

    AnimatorManager animatorManager;

    private WaitForSeconds regenTicks = new WaitForSeconds(0.1f);
    private Coroutine regen;

    private void Awake()
    {
        staminaBar = FindObjectOfType<StaminaBar>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxHealthFromHealthLevel();
        currentStamina = maxStamina;
        
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        healthbar.SetCurrentHealth(currentHealth);

        animatorManager.PlayTargetAnimation("Damage_01", true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            animatorManager.PlayTargetAnimation("Dead_01", true);
            //HANDLE PLAYER DEATH

        }

    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);

        if (regen != null)
        {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(StaminaRegen());

    }

    private IEnumerator StaminaRegen()
    {
        yield return new WaitForSeconds(0.5f);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 40;
            staminaBar.SetCurrentStamina(currentStamina);
            yield return regenTicks;
        }

        regen = null;
    }
}
