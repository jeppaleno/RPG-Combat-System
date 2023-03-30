using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    public string bossName;

    UIBossHealthBar bossHealthBar;
    EnemyStatsManager enemyStats;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;

    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        enemyStats = GetComponent<EnemyStatsManager>();
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

        if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
        {
            bossCombatStanceState.hasPhaseShifted = true;
            ShiftToSecondPhase();
        }
    }

    public void ShiftToSecondPhase()
    {
        //PLAY AN ANIMATION W AN EVENT THAT TRIGGES PARTICLE FX/WEAPON FX
        //enemyAnimatorManager.animator.SetBool("isInvulnerable", true);
        enemyAnimatorManager.animator.SetBool("isPhaseShifting", true);
        enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        //SWITCH ATTACK ACTIONS TO NEW PHASE PATTERNS
        bossCombatStanceState.hasPhaseShifted = true;
    }
    //handle switching attack patterns
}
