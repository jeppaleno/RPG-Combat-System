using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassStats 
{
    public string playerClass;
    public int classLevel;

    [TextArea]
    public string classDescription;

    [Header("Class Stats")]
    public int strenghLevel;
    public int dexterityLevel;
    //Vigor Level
    //stamina level
}
