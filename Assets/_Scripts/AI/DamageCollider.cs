using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    protected Collider damageCollider;
    public bool enabledDamageColliderOnStartUp = false;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Damage")]
    public int currentWeaponDamage = 25;

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = enabledDamageColliderOnStartUp;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
            CharacterManager playerCharacterManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager playerEffectsManager = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
            
            if (playerCharacterManager != null)
            {
                if (playerCharacterManager.isParrying)
                {
                    //Check here if you are parryable
                    characterManager.GetComponentInChildren<PlayerAnimatorManager>().PlayTargetAnimation("Parried", true); //change to getcomponent later
                    return;
                }
                else if (shield != null && playerCharacterManager.isBlocking)
                {
                    float physicalDamageAfterBlock = 
                        currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                        return;
                    }
                }
            }

            if (playerStats != null)
            {
                playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                playerStats.totalPoiseDefence = playerStats.totalPoiseDefence - poiseBreak;
                //Debug.Log("Player's Poise is currently" + playerStats.totalPoiseDefence);

                //Detects where on the collider the weapon first makes contact
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                playerEffectsManager.PlayBloodSplatterFX(contactPoint);

                if (playerStats.totalPoiseDefence > poiseBreak)
                {
                    playerStats.TakeDamageNoAnimation(currentWeaponDamage);
                    //Debug.Log("Enemy Poise is currently" + playerStats.totalPoiseDefence);
                }
                else
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }
        }

        if (collision.tag == "Enemy")
        {
            EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffectsManager = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    //Check here if you are parryable
                    characterManager.GetComponentInChildren<PlayerAnimatorManager>().PlayTargetAnimation("Parried", true); //change to getcomponent later
                    return;
                }
                else if (shield != null && enemyCharacterManager.isBlocking)
                {
                    float physicalDamageAfterBlock =
                        currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                        return;
                    }
                }
            }

            if (enemyStats != null)
            {
                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;
                //Debug.Log("Enemies's Poise is currently" + enemyStats.totalPoiseDefence);

                //Detects where on the collider the weapon first makes contact
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffectsManager.PlayBloodSplatterFX(contactPoint);

                if (enemyStats.isBoss)
                {
                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        //Debug.Log("Enemy Poise is currently" + enemyStats.totalPoiseDefence);
                    }
                    else
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        enemyStats.BreakGuard();
                    }
                }
                else
                {
                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        Debug.Log("Enemy Poise is currently" + enemyStats.totalPoiseDefence);
                    }
                    else
                    {
                        enemyStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }
        }

        if (collision.tag == "Illusionary Wall")
        {
            IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

            illusionaryWall.wallHasBeenHit = true;
        }
    }
}
