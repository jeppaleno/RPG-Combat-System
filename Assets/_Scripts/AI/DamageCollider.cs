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
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    public int darkDamage;

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
        if (collision.tag == "Character")
        {
            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
            
            if (enemyManager != null)
            {
                if (enemyManager.isParrying)
                {
                    //Check here if you are parryable
                    characterManager.GetComponentInChildren<PlayerAnimatorManager>().PlayTargetAnimation("Parried", true); //change to getcomponent later
                    return;
                }
                else if (shield != null && enemyManager.isBlocking)
                {
                    float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                    float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;

                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock),0, "Block Guard");
                        return;
                    }
                }
            }

            if (enemyStats != null)
            {
                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;
                //Debug.Log("Player's Poise is currently" + playerStats.totalPoiseDefence);

                //Detects where on the collider the weapon first makes contact
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffects.PlayBloodSplatterFX(contactPoint);

                if (enemyStats.totalPoiseDefence > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
                    //Debug.Log("Enemy Poise is currently" + playerStats.totalPoiseDefence);
                }
                else
                {
                    enemyStats.TakeDamage(physicalDamage, 0);
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
