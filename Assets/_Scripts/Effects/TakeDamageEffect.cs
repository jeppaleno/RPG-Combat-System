using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Take Damage")]
public class TakeDamageEffect : CharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;  // IF THE DAMAGE IS CAUSED BY A CHARACTER, THEY ARE LISTED HERE

    [Header("Damage")]
    public float physicalDamage = 0;
    public float fireDamage = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelecetDamageAnimation = false;
    public string damageAnimation;

    [Header("SFX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundSFX;     // EXTRA SFX THAT IS PLAYED WHEN THERE IS ELEMENTAL DAMAGE (FIRE, MAGIC, DARKNESS, LIGHTNING)

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;       // WHERE THE DAMAGE STRIKES THE PLAYER ON THEIR BODY

    public override void ProcessEffect(CharacterManager character)
    {
        // IF THE CHARACTER IS DEAD RETURN WITHOUT RUNNING ANY LOGIC
        if (character.isDead)
            return;

        // IF THE CHARACTER IS INVULNERABLE, NO DAMAGE IS TAKEN
        if (character.isInvulnerable)
            return;

        // CALCULATE TOTAL DAMAGE AFTER DEFENSE
        CalculateDamage(character);
        // CHECK WHICH DIRECTION THE DAMAGE CAME FROM SO WE CAN PLAY THE RIGHT ANIMATION
        CheckWichDirectionDamageCameFrom(character);
        // PLAY A DAMAGE ANIMATION
        PlayDamageAnimation(character);
        // PLAY DAMAGE SFX
        PlayDamageSoundFX(character);
        // PLAY BLOOD SPLATTER FX
        PlayBloodSplatter(character);
        // IF THE CHARACTER IS A.I, ASSIGN THEM THE DAMAGING CHARACTER AS TARGET
        AssignNewAITarget(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        // Before calculating damage defense, we check the attacking characters modifiers
        if (characterCausingDamage != null)
        {
            physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
            fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));
        }

        character.characterAnimatorManager.EraseHandIKWeapon();

        float totalPhysicalDamageAbsorptions = 1 -
            (1 - character.characterStatsManager.physicalDamageAbsoptionHead / 100) *
            (1 - character.characterStatsManager.physicalDamageAbsoptionBody / 100) *
            (1 - character.characterStatsManager.physicalDamageAbsoptionLegs / 100) *
            (1 - character.characterStatsManager.physicalDamageAbsoptionHands / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorptions));

        float totalFireDamageAbsorption = 1 -
            (1 - character.characterStatsManager.fireDamageAbsorptionHead / 100) *
            (1 - character.characterStatsManager.fireDamageAbsorptionBody / 100) *
            (1 - character.characterStatsManager.fireDamageAbsorptionLegs / 100) *
            (1 - character.characterStatsManager.fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

        physicalDamage = physicalDamage - Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100));
        fireDamage = fireDamage - Mathf.RoundToInt(fireDamage * (character.characterStatsManager.fireAbsorptionPercentageModifier / 100));

        float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightingDamage + darkDamage

        character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);

        if (character.characterStatsManager.totalPoiseDefence < poiseDamage)
        {
            poiseIsBroken = true;
        }

        if (character.characterStatsManager.currentHealth <= 0)
        {
            character.characterStatsManager.currentHealth = 0;
            character.isDead = true;
        }
    }

    private void CheckWichDirectionDamageCameFrom(CharacterManager character)
    {
        if (manuallySelecetDamageAnimation)
            return;

        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            ChooseDamageAnimationForward(character);
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180)
        {
            ChooseDamageAnimationForward(character);
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            ChooseDamageAnimationBackward(character);
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            ChooseDamageAnimationLeft(character);
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            ChooseDamageAnimationRight(character);
        }
    }

    private void ChooseDamageAnimationForward(CharacterManager character)
    {
        // POISE BRACKET < 25       SMALL
        // POISE BRACKET > 25 < 50  MEDIUM
        // POISE BRACKET > 50 < 75  LARGE
        // POISE BRACKET > 75       COLOSAL

        if (poiseDamage <= 24 && poiseDamage >= 0)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Light_Forward);
            return;
        }
        else if (poiseDamage <= 49 && poiseDamage >= 25)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Forward);
            return;
        }
        else if (poiseDamage <= 74 && poiseDamage >= 50)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Forward);
            return;
        }
        else if (poiseDamage >= 75)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Forward);
            return;
        }
    }

    private void ChooseDamageAnimationBackward(CharacterManager character)
    {
        // POISE BRACKET < 25       SMALL
        // POISE BRACKET > 25 < 50  MEDIUM
        // POISE BRACKET > 50 < 75  LARGE
        // POISE BRACKET > 75       COLOSAL

        if (poiseDamage <= 24 && poiseDamage >= 0)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Light_Backward);
            return;
        }
        else if (poiseDamage <= 49 && poiseDamage >= 25)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Backward);
            return;
        }
        else if (poiseDamage <= 74 && poiseDamage >= 50)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Backward);
            return;
        }
        else if (poiseDamage >= 75)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Backward);
            return;
        }
    }

    private void ChooseDamageAnimationLeft(CharacterManager character)
    {
        // POISE BRACKET < 25       SMALL
        // POISE BRACKET > 25 < 50  MEDIUM
        // POISE BRACKET > 50 < 75  LARGE
        // POISE BRACKET > 75       COLOSAL

        if (poiseDamage <= 24 && poiseDamage >= 0)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Light_Left);
            return;
        }
        else if (poiseDamage <= 49 && poiseDamage >= 25)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Left);
            return;
        }
        else if (poiseDamage <= 74 && poiseDamage >= 50)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Left);
            return;
        }
        else if (poiseDamage >= 75)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Left);
            return;
        }
    }

    private void ChooseDamageAnimationRight(CharacterManager character)
    {
        // POISE BRACKET < 25       SMALL
        // POISE BRACKET > 25 < 50  MEDIUM
        // POISE BRACKET > 50 < 75  LARGE
        // POISE BRACKET > 75       COLOSAL

        if (poiseDamage <= 24 && poiseDamage >= 0)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Light_Right);
            return;
        }
        else if (poiseDamage <= 49 && poiseDamage >= 25)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Right);
            return;
        }
        else if (poiseDamage <= 74 && poiseDamage >= 50)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Right);
            return;
        }
        else if (poiseDamage >= 75)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Right);
            return;
        }
    }

    private void PlayDamageSoundFX(CharacterManager character)
    {
        character.characterSoundFXManager.PlayRandomDamageSoundFX();

        if (fireDamage > 0)
        {
            character.characterSoundFXManager.PlaySoundFX(elementalDamageSoundSFX);
        }
    }

    private void PlayDamageAnimation(CharacterManager character)
    {
        // IF WE ARE CURRENTLY PLAYING A DAMAGE ANIMATION THAT IS HEAVY AND A LIGHT ATTACK HITS US
        // WE DO NOT WANT TO PLAY THE LIGHT DAMAGE ANIMATION, WE WANT TO FINISH THE HEAVY ANIMATION
        if (character.isInteracting && character.characterCombatManager.previousPoiseDamageTaken > poiseDamage)
        {
            // IF THE CHARACTER IS INTERACTING && THE PREVIOUS POISE DAMAGE IS ABOVE 0, THEY MUST BE IN A DAMAGE ANIMATION
            // IF THE PREVIOUS POISE IS ABOVE THE CURRENT POISE, RETURN, SO WE DONT CHANGE THE DAMAGE ANIMATION TO A LIGHTER ANIMATION
            return;
        }

        if (character.isDead)
        {
            character.characterWeaponSlotManager.CloseDamageCollider();
            character.characterAnimatorManager.PlayTargetAnimation("Dead_01", true, true);
            return;
        }

        // IF THE CHARACTERS POISE IS NOT BROKEN, NO DAMAGE ANIMATION IS PLAYED
        if (!poiseIsBroken)
        {
            return;
        }
        else
        {
            // ENABLE/DISABLE STUN LOCK

            if (playDamageAnimation)
            {
                character.characterAnimatorManager.PlayTargetAnimation(damageAnimation, true);
            }
        }
    }

    private void PlayBloodSplatter(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterFX(contactPoint);
    }

    private void AssignNewAITarget(CharacterManager character)
    {
        AICharacterManager aiCharacter = character as AICharacterManager;

        if (aiCharacter != null && characterCausingDamage != null)
        {
            aiCharacter.currentTarget = characterCausingDamage;
        }
    }
}
