using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDataBase : MonoBehaviour
{
    public static WorldItemDataBase Instance;

    public List<WeaponItem> weaponItems = new List<WeaponItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public WeaponItem GetWeaponItemByID(int weaponID)
    {
        return weaponItems.FirstOrDefault(weapon => weapon.itemID == weaponID);
    }
}
