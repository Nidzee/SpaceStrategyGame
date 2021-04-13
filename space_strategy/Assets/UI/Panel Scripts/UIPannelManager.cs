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

        ReloadLoadGameScrollItems();
    }

    private void ReloadLoadGameScrollItems()
    {
        foreach (var i in saveGameScrollItems)
        {
            Destroy(i);
        }

        saveGameScrollItems.Clear();

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
                prefab.GetComponent<LoadGameItem>().loadGameText.text = GlobalSave.Instance.savingData[i].slotDescription;
                prefab.GetComponent<LoadGameItem>().levelName.text = "LVL: " + GlobalSave.Instance.savingData[i].levelNumber;

                prefab.GetComponent<Button>().onClick.AddListener(delegate{ReSave(prefab.GetComponent<LoadGameItem>().loadGameID);});


                saveGameScrollItems.Add(prefab);
            }
        }
    }

    public void CreateNewSave()
    {
        Debug.Log("Create new save!");

        GlobalSave.Instance.SaveCurrentInfoAbaoutEveruthingToList();

        ReloadLoadGameScrollItems();
    }

    public void ReSave(int indexOfSaveSlot)
    {
        Debug.Log("NO FUNCTIONAL YET!");
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