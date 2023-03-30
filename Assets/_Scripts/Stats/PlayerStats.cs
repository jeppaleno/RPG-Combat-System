using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerManager playerManager;

    public HealthBar healthbar;
    StaminaBar staminaBar;
    FocusPointBar focusPointsBar;


    AnimatorManager animatorManager;

    private WaitForSeconds regenTicks = new WaitForSeconds(0.1f);
    private Coroutine regen;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        staminaBar = FindObjectOfType<StaminaBar>();
        focusPointsBar = FindObjectOfType<FocusPointBar>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxHealthFromHealthLevel();
        currentStamina = maxStamina;

        maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
        currentFocusPoints = maxFocusPoints;
        focusPointsBar.SetMaxFocusPoints(maxFocusPoints);
        focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);
    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
        {
            totalPoiseDefence = armorPoiseBonus;
        }
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

    private int SetMaxFocusPointsFromFocusLevel()
    {
        maxFocusPoints = focusLevel * 10;
        return maxFocusPoints;
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;

        currentHealth = currentHealth - damage;

        healthbar.SetCurrentHealth(currentHealth);

        animatorManager.PlayTargetAnimation(damageAnimation, true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            animatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
            //HANDLE PLAYER DEATH

        }

    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
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

    public void HealPlayer(int healAmount)
    {
        currentHealth = currentHealth + healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthbar.SetCurrentHealth(currentHealth);
    }

    public void DeductFocusPoints(int focusPoints)
    {
        currentFocusPoints = currentFocusPoints - focusPoints;

        if (currentFocusPoints < 0)
        {
            currentFocusPoints = 0;
        }

        focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);
    }

    public void AddSouls(int souls)
    {
        soulCount = soulCount + souls;
    }
}
