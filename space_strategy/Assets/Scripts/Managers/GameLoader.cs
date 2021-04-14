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










    public void StartParticularLevel(int level)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        StartCoroutine(InitAllDataInNewScene(level));
    }

    IEnumerator InitAllDataInNewScene(int index)
    {
        yield return null;

        GameHendler.Instance.StartGame(index, false);

        Time.timeScale = 1f;

        AstarPath.active.Scan();
    }




    


    public void LoadParticularSaveWithIndexFromFile(int index)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        StartCoroutine(InitAllDataInNewScene_another(index));
    }

    IEnumerator InitAllDataInNewScene_another(int index)
    {
        yield return null;

        GlobalSave.Instance.InitDataFromFileWithIndex(index);

        GameHendler.Instance.StartGame(GameHendler.Instance.particularLevelNumber, true);

        Time.timeScale = 1f;

        AstarPath.active.Scan();
    }
}
