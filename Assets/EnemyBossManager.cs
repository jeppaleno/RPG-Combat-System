using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    public string bossName;

    UIBossHealthBar bossHealthBar;
    EnemyStats enemyStats;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;

    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);

        if (currentHealth <= maxHealth / 2)
        {
            ShiftToSecondPhase();
        }
    }

    public void ShiftToSecondPhase()
    {
        //PLAY AN ANIMATION W AN EVENT THAT TRIGGES PARTICLE FX/WEAPON FX
        enemyAnimatorManager.PlayTargetAnimation("Phase shift", true);
        //SWITCH ATTACK ACTIONS TO NEW PHASE PATTERNS
        bossCombatStanceState.hasPhaseShifted = true;
    }
    //handle switching attack patterns
}
