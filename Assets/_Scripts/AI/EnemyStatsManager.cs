using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsManager : CharacterStatsManager
{
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyBossManager enemyBossManager;
    public UIEnemyHealthBar enemyHealthBar;

    public bool isBoss;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
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
        else if (poiseResetTimer <= 0 && !enemyManager.isInteracting)
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamageNoAnimation(int damage)
    {
        base.TakeDamageNoAnimation(damage);

        if (!isBoss)
        {
            //currentHealth = currentHealth - damage;
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemyBossManager != null)
        {
            //currentHealth = currentHealth - damage;
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        //enemyHealthBar.SetHealth(currentHealth);
    }

    public void BreakGuard()
    {
        enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        /*if (isDead)
            return;*/

        base.TakeDamage(damage, damageAnimation = "Damage_01");
        
        if (!isBoss)
        {
            //currentHealth = currentHealth - damage;
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemyBossManager != null)
        {
            //currentHealth = currentHealth - damage;
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true, true);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }

    }

    private void HandleDeath()
    {
        currentHealth = 0;
        enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        isDead = true;
    }
}
