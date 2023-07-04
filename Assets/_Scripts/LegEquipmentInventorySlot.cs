using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LegEquipmentInventorySlot : MonoBehaviour
{
    UIManager uiManager;

    public Image icon;
    LegEquipment item;

    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    public void AddItem(LegEquipment newItem)
    {
        // Change icon of gameobject
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void EquipThisItem()
    {
        if (uiManager.legEquipmentSlotSelected)
        {
            //add the current equipped helmet (if anu) to our helmet inventory
            if (uiManager.player.playerInventoryManager.currentLegEquipment != null)
            {
                uiManager.player.playerInventoryManager.legEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentLegEquipment);
            }
            //Remove the current equipped helmet and replace it with our new helmet
            uiManager.player.playerInventoryManager.currentLegEquipment = item;
            //Remove our newly equipped helmet from our inventory
            uiManager.player.playerInventoryManager.legEquipmentInventory.Remove(item);
            //Load the new gear
            uiManager.player.playerEquipmentManager.EquipAllEquipmentModelsOnStart();
        }
        else
        {
            return;
        }

        //Update the gear to reflect on the ui/eq screen
        uiManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uiManager.player.playerInventoryManager);
        uiManager.ResetAllSelectedSlots();
    }
}
