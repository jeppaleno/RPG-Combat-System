using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    public string characterName;

    public int characterLevel;

    [Header("Equipment")]
    public int currentRightHandWeaponID;
    public int currentLeftHandWeaponID;

    public int currentHeadGearItemID;
    public int currentChestGearItemID;
    public int currentLegGearItemID;
    public int currentHandGearItemID;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Items Looted From World")]
    public SerializbleDictionary<int, bool> itemsInWorld; // The Int is the world item ID, the bool is if the item has been looted

    public CharacterSaveData()
    {
        itemsInWorld = new SerializbleDictionary<int, bool>();
    }
}
