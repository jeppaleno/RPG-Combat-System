using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animator Replacer")]
    public AnimatorOverrideController weaponController;
    public string offHandIdleAnimation = "Left_Arm_Idle_01";

    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int criticalDamageMultiplier = 4;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Absorption")]
    public float physicalDamageAbsorption;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Item Actions")]
    public ItemAction oh_hold_RB_Action;
    public ItemAction oh_tap_RB_Action;
    public ItemAction oh_tap_LB_Action;
    public ItemAction oh_hold_LB_Action;
    public ItemAction oh_tap_RT_Action;
    public ItemAction oh_hold_RT_Action;
    public ItemAction oh_tap_LT_Action;
    public ItemAction oh_hold_LT_Action;

    [Header("TWO Handed Item Actions")]
    public ItemAction th_hold_RB_Action;
    public ItemAction th_tap_RB_Action;
    public ItemAction th_tap_LB_Action;
    public ItemAction th_hold_LB_Action;
    public ItemAction th_tap_RT_Action;
    public ItemAction th_hold_RT_Action;
    public ItemAction th_tap_LT_Action;
    public ItemAction th_hold_LT_Action;
}
