using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PyromancyCaster,
    FaithCaster,
    SpellCaster,
    Unarmed,
    StraightSword,
    SmallShield,
    shield,
    Bow
}

public enum AmmoType
{
    Arrow,
    Bolt
}

public enum AttackType
{
    light,
    heavy
    //lightattack01
    //lightattack02
    //heavyattack01
    //heavyattack02
}

public enum AICombatStyle
{
    swordAndShield,
    archer
}

public enum AIAttackActionType
{
    meleeAttackAction,
    magicAttackAction,
    rangedAttackAction
}

public enum DamageType
{
    Physical,
    Fire
}

public enum BuffClass
{
    Physical,
    Fire
}

public enum EffectParticleType
{
    poison
}

public class Enums : MonoBehaviour
{
   
}
