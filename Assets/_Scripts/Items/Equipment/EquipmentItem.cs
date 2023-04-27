using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{
    [Header("Defense Bonus")]
    public float physicalDefense;
    public float magicDefense;
    [Space]
    public float fireDefense;
    public float lightningDefense;
    public float darknessDefense;

    [Header("Resistances")]
    public float poisonResistance;

}
