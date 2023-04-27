using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadEquipmentSlotUI : MonoBehaviour
{
    UIManager uiManager;
    public Image icon;
    HelmetEquipment helmetItem;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(HelmetEquipment helmetEquipment)
    {
        if (helmetEquipment != null)
        {
            helmetItem = helmetEquipment;
            icon.sprite = helmetItem.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }
        else
        {
            ClearItem();
        }
    }


    public void ClearItem()
    {
        helmetItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SelectThisSlot()
    {
        uiManager.headEquipmentSlotSelected = true;
    }
}
