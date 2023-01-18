using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassGear 
{
    [Header("Class Name")]
    public string className;

    [Header("Weapons")]
    public WeaponItem primaryWeapon;
    public WeaponItem offHandWeapon;
    //public WeaponItem secondaryWeapon;

    //ARMOR GEAR FOR FUTURE
}
