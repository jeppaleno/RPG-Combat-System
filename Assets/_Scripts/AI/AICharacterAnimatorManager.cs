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

    public override void OnAnimatorMove()
    {
        if (character.isUsingRootMotion)
        {
            if (character.isInteracting == false)
                return;

            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);
            
        }
        
        if (AICharacter.isRotatingWithRootMotion)
        {
            character.transform.rotation *= character.animator.deltaRotation;
        }
    }
}
