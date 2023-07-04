using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    public string characterName;

    public int characterLevel;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
