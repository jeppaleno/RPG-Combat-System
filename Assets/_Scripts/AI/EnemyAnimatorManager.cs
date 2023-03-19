using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : PreAnimatorManager
{
    EnemyManager enemyManager;
    EnemyStats enemyStats;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        enemyStats = GetComponent<EnemyStats>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
        enemyManager.pendingCriticalDamage = 0;
    }

    public void CanRotate()
    {
        animator.SetBool("canRotate", true);
    }

    public void stopRotation()
    {
        animator.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public void EnableIsParrying()
    {
        enemyManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        enemyManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        enemyManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        enemyManager.canBeRiposted = false;
    }

    public void AwardSoulsOnDeath()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

        if (playerStats != null)
        {
            playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

            if (soulCountBar != null)
            {
                soulCountBar.SetSoulCountText(playerStats.soulCount);
            }
        }
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidBody.velocity = velocity;

        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= animator.deltaRotation;
        }
    }
}
