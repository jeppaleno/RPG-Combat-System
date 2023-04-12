using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    public RangedAmmoItem ammoItem;
    protected bool hasAlreadyPenetratedASurface;
    protected GameObject penetratedProjectile;

    protected override void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Character")
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;

            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyManager != null)
            {
                if (enemyStats.teamIDNumber == teamIDNumber)
                    return;

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager, enemyStats, shield);
            }

            if (enemyStats != null)
            {
                if (enemyStats.teamIDNumber == teamIDNumber)
                    return;

                if (hasBeenParried)
                    return;

                if (shieldHasBeenHit)
                    return;

                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;
                //Debug.Log("Player's Poise is currently" + playerStats.totalPoiseDefence);

                //Detects where on the collider the weapon first makes contact
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                enemyEffects.PlayBloodSplatterFX(contactPoint);

                if (enemyStats.totalPoiseDefence > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
                    //Debug.Log("Enemy Poise is currently" + playerStats.totalPoiseDefence);
                }
                else
                {
                    enemyStats.TakeDamage(physicalDamage, 0, currentDamageAnimation);
                }
            }
        }

        if (collision.tag == "Illusionary Wall")
        {
            IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

            illusionaryWall.wallHasBeenHit = true;
        }

        if (!hasAlreadyPenetratedASurface && penetratedProjectile == null)
        {
            hasAlreadyPenetratedASurface = true;

            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            GameObject penetratedArrow = Instantiate(ammoItem.penetradedModel, contactPoint, Quaternion.Euler(0, 0, 0));

            penetratedProjectile = penetratedArrow;
            penetratedArrow.transform.parent = collision.transform;
            penetratedArrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
        }

        Destroy(transform.root.gameObject);
    }
}
