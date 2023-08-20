using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject elfPrefab;
    public GameObject humanPrefab;
    public GameObject dwarfPrefab;

    public Button elfButton;
    public Button humanButton;
    public Button dwarfButton;

    private void Start()
    {
        Debug.Log("Start method called"); // Add this line for debugging

        // Attach button click listeners
        elfButton.onClick.AddListener(SpawnElf);
        humanButton.onClick.AddListener(SpawnHuman);
        dwarfButton.onClick.AddListener(SpawnDwarf);
    }

    private void Update()
    {
        // Check for key press to spawn human
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnHuman();
        }
    }

    public void SpawnElf()
    {
        ClearCharacters();
        Instantiate(elfPrefab, transform);
    }

    public void SpawnHuman()
    {
        Debug.Log("Spawning Human");
        ClearCharacters();
        Instantiate(humanPrefab, transform);
    }

    public void SpawnDwarf()
    {
        ClearCharacters();
        Instantiate(dwarfPrefab, transform);
    }

    private void ClearCharacters()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

