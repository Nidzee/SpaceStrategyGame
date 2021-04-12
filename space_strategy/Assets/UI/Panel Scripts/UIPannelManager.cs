using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPannelManager : MonoBehaviour
{
    public static UIPannelManager Instance {get;private set;}

    [SerializeField] private List<GameObject> pannels;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitAllPanels();

        ResetPanels("GameView");
    }

    private void InitAllPanels()
    {
        for (int i = 0; i < pannels.Count; i++)
        {
            pannels[i].SetActive(true);
        }

        ShtabStaticData.baseMenuReference = GameObject.Find("BaseMenu").GetComponent<BaseMenu>();

        AntenneStaticData.antenneMenuReference = GameObject.Find("AntenneMenu").GetComponent<AntenneMenu>();

        PowerPlantStaticData.powerPlantMenuReference = GameObject.Find("PowerPlantMenu").GetComponent<PowerPlantMenu>();

        MineShaftStaticData.shaftMenuReference = GameObject.Find("ShaftMenu").GetComponent<ShaftMenu>();

        GarageStaticData.garageMenuReference = GameObject.Find("GarageMenu").GetComponent<GarageMenu>();


        TurretStaticData.turretMenuReference = GameObject.Find("TurretMenu").GetComponent<TurretMenu>();

        ShiledGeneratorStaticData.shieldGeneratorMenuReference = GameObject.Find("ShieldGeneratorMenu").GetComponent<ShiledGeneratorMenu>();
    }


    // Resets all panels to notactive except needed one
    public void ResetPanels(string menuName)
    {
        for (int i = 0; i < pannels.Count; i++)
        {
            if (pannels[i].name == menuName)
            {
                pannels[i].SetActive(true);
            }
            else
            {
                pannels[i].SetActive(false);
            }
        }
    }



    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }

    
    private List<GameObject> saveGameScrollItems = new List<GameObject>();
    public GameObject saveGameScrollItemPrefab;
    public GameObject emptySlotForSavingPrefab;
    public GameObject saveGameConten;

    public void SaveGameMenu()
    {
        ResetPanels("SaveGameMenu");

        foreach (var i in saveGameScrollItems)
        {
            Destroy(i);
        }

        saveGameScrollItems.Clear();

        ReloadLoadGameScrollItems();
    }

    private void ReloadLoadGameScrollItems()
    {
        // Create empty slot
        GameObject newSaveItem = Instantiate(emptySlotForSavingPrefab);
        newSaveItem.gameObject.transform.SetParent(saveGameConten.transform, false);
        newSaveItem.GetComponent<Button>().onClick.AddListener(CreateNewSave);
        saveGameScrollItems.Add(newSaveItem);


        if (GlobalSave.Instance.savingData != null)
        {
            for (int i = 0; i < GlobalSave.Instance.savingData.Count; i++)
            {
                GameObject prefab = Instantiate(saveGameScrollItemPrefab);
                prefab.gameObject.transform.SetParent(saveGameConten.transform, false);


                prefab.GetComponent<LoadGameItem>().loadGameID = i;
                prefab.GetComponent<LoadGameItem>().loadGameText.text = "This is: " + i + " load";
                prefab.GetComponent<LoadGameItem>().timeText.text = i + ":" + i;

                prefab.GetComponent<Button>().onClick.AddListener(delegate{ReSave(prefab.GetComponent<LoadGameItem>().loadGameID);});


                saveGameScrollItems.Add(prefab);
            }
        }


        // // Load all saves
        // for (int i = 0; i < 3; i++)
        // {
        //     GameObject prefab = Instantiate(saveGameScrollItemPrefab);
        //     prefab.gameObject.transform.SetParent(saveGameConten.transform, false);


        //     prefab.GetComponent<LoadGameItem>().loadGameID = i;
        //     prefab.GetComponent<LoadGameItem>().loadGameText.text = "This is: " + i + " load";
        //     prefab.GetComponent<LoadGameItem>().timeText.text = i + ":" + i;

        //     prefab.GetComponent<Button>().onClick.AddListener(delegate{ReSave(prefab.GetComponent<LoadGameItem>().loadGameID);});


        //     saveGameScrollItems.Add(prefab);
        // }
    }

    public void CreateNewSave()
    {
        Debug.Log("Create new save!");

        GameHendler.Instance.SAVE_TEMP();
    }
    public void ReSave(int indexOfSaveSlot)
    {
        Debug.Log("Resave save slot: " + indexOfSaveSlot);
    }



    public void BackToPauseMenuFromSaveGameMenu()
    {
        ResetPanels("PauseMenu");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        ResetPanels("GameView");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        ResetPanels("PauseMenu");
    }







    public void Victory()
    {
        Time.timeScale = 0f;
        ResetPanels("VictoryMenu");
    }

    public void Loose()
    {
        Time.timeScale = 0f;
        ResetPanels("LooseMenu");
    }

    public void StatisticVictory()
    {
        ResetPanels("StatisticMenuVictory");
    }
    public void StatisticLoose()
    {
        ResetPanels("StatisticMenuLoose");
    }
}