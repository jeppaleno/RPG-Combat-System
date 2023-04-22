using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    PlayerManager playerManager;
    public EquipmentWindowUI equipmentWindowUI;
    private QuickSlotsUI quickSlotsUI;

    [Header("HUD")]
    public GameObject crossHair;
    public Text soulCount;

    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject equipmentScreenWindow;
    public GameObject weaponInventoryWindow;
    public GameObject levelUpWindow;

    [Header("Equipment Window Slot Selected")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent; // Where slots instantiates on
    WeaponInventorySlot[] weaponInventorySlots;

    private void Awake()
    {
        quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
        playerManager = FindObjectOfType<PlayerManager>();
    }


    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerManager.playerInventoryManager);
        quickSlotsUI.UpdateCurrentSpellIcon(playerManager.playerInventoryManager.currentSpell);
        quickSlotsUI.UpdateCurrentConsumableIcon(playerManager.playerInventoryManager.currentConsumable);
        soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if (i < playerManager.playerInventoryManager.weaponInventory.Count)
            {
                if (weaponInventorySlots.Length < playerManager.playerInventoryManager.weaponInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                weaponInventorySlots[i].AddItem(playerManager.playerInventoryManager.weaponInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }
    }

    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
    }
}
