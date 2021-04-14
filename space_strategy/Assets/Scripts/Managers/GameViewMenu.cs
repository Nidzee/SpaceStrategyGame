using UnityEngine;
using UnityEngine.UI;

public class GameViewMenu : MonoBehaviour
{
    public static GameViewMenu Instance {get; private set;}
    private void Awake()
    {
        Debug.Log("GAME VIEW MENU START WORKING!");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Text waveInfo;                     // Init in inspector
    [SerializeField] private Text crystalCounter;               // Init in inspector
    [SerializeField] private Text ironCounter;                  // Init in inspector
    [SerializeField] private Text gelCounter;                   // Init in inspector
    public Slider electricityCountSlider;                       // Init in inspector
    public Slider electricityNeedCountSlider;                       // Init in inspector
    [SerializeField] private Button buildingsManageMenuButton;  // Init in inspector
    [SerializeField] private Button buildingCreationMenuButton; // Init in inspector
    [SerializeField] private Button unitManageMenuButton;       // Init in inspector

    public bool isMenuAllResourcesTabOpened      = false;
    private bool isUnitManageMenuOpened          = false;
    private bool isMenuCrystalTabOpened          = false;
    private bool isMenuIronTabOpened             = false;
    private bool isMenuGelTabOpened              = false;
    private bool isBuildingsManageMenuOpened     = false;    
    private bool isIndustrialBuildingsMenuOpened = false;
    private bool isMilitaryBuildingsMenuOpened   = false;






    public void UpdateWaveCounter()
    {
        waveInfo.text = "Wave :" + ResourceManager.currentWave + "/" + ResourceManager.winWaveCounter;
    }
    
    public void UpdateResourcesCount(int crystals, int iron, int gel)
    {
        crystalCounter.text = crystals.ToString();
        ironCounter.text = iron.ToString();
        gelCounter.text = gel.ToString();
    }

    public void ReloadMainUnitCount()
    {
        if (isUnitManageMenuOpened)
        { 
            UnitManageMenu.Instance.ReloadMainUnitCount();
        }
    }


    public void InitData()
    {
        waveInfo.text = "Wave :" + ResourceManager.currentWave + "/" + ResourceManager.winWaveCounter;

        crystalCounter.text = ResourceManager.Instance.resourceCrystalCount.ToString();
        ironCounter.text = ResourceManager.Instance.resourceIronCount.ToString();
        gelCounter.text = ResourceManager.Instance.resourceGelCount.ToString();

        electricityCountSlider.maxValue = ResourceManager.Instance.electricityCount_max;
        electricityCountSlider.value = ResourceManager.Instance.electricityCount;
        
        electricityNeedCountSlider.maxValue = ResourceManager.Instance.electricityNeedCount_max;
        electricityNeedCountSlider.value = ResourceManager.Instance.electricityNeedCount;
    }





























    private bool isButtonsInit = false;

    public void OpenBuildingCreationMenu()
    {
        if (!isButtonsInit)
        {
            isButtonsInit = true;
            BuildingCreationMenu.Instance.InitBuildingsCosts();
        }

        UIPannelManager.Instance.ResetPanels("BuildingCreationMenu");
    }

    public void OpenUnitManageMenu()
    {
        TurnOnUnitManageMenu();
        UIPannelManager.Instance.ResetPanels("UnitManageMenu");
        UnitManageMenu.Instance.ReloadPanel();
    }

    public void OpenBuildingsManagmentMenu()
    {
        TurnOnBuildingsManageMenu();
        UIPannelManager.Instance.ResetPanels("BuildingsManageMenu");
        BuildingsManageMenu.Instance.ReloadPanel();
    }

















    public void IncreaseElectricityCountSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityCount <= electricityCountSlider.maxValue)
        {
            electricityCountSlider.value = electricityCount;
        }

        if ((electricityCount == 80 || electricityNeedCount == 80) || (electricityCount == 120 || electricityNeedCount == 120))
        {
            electricityCountSlider.maxValue += 50;
            electricityNeedCountSlider.maxValue += 50;
        }
    }

    public void DecreaseElectricityCountSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityCount <= electricityCountSlider.maxValue)
        {
            electricityCountSlider.value = electricityCount;
        }
    }


    public void IncreaseElectricityNeedSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityNeedCount <= electricityNeedCountSlider.maxValue)
        {
            electricityNeedCountSlider.value = electricityNeedCount;
        }

        if ((electricityCount == 80 || electricityNeedCount == 80) || (electricityCount == 120 || electricityNeedCount == 120))
        {
            electricityCountSlider.maxValue += 50;
            electricityNeedCountSlider.maxValue += 50;
        }
    }

    public void DecreaseElectricityNeedSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityNeedCount <= electricityNeedCountSlider.maxValue)
        {
            electricityNeedCountSlider.value = electricityNeedCount;
        }
    }


















    #region Bool variables mainaining

        public void TurnOffUnitManageMenuButtonAndBuildingsManageMenuButton()
        {
            if (isUnitManageMenuOpened)
            {
                UnitManageMenu.Instance.ExitMenu();
            }
            if (isBuildingsManageMenuOpened)
            {
                BuildingsManageMenu.Instance.ExitMenu();
            }

            unitManageMenuButton.interactable = false;
            buildingsManageMenuButton.interactable = false;
        }

        public void TurnOnUnitManageMenuButtonAndBuildingsManageMenuButton()
        {
            unitManageMenuButton.interactable = true;
            buildingsManageMenuButton.interactable = true;
        }

        public void TurnBuildingsCreationButtonON()
        {
            buildingCreationMenuButton.interactable = true;
        }

        public void TurnBuildingsCreationButtonOFF()
        {
            buildingCreationMenuButton.interactable = false;
        }

        public void TurnOnBuildingsManageMenu()
        {
            isBuildingsManageMenuOpened = true;
        }

        public void TurnOffBuildingsManageMenu()
        {
            isBuildingsManageMenuOpened = false;
            isIndustrialBuildingsMenuOpened = false;
            isMilitaryBuildingsMenuOpened = false;
        }

        public void TurnOnIndustrialBuildingsTab()
        {
            isIndustrialBuildingsMenuOpened = true;
            isMilitaryBuildingsMenuOpened = false;
        }

        public void TurnOnMilitaryBuildingsTab()
        {
            isIndustrialBuildingsMenuOpened = false;
            isMilitaryBuildingsMenuOpened = true;
        }


        public void TurnOnUnitManageMenu()
        {
            isUnitManageMenuOpened = true;

            isMenuAllResourcesTabOpened = false;
            isMenuCrystalTabOpened = false;
            isMenuIronTabOpened = false;
            isMenuGelTabOpened = false;
        }

        public void TurnOffUnitManageMenu()
        {
            isMenuAllResourcesTabOpened = false;
            isMenuCrystalTabOpened = false;
            isMenuGelTabOpened = false;
            isMenuIronTabOpened = false;

            isUnitManageMenuOpened = false;
        }


        public void TurnOnAllResourceTab()
        {
            isMenuAllResourcesTabOpened = true;
        }

        public void TurnOnCrystalTab()
        {
            isMenuCrystalTabOpened = true;
        }

        public void TurnOnIronTab()
        {
            isMenuIronTabOpened = true;
        }

        public void TurnOnGelTab()
        {
            isMenuGelTabOpened = true;
        }

    #endregion
}