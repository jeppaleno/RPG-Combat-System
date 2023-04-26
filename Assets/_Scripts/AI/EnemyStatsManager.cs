using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsManager : CharacterStatsManager
{
    EnemyManager enemy;
    public UIEnemyHealthBar enemyHealthBar;

    public bool isBoss;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<EnemyManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (!isBoss)
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
        }
    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !enemy.isInteracting)
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
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemy.enemyBossManager != null)
        {
            enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (enemy.isDead)
            return;

        base.TakePoisonDamage(damage);

        if (!isBoss)
        {
            //currentHealth = currentHealth - damage;
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemy.enemyBossManager != null)
        {
            //currentHealth = currentHealth - damage;
            enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        //enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            enemy.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true, true);
            enemy.isDead = true;
        }
    }

    public void BreakGuard()
    {
        enemy.enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }

    public override void TakeDamage(int damage, int fireDamage, string damageAnimation)
    {
        base.TakeDamage(damage, fireDamage, damageAnimation);
        
        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemy.enemyBossManager != null)
        {
            enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        enemy.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true, true);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }

    }

    private void HandleDeath()
    {
        currentHealth = 0;
        enemy.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        enemy.isDead = true;
    }
}
