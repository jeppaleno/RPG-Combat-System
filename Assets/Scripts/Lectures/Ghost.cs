using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] GhostManager.GhostTypes ghostType;

    void Start()
    {
        GhostType();
    }

    void GhostType()
    {
        switch (ghostType)
        {
            case GhostManager.GhostTypes.Rover:
                print("I am a Rover");
                break;
            case GhostManager.GhostTypes.Destroyer:
                print("I am a Destroyer");
                break;
            case GhostManager.GhostTypes.Orbiter:
                print("I am a Orbiter");
                break;

        }
        //Logic
    }
}


