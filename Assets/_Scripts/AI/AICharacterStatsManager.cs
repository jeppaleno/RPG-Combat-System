using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterStatsManager : CharacterStatsManager
{
    AICharacterManager aiCharacter;
    public UIAICharacterHealthBar aiCharacterHealthBar;

    public bool isBoss;

    protected override void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AICharacterManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (!isBoss)
        {
            aiCharacterHealthBar.SetMaxHealth(maxHealth);
        }
    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !aiCharacter.isInteracting)
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamageNoAnimation(int damage, int fireDamage)
    {
        base.TakeDamageNoAnimation(damage, fireDamage);

        if (!isBoss)
        {
            aiCharacterHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && aiCharacter.aiCharacterBossManager != null)
        {
            aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (aiCharacter.isDead)
            return;

        base.TakePoisonDamage(damage);

        if (!isBoss)
        {
            //currentHealth = currentHealth - damage;
            aiCharacterHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && aiCharacter.aiCharacterBossManager != null)
        {
            //currentHealth = currentHealth - damage;
            aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        //enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Dead_01", true, true);
            aiCharacter.isDead = true;
        }
    }

    public void BreakGuard()
    {
        aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }

    public override void TakeDamage(int damage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
    {
        base.TakeDamage(damage, fireDamage, damageAnimation, enemyCharacterDamagingMe);
        
        if (!isBoss)
        {
            aiCharacterHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && aiCharacter.aiCharacterBossManager != null)
        {
            aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(damageAnimation, true, true);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }

    }

    private void HandleDeath()
    {
        currentHealth = 0;
        aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Dead_01", true);
        aiCharacter.isDead = true;
    }
}
