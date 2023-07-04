using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public PlayerManager player;

    [Header("Save Data Writer")]
    SaveGameDataWriter saveGameDataWriter;

    [Header("Current Character Data")]
    // CHARACTER SLOT #
    public CharacterSaveData currentCharacterSaveData;
    [SerializeField] private string fileName;

    [Header("SAVE/LOAD")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Load all possible character profiles
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }
        else if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    // NEW GAME

    // SAVE GAME
    public void SaveGame()
    {
        saveGameDataWriter = new SaveGameDataWriter();
        saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;  // Figures out best datapath on both windows and mac's etc
        saveGameDataWriter.dataSaveFileName = fileName;

        // Pass along our character data to the current save file
        player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

        // Write the current character data to a json and save it on this device
        saveGameDataWriter.WriteCharacterDataToSaveDataFile(currentCharacterSaveData);

        Debug.Log("SAVING GAME...");
        Debug.Log("FILE SAVED AS: " + fileName);
    }

    // LOAD GAME

    public void LoadGame()
    {
        // DECIDE LOAD FILED BASED ON CHARACTER SAVE SLOT

        saveGameDataWriter = new SaveGameDataWriter();
        saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveGameDataWriter.dataSaveFileName = fileName;
        currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

        StartCoroutine(LoadWorldSceneAsynchronously());
    }

    private IEnumerator LoadWorldSceneAsynchronously()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerManager>();
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(0);  // CHANGE SCENE HERE <--- NOTE

        while (!loadOperation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            //ENABLE A LOADING SCREEN AND PASS THE LOADING PROGRESS TO A SLIDER/EFFECT
            yield return null;
        }

        player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
    }

}
