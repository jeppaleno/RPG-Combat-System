using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClassSelector : MonoBehaviour
{
    PlayerManager player;
   

    //A Set of stats for each class
    [Header("Class Starting Stats")]
    public ClassStats[] classStats;

    [Header("Class Starting Gear")]
    public ClassGear[] classGears;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
       
    }

    private void AssignClassStats(int classChosen)
    {
        
    }

    public void AssignHumanClass()
    {
        AssignClassStats(0);
        //asign gear
        Debug.Log("IM HUMAN");

    }

   
    public void AssignElfClass()
    {
        Debug.Log("IM ELF");
    }

    public void AssignDwarfClass()
    {
        Debug.Log("IM DWARF");
    }
}
