using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyBossManager enemyBossManager;
    public UIEnemyHealthBar enemyHealthBar;
    public int soulsAwardedOnDeath = 50;

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

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
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
