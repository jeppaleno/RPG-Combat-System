using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    public RangedAmmoItem ammoItem;
    protected bool hasAlreadyPenetratedASurface;

    Rigidbody arrowRigidbody;
    CapsuleCollider arrowCapsuleCollider;

    protected override void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.enabled = true;
        arrowCapsuleCollider = GetComponent<CapsuleCollider>();
        arrowRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        shieldHasBeenHit = false;
        hasBeenParried = false;

        CharacterManager enemyManager = collision.gameObject.GetComponentInParent<CharacterManager>();

        if (enemyManager != null)
        {
            if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                return;

            CheckForParry(enemyManager);
            CheckForBlock(enemyManager);

            if (hasBeenParried)
                return;

            if (shieldHasBeenHit)
                return;

            enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
            enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseDefence - poiseBreak;
            //Debug.Log("Player's Poise is currently" + playerStats.totalPoiseDefence);

            //Detects where on the collider the weapon first makes contact
            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
            ChooseWhichDirectionDamageCameFrom(directionHitFrom);
            enemyManager.characterEffectsManager.PlayBloodSplatterFX(contactPoint);

            if (enemyManager.characterStatsManager.totalPoiseDefence > poiseBreak)
            {
                enemyManager.characterStatsManager.TakeDamageNoAnimation(physicalDamage, 0);
                //Debug.Log("Enemy Poise is currently" + playerStats.totalPoiseDefence);
            }
            else
            {
                enemyManager.characterStatsManager.TakeDamage(physicalDamage, 0, currentDamageAnimation, characterManager);
            }
        }

        

        if (collision.gameObject.tag == "Illusionary Wall")
        {
            IllusionaryWall illusionaryWall = collision.gameObject.GetComponent<IllusionaryWall>();

            illusionaryWall.wallHasBeenHit = true;
        }

        if (!hasAlreadyPenetratedASurface)
        {
            hasAlreadyPenetratedASurface = true;
            arrowRigidbody.isKinematic = true;
            arrowCapsuleCollider.enabled = false;

            gameObject.transform.position = collision.GetContact(0).point;
            gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
            gameObject.transform.parent = collision.collider.transform;
        }
    }

    private void FixedUpdate()
    {
        if (arrowRigidbody.velocity != Vector3.zero)
        {
            arrowRigidbody.rotation = Quaternion.LookRotation(arrowRigidbody.velocity);
        }
    }
}
