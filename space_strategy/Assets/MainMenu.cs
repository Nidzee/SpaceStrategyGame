using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject[] menus;

    public Text levelMapName;
    public Text levelDescription;

    public Image level1Map;
    public Image level2Map;
    public Image level3Map;
    public Image level4Map;
    public Image level5Map;

    public string Level1MapName = "Planet 1";
    public string Level2MapName = "Planet 2";
    public string Level3MapName = "Planet 3";
    public string Level4MapName = "Planet 4";
    public string Level5MapName = "Planet 5";

    public string Level1MapDescription = "Planet 1 Desc";
    public string Level2MapDescription = "Planet 2 Desc";
    public string Level3MapDescription = "Planet 3 Desc";
    public string Level4MapDescription = "Planet 4 Desc";
    public string Level5MapDescription = "Planet 5 Desc";

    public int level = 0;

    enum MenusIDs
    {
        mainMenu,
        optionsMenu,
        playMenu,
        loadMenu,
        campaginMenu,
        particularMapMenu
    }

    private void Start()
    {
        SetPanel((int)MenusIDs.mainMenu);
    }

    public void SetPanel(int menuIndex)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menuIndex == i)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }

    public void PlayMenu()
    {
        SetPanel((int)MenusIDs.playMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        SetPanel((int)MenusIDs.optionsMenu);
    }

    public void BackToMainMenuFromOptions()
    {
        SetPanel((int)MenusIDs.mainMenu);
    }

    // Play menu
    public void BackToMainMenuFromPlayMenu()
    {
        SetPanel((int)MenusIDs.mainMenu);
    }

    public void CampaginMenu()
    {
        SetPanel((int)MenusIDs.campaginMenu);
    }

    private List<GameObject> loadGameScrollItems = new List<GameObject>();
    public GameObject loadGameScrollItemPrefab;
    public GameObject loadGameContent;

    public void LoadGameMenu()
    {
        SetPanel((int)MenusIDs.loadMenu);


        foreach (var i in loadGameScrollItems)
        {
            Destroy(i);
        }

        loadGameScrollItems.Clear();

        ReloadLoadGameScrollItems();
    }















    private void ReloadLoadGameScrollItems()
    {
        if (GlobalSave.Instance.savingData != null)
        {
            for (int i = 0; i < GlobalSave.Instance.savingData.Count; i++)
            {
                GameObject prefab = Instantiate(loadGameScrollItemPrefab);
                prefab.gameObject.transform.SetParent(loadGameContent.transform, false);


                prefab.GetComponent<LoadGameItem>().loadGameID = i;
                prefab.GetComponent<LoadGameItem>().loadGameText.text = GlobalSave.Instance.savingData[i].slotDescription;
                prefab.GetComponent<LoadGameItem>().levelName.text = "LVL: " + GlobalSave.Instance.savingData[i].levelNumber;

                prefab.GetComponent<Button>().onClick.AddListener(delegate{LoadGame(prefab.GetComponent<LoadGameItem>().loadGameID);});


                loadGameScrollItems.Add(prefab);
            }
        }
    }

    public void LoadGame(int indexOfLoadSlot)
    {
        GlobalSave.Instance.LoadParticularSaveWithIndexFromFile(indexOfLoadSlot);
    }



















    


    // Load game Menu
    public void BackToPlayMenuFormLoadGameMenu()
    {
        SetPanel((int)MenusIDs.playMenu);
    }

    // Campagin menu
    public void BackToPlayMenuFromCampaginMenu()
    {
        SetPanel((int)MenusIDs.playMenu);
    }

    public void LoadParticularLevel(int levelNumber)
    {
        SetPanel((int)MenusIDs.particularMapMenu);

        switch (levelNumber)
        {
            case 1:
            levelMapName.text = Level1MapName;
            levelDescription.text = Level1MapDescription;
            level = 1;
            break;

            case 2:
            levelMapName.text = Level2MapName;
            levelDescription.text = Level2MapDescription;
            level = 2;
            break;

            case 3:
            levelMapName.text = Level3MapName;
            levelDescription.text = Level3MapDescription;
            level = 3;
            break;

            case 4:
            levelMapName.text = Level4MapName;
            levelDescription.text = Level4MapDescription;
            level = 4;
            break;

            case 5:
            levelMapName.text = Level5MapName;
            levelDescription.text = Level5MapDescription;
            level = 5;
            break;
        }
    }













    // Particular level menu
    public void BackToCampaginMenuFromParticularLevel()
    {
        SetPanel((int)MenusIDs.campaginMenu);
    }

    public void StartParticularLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
    }
}
