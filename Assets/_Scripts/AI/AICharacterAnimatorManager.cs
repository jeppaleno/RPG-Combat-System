using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    AICharacterManager AICharacter;
    
    protected override void Awake()
    {
        base.Awake();
        AICharacter = GetComponent<AICharacterManager>();
    }

    public void AwardSoulsOnDeath()
    {
        PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
        SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

        if (playerStats != null)
        {
            playerStats.AddSouls(AICharacter.aiCharacterStatsManager.soulsAwardedOnDeath);

            if (soulCountBar != null)
            {
                soulCountBar.SetSoulCountText(playerStats.currentSoulCount);
            }
        }
    }

    public void InstantiateBossParticlesFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

        GameObject phaseFX = Instantiate(AICharacter.aiCharacterBossManager.particleFX, bossFXTransform.transform);
    }

    public void PlayWeaponTrailFX()
    {
        AICharacter.aiCharacterEffectsManager.PlayWeaponFX(false);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        AICharacter.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = AICharacter.animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        AICharacter.enemyRigidBody.velocity = velocity;

        if (AICharacter.isRotatingWithRootMotion)
        {
            AICharacter.transform.rotation *= AICharacter.animator.deltaRotation;
        }
    }
}
