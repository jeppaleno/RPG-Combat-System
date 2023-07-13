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
    public float poiseDamage;
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

    protected Vector3 contactPoint;
    protected float angleHitFrom;

    private List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();

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
        if (charactersDamagedDuringThisCalculation.Count > 0)
        {
            charactersDamagedDuringThisCalculation.Clear();
        }
        
        damageCollider.enabled = false;
    }
    
    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;

            CharacterManager enemyManager = collision.GetComponentInParent<CharacterManager>();
            
            if (enemyManager != null)
            {
                AICharacterManager aiCharacter = enemyManager as AICharacterManager;

                if (charactersDamagedDuringThisCalculation.Contains(enemyManager))
                    return;

                charactersDamagedDuringThisCalculation.Add(enemyManager);

                if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                    return;

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager);

                if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                    return;

                if (hasBeenParried)
                    return;

                if (shieldHasBeenHit)
                    return;

                enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
                enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseDefence - poiseDamage;

                contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                angleHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                DealDamage(enemyManager);

                if (aiCharacter != null)
                {
                    // If target is AI, the AI receives a new target, the person dealing damage to it
                    aiCharacter.currentTarget = characterManager;
                }
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
        Vector3 directionFromPlayerToEnemy = (characterManager.transform.position - enemyManager.transform.position);
        float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

        if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f) //Experiment with this number til it's right
        {
            shieldHasBeenHit = true;

            TakeBlockedDamageEffect takeBlockedDamage = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);
            takeBlockedDamage.physicalDamage = physicalDamage;
            takeBlockedDamage.fireDamage = fireDamage;
            takeBlockedDamage.poiseDamage = poiseDamage;
            takeBlockedDamage.staminaDamage = poiseDamage;

            enemyManager.characterEffectsManager.ProcessEffectInstantly(takeBlockedDamage);
        }
    }

    protected virtual void DealDamage(CharacterManager enemyManager)
    {
        float finalPhysicalDamage = physicalDamage;
        float finalFireDamage = fireDamage;

        //IF WE ARE USING THE RIGHT WEAPON, WE COMPARE THE RIGHT WEAPON MODIFIERS
        if (characterManager.isUsingRightHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                finalFireDamage = finalFireDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                finalFireDamage = finalFireDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
            }
        }
        //OTHERWISE WE COMPARE THE LEFT WEAPON MODIFIERS
        else if (characterManager.isUsingLeftHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                finalFireDamage = finalFireDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                finalFireDamage = finalFireDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
            }
        }

        TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        takeDamageEffect.physicalDamage = finalPhysicalDamage;
        takeDamageEffect.fireDamage = finalFireDamage;
        takeDamageEffect.poiseDamage = poiseDamage;
        takeDamageEffect.contactPoint = contactPoint;
        takeDamageEffect.angleHitFrom = angleHitFrom;
        enemyManager.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);   
    }
}
