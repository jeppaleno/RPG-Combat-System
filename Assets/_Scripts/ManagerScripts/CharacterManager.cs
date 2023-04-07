using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    CharacterAnimatorManager characterAnimatorManager;
    CharacterWeaponSlotManager characterWeaponSlotManager;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Combat Colliders")]
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider riposteCollider;

    [Header("Interaction")]
    public bool isInteracting;
    public bool isUsingRootMotion;

    [Header("Combat Flags")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool canDoCombo;
    public bool isParrying;
    public bool isBlocking;
    public bool isInvulnerable;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isAiming;
    public bool isTwoHandingWeapon;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
   
    [Header("Spells")]
    public bool isFiringSpell;

    //Damage will be inflicted during an animation event
    // Used in backstab or riposte animations
    public int pendingCriticalDamage;

    protected virtual void Awake()
    {
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }

    protected virtual void FixedUpdate()
    {
        characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
    }
}
