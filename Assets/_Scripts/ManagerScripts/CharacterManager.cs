using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Animator animator;
    public CharacterAnimatorManager characterAnimatorManager;
    public CharacterWeaponSlotManager characterWeaponSlotManager;
    public CharacterStatsManager characterStatsManager;
    public CharacterInventoryManager characterInventoryManager;
    public CharacterEffectsManager characterEffectsManager;
    public CharacterSoundFXManager characterSoundFXManager;
    public CharacterCombatManager characterCombatManager;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Ray Casts")]
    public Transform criticalRayCastStartPoint;

    [Header("Combat Colliders")]
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider riposteCollider;

    [Header("Interaction")]
    public bool isInteracting;
    public bool isUsingRootMotion;

    [Header("Status")]
    public bool isDead;

    [Header("Combat Flags")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool canDoCombo;
    public bool isParrying;
    public bool isBlocking;
    public bool isInvulnerable;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isHoldingArrow;
    public bool isAiming;
    public bool isTwoHandingWeapon;
    public bool isPerformingFullyChargedAttack;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
    public bool isSprinting;
   
    [Header("Spells")]
    public bool isFiringSpell;

    //Damage will be inflicted during an animation event
    // Used in backstab or riposte animations
    public int pendingCriticalDamage;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
    }

    protected virtual void FixedUpdate()
    {
        characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
    }

    public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
    {
        if (usingRightHand)
        {
            isUsingRightHand = true;
            isUsingLeftHand = false;   
        }
        else
        {
            isUsingLeftHand = true;
            isUsingRightHand = false;
        }
    }
}
