using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
   
    PlayerWeaponSlotManager playerWeaponSlotManager;

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

    public List<WeaponItem> weaponInventory;

    private void Awake()
    {
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();   
    }

    private void Start()
    {
        rightWeapon = weaponsInRightHandSlots[0];
        leftWeapon = weaponsInLeftHandSlots[0];
        playerWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        playerWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void changeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length -1)
        {
            currentRightWeaponIndex = -1;
            rightWeapon = playerWeaponSlotManager.unarmedWeapon;
            playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, false);
        }
    }

    public void changeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
        }
        else
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = playerWeaponSlotManager.unarmedWeapon;
            playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, true);
        }
    }


}
