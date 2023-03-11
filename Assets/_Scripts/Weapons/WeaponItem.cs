using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Damage")]
    public int baseDamage = 25;
    public int criticalDamageMultiplier = 4;

    [Header("Absorption")]
    public float physicalDamageAbsorption;

    [Header("Idle Animations")]
    public string right_hand_Idle;
    public string left_hand_Idle;
    public string th_idle;

    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack_1;
    public string OH_Light_Attack_2;
    public string OH_Heavy_Attack_1;
    public string th_light_attack_01;
    public string th_light_attack_02;

    [Header("Weapon Art")]
    public string weapon_art;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Weapon Type")]
    public bool isSpellCaster;
    public bool isFaithCaster;
    public bool isPyroCaster;
    public bool isMeleeWeapon;
    public bool isShieldWeapon;
}