using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemStatsWindowUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public Image itemIconImage;

    [Header("Equipment Stats Windows")]
    public GameObject weaponStats;
    public GameObject armorStats;

    [Header("Weapon Stats")]
    public TextMeshProUGUI physicalDamageText;
    public TextMeshProUGUI magicDamageText;
    public TextMeshProUGUI physicalAbsorptionText;
    public TextMeshProUGUI magicAbsorptionText;

    [Header("Armor Stats")]
    public TextMeshProUGUI armorPhysicalAbsorptionText;
    public TextMeshProUGUI armorMagicAbsorptionText;
    public TextMeshProUGUI armorPoisonResitanceText;

    public void UpdateWeaponItemStats(WeaponItem weapon)
    {
        CloseAllStatWindows();

        if (weapon != null)
        {
            if (weapon.itemName != null)
            {
                itemNameText.text = weapon.itemName;
            }
            else
            {
                itemNameText.text = "";
            }

            if (weapon.itemIcon != null)
            {
                itemIconImage.gameObject.SetActive(true);
                itemIconImage.enabled = true;
                itemIconImage.sprite = weapon.itemIcon;
            }
            else
            {
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
            }

            physicalDamageText.text = weapon.physicalDamage.ToString();
            physicalAbsorptionText.text = weapon.physicalDamageAbsorption.ToString();
            //Magic Damage
            //Magic Absorption

            weaponStats.SetActive(true);
        }
        else
        {
            itemNameText.text = "";
            itemIconImage.gameObject.SetActive(false);
            itemIconImage.sprite = null;
            weaponStats.SetActive(false);
        }
    }

    public void UpdateArmorItemStats(EquipmentItem armor)
    {
        CloseAllStatWindows();

        if (armor != null)
        {
            if (armor.itemName != null)
            {
                itemNameText.text = armor.itemName;
            }
            else
            {
                itemNameText.text = "";
            }

            if (armor.itemIcon != null)
            {
                itemIconImage.gameObject.SetActive(true);
                itemIconImage.enabled = true;
                itemIconImage.sprite = armor.itemIcon;
            }
            else
            {
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
            }

            armorPhysicalAbsorptionText.text = armor.physicalDefense.ToString();
            armorMagicAbsorptionText.text = armor.magicDefense.ToString();
            armorPoisonResitanceText.text = armor.poisonResistance.ToString();

            armorStats.SetActive(true);
        }
        else
        {
            itemNameText.text = "";
            itemIconImage.gameObject.SetActive(false);
            itemIconImage.sprite = null;
            armorStats.SetActive(false);
        }
    }

    private void CloseAllStatWindows()
    {
        weaponStats.SetActive(false);
        armorStats.SetActive(false);
    }

    //Update consumable item stats

    //Update ring item stats
}
