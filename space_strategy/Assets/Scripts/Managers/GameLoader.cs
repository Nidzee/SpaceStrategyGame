using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start game with fresh map
    public void StartParticularLevel(int level)
    {
        // Load scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // Init all START DATA
        StartCoroutine(InitAllDataInNewScene(level));
    }

    IEnumerator InitAllDataInNewScene(int mapNumber)
    {
        yield return null;

        // Use mapNumber to build particular map
        // Bool variable means: "Is it loaded from file?"
        GameHendler.Instance.StartGame(mapNumber, false);

        Time.timeScale = 1f;
    }


    // Load particular save
    public void LoadParticularSaveWithIndexFromFile(int indexOfSavedGameInFile)
    {
        // Load scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // Init all START DATA
        StartCoroutine(InitAllDataInNewScene_another(indexOfSavedGameInFile));
    }

    IEnumerator InitAllDataInNewScene_another(int indexOfSavedGameInFile)
    {
        yield return null;

        // Get data into "GAME HENDLER" variables from file
        GlobalSave.Instance.InitDataFromFileWithIndex(indexOfSavedGameInFile);
        
        // Use this data and bool to build scene
        // Bool variable means: "Is it loaded from file?"
        GameHendler.Instance.StartGame(GameHendler.Instance.particularLevelNumber, true);

        Time.timeScale = 1f;
    }
}