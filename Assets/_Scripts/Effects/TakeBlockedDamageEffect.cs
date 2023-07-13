using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Take Blocked Damage")]
public class TakeBlockedDamageEffect : CharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;  // IF THE DAMAGE IS CAUSED BY A CHARACTER, THEY ARE LISTED HERE

    [Header("Base Damage")]
    public float physicalDamage = 0;
    public float fireDamage = 0;
    public float staminaDamage = 0;
    public float poiseDamage = 0;

    [Header("Animation")]
    public string blockAnimation;

    public override void ProcessEffect(CharacterManager character)
    {
        // IF THE CHARACTER IS DEAD RETURN WITHOUT RUNNING ANY LOGIC
        if (character.isDead)
            return;

        // IF THE CHARACTER IS INVULNERABLE, NO DAMAGE IS TAKEN
        if (character.isInvulnerable)
            return;

        CalculateDamage(character);
        CalculateStaminaDamage(character);
        DecideBlockAnimationBasedOnPoiseDamage(character);
        PlayBlockSoundFX(character);
        AssignNewAITarget(character);

        if (character.isDead)
        {
            character.characterAnimatorManager.PlayTargetAnimation("Dead_01", true, true);
        }
        else
        {
            if (character.characterStatsManager.currentStamina <= 0)
            {
                character.characterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true, true);
                //character.canBeRiposted = true;
                //character.characterSoundFXManager. Play guard break sound
                character.isBlocking = false;
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true, true);
                character.isAttacking = false;
            }
        }
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (character.isDead)
            return;

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

        Debug.Log("Total Damage Absoption is" + totalPhysicalDamageAbsorptions + "%");

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

        if (character.characterStatsManager.currentHealth <= 0)
        {
            character.characterStatsManager.currentHealth = 0;
            character.isDead = true;
        }
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        float staminaDamageAbsorption = staminaDamage * (character.characterStatsManager.blockingStabilityRating / 100);
        float staminaDamageAfterAbsorption = staminaDamage - staminaDamageAbsorption;
        character.characterStatsManager.currentStamina -= staminaDamageAfterAbsorption;
    }

    private void DecideBlockAnimationBasedOnPoiseDamage(CharacterManager character) // TO DO: ADD MORE BLOCK ANIMS --------------
    {
        if (!character.isTwoHandingWeapon)
        {
            // POISE BRACKET < 25       SMALL
            // POISE BRACKET > 25 < 50  MEDIUM
            // POISE BRACKET > 50 < 75  LARGE
            // POISE BRACKET > 75       COLOSAL

            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                blockAnimation = "OH_Block_Guard_Light_01";
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                blockAnimation = "OH_Block_Guard_Light_01";
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                blockAnimation = "OH_Block_Guard_Light_01";
                return;
            }
            else if (poiseDamage >= 75)
            {
                blockAnimation = "OH_Block_Guard_Light_01";
                return;
            }
        }
        else
        {
            // POISE BRACKET < 25       SMALL
            // POISE BRACKET > 25 < 50  MEDIUM
            // POISE BRACKET > 50 < 75  LARGE
            // POISE BRACKET > 75       COLOSAL

            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                blockAnimation = "TH_Block_Guard_Light_01";
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                blockAnimation = "TH_Block_Guard_Light_01";
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                blockAnimation = "TH_Block_Guard_Light_01";
                return;
            }
            else if (poiseDamage >= 75)
            {
                blockAnimation = "TH_Block_Guard_Light_01";
                return;
            }
        }
    }

    private void PlayBlockSoundFX(CharacterManager character)
    {
        // IF WE ARE BLOCKING WITH OUR RIGHT HANDED WEAPON
        if(character.isTwoHandingWeapon)
        {
            character.characterSoundFXManager.PlayRandomSOundFXFromArray(character.characterInventoryManager.rightWeapon.blockingNoises);
        }
        // IF WE ARE BLOCKING WITH OUR OFF (LEFT) HANDED WEAPON
        else
        {
            character.characterSoundFXManager.PlayRandomSOundFXFromArray(character.characterInventoryManager.leftWeapon.blockingNoises);
        }
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
