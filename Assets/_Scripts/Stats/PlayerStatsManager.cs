using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager playerManager;

    public HealthBar healthbar;
    StaminaBar staminaBar;
    FocusPointBar focusPointsBar;


    PlayerAnimatorManager playerAnimatorManager;

    private WaitForSeconds regenTicks = new WaitForSeconds(0.1f);
    private Coroutine regen;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        staminaBar = FindObjectOfType<StaminaBar>();
        focusPointsBar = FindObjectOfType<FocusPointBar>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
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



    public override void TakeDamage(int damage, int fireDamage, string damageAnimation)
    {
        if (isDead)
            return;

        currentHealth = currentHealth - damage; //REFACTOR AND CALL BASE INSTEAD
        healthbar.SetCurrentHealth(currentHealth);
        playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
            //HANDLE PLAYER DEATH
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (isDead)
            return;

        base.TakePoisonDamage(damage);
        healthbar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }
    public override void TakeDamageNoAnimation(int damage, int fireDamage)
    {
        base.TakeDamageNoAnimation(damage, fireDamage);
        healthbar.SetCurrentHealth(currentHealth);
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
        currentSoulCount = currentSoulCount + souls;
    }
}
