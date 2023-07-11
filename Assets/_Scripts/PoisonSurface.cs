using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSurface : MonoBehaviour
{
    public float poisonBuilUpAmount = 7;

    public List<CharacterStatsManager> charactersInsidePoisonSurface;

    private void OnTriggerEnter(Collider other)
    {
        CharacterStatsManager character = other.GetComponent<CharacterStatsManager>();

        if (character != null)
        {
            charactersInsidePoisonSurface.Add(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterStatsManager character = other.GetComponent<CharacterStatsManager>();

        if (character != null)
        {
            charactersInsidePoisonSurface.Remove(character);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (CharacterStatsManager character in charactersInsidePoisonSurface)
        {
            if (character.isPoisoned)
                return;

            character.poisonBuildup = character.poisonBuildup + poisonBuilUpAmount * Time.deltaTime;
        }
    }
}
