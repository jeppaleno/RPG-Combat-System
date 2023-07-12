using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager player;

    public HealthBar healthbar;
    public StaminaBar staminaBar;
    public FocusPointBar focusPointsBar;

    public float staminaRegenerationAmount = 1;
    public float staminaRegenerationAmountWhilstBlocking = 0.1f;
    private float staminaRegenTimer = 0;

    private float sprintingTimer = 0;

    //private WaitForSeconds regenTicks = new WaitForSeconds(0.1f);
    //private Coroutine regen;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        staminaBar = FindObjectOfType<StaminaBar>();
        focusPointsBar = FindObjectOfType<FocusPointBar>();
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
        else if(poiseResetTimer <= 0 && !player.isInteracting)
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (player.isDead)
            return;

        base.TakePoisonDamage(damage);
        healthbar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            player.playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
            player.isDead = true;
        }
    }
    public override void TakeDamageNoAnimation(int damage, int fireDamage)
    {
        base.TakeDamageNoAnimation(damage, fireDamage);
        healthbar.SetCurrentHealth(currentHealth);
    }

    public override void DeductStamina(float staminaToDeduct)
    {
        base.DeductStamina(staminaToDeduct);
        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
    }

    public void DeductSprintingStamina(float staminaToDeduct)
    {
        if (player.isSprinting)
        {
            sprintingTimer = sprintingTimer + Time.deltaTime;

            if (sprintingTimer > 0.1f)
            {
                //Reset Timer
                sprintingTimer = 0;
                // Deduct Stamina
                currentStamina = currentStamina - staminaToDeduct;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
        else
        {
            sprintingTimer = 0;
        }
    }
    public void RegenerateStamina()
    {
        if (player.isInteracting || player.isSprinting)
        {
            staminaRegenTimer = 0;
        }
        else
        {
            staminaRegenTimer += Time.deltaTime;

            if (currentStamina < maxStamina && staminaRegenTimer > 1f)
            {
                if (player.isBlocking)
                {
                    currentStamina += staminaRegenerationAmountWhilstBlocking * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
                else
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
                
            }
        }
    }

    public override void HealCharacter(int healAmount)
    {
        base.HealCharacter(healAmount);

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
