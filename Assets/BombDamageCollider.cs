using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamageCollider : DamageCollider
{
    [Header("Explosive Damage & Radius")]
    public int explosiveRadius = 1;
    public int fireExplosionDamage;
    //magicExplosionDamage
    //lightiningExplosionDamage

    public Rigidbody bombRigidBody;
    private bool hasCollided = false;
    public GameObject impactParticles;

    protected override void Awake()
    {
        damageCollider = GetComponent<Collider>();
        bombRigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
            Explode();

            CharacterStatsManager character = collision.transform.GetComponent<CharacterStatsManager>();

            if(character != null)
            {
                //Check for friendly fire
                //character.TakeDamage();
            }

            Destroy(impactParticles, 5f);
            Destroy(transform.parent.gameObject);
        }
    }

    private void Explode()
    {
        Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

        foreach (Collider objectsInExplosion in characters)
        {
            CharacterStatsManager character = objectsInExplosion.GetComponent<CharacterStatsManager>();

            if (character != null)
            {
                //Deal fire damage
            }
        }
    }
}
