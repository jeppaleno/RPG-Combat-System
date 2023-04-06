using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : DamageCollider
{
    public GameObject impactParticles;
    public GameObject projectileParticles;
    public GameObject muzzleParticles;

    bool hasCollided = false;

    CharacterStatsManager spellTarget;
    Rigidbody rigidBody;

    Vector3 impactNormal; // used to rotate impact particles

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
        projectileParticles.transform.parent = transform;

        if(muzzleParticles)
        {
            muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
            Destroy(muzzleParticles, 2f); // How long the muzzle particles last
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            spellTarget = collision.transform.GetComponent<CharacterStatsManager>();

            if (spellTarget != null && spellTarget.teamIDNumber != teamIDNumber)
            {
                spellTarget.TakeDamage(0, fireDamage, currentDamageAnimation);
            }

            hasCollided = true;
            impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

            Destroy(projectileParticles);
            Destroy(impactParticles, 5f);
            Destroy(gameObject, 5f);

        }
    }
}
