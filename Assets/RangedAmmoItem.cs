using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ammo")]
public class RangedAmmoItem : Item
{
    [Header("Ammo Type")]
    public AmmoType ammoType;

    [Header("Ammo Velocity")]
    public float forwardVelocity = 550;
    public float upwardVelocity = 0;
    public float ammoMass = 0;
    public bool useGravity = false;

    [Header("Ammo Capacity")]
    public int carryLimit = 99;
    public int currentAmount = 99;

    [Header("Ammo Base Damage")]
    public float physicalDamage = 50;
    //Magic Damage
    //Fire Damage
    //Dark Damage
    //Lightning Damage

    [Header("Item Models")]
    public GameObject loadedItemModel; //The model that is displayed while drawing the bow back
    public GameObject liveAmmoModel; // The live model that damage characters
    public GameObject penetradedModel; //The model that instantiates into a collider on contact
}
