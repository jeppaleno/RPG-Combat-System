using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
    EnemyAnimatorManager enemyAnimatorManager;

    public UIEnemyHealthBar enemyHealthBar;

    public int soulsAwardedOnDeath = 50;

    private void Awake()
    {
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    void Start()
    {
        enemyHealthBar.SetMaxHealth(maxHealth);
        
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;

        currentHealth = currentHealth - damage;
        enemyHealthBar.SetHealth(currentHealth);

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
