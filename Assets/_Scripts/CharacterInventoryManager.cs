using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    protected CharacterWeaponSlotManager characterWeaponSlotManager;

    [Header("Quick Slot Items")]
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public ConsumableItem currentConsumable;

    [Header("Current Equipment")]
    public HelmetEquipment currentHelmetEquipment;


    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    private void Awake()
    {
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }

    private void Start()
    {
        characterWeaponSlotManager.LoadBothWeaponOnSlot();
    }
}
