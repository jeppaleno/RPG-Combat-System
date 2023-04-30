using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    protected Collider damageCollider;
    public bool enabledDamageColliderOnStartUp = false;

    [Header("Team I.D")]
    public int teamIDNumber = 0;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    public int darkDamage;

    [Header("Guard Break Modifier")]
    public float guardBreakModifier = 1;

    protected bool shieldHasBeenHit;
    protected bool hasBeenParried;
    protected string currentDamageAnimation;

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

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Character")
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;

            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
            
            if (enemyManager != null)
            {
                if (enemyStats.teamIDNumber == teamIDNumber)
                    return;

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager);
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

                //Deals Damage
                DealDamage(enemyStats);
            }
        }

       

        if (collision.tag == "Illusionary Wall")
        {
            IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

            illusionaryWall.wallHasBeenHit = true;
        }
    }

    protected virtual void CheckForParry(CharacterManager enemyManager)
    {
        if (enemyManager.isParrying)
        {
            //Check here if you are parryable
            characterManager.GetComponentInChildren<PlayerAnimatorManager>().PlayTargetAnimation("Parried", true, true);
            hasBeenParried = true;
        }
    }

    protected virtual void CheckForBlock(CharacterManager enemyManager)
    {
        CharacterStatsManager enemyShield = enemyManager.characterStatsManager;
        Vector3 directionFromPlayerToEnemy = (characterManager.transform.position - enemyManager.transform.position);
        float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

        if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f) //Experiment with this number til it's right
        {
            shieldHasBeenHit = true;
            float physicalDamageAfterBlock = physicalDamage - (physicalDamage * enemyShield.blockingPhysicalDamageAbsorption) / 100;
            float fireDamageAfterBlock = fireDamage - (fireDamage * enemyShield.blockingFireDamageAbsorption) / 100;

            //ATTEMPT TO BLOCK THE ATTACK (cHECK FOR GUARD BREAK)
            enemyManager.characterCombatManager.AttemptBlock(this, physicalDamageAfterBlock, fireDamageAfterBlock, "Block_01");
            enemyShield.TakeDamageAfterBlock(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock), characterManager); 
        }
    }

    protected virtual void DealDamage(CharacterStatsManager enemyStats)
    {
        float finalPhysicalDamage = physicalDamage;

        //IF WE ARE USING THE RIGHT WEAPON, WE COMPARE THE RIGHT WEAPON MODIFIERS
        if (characterManager.isUsingRightHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
            }
        }
        //OTHERWISE WE COMPARE THE LEFT WEAPON MODIFIERS
        else if (characterManager.isUsingLeftHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
            }
        }
        
        //DEAL MODIFIED DAMAGE
        if (enemyStats.totalPoiseDefence > poiseBreak)
        {
            enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalPhysicalDamage), 0);
            //Debug.Log("Enemy Poise is currently" + playerStats.totalPoiseDefence);
        }
        else
        {
            enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), 0, currentDamageAnimation, characterManager);
        }
    }

    protected virtual void ChooseWhichDirectionDamageCameFrom(float direction)
    {
        //Debug.Log(direction);

        if (direction >= 145 && direction <= 180)
        {
            currentDamageAnimation = "Damage_Forward_01";
        }
        else if (direction <= -145 && direction >= -180)
        {
            currentDamageAnimation = "Damage_Forward_01";
        }
        else if (direction >= -45 && direction <= 45)
        {
            currentDamageAnimation = "Damage_Back_01";
        }
        else if (direction >= -144 && direction <= -45)
        {
            currentDamageAnimation = "Damage_Left_01";
        }
        else if (direction >= 45 && direction <= 144)
        {
            currentDamageAnimation = "Damage_Right_01";
        }
    }
}
