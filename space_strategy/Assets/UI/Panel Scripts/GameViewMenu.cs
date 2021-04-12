using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameViewMenu : MonoBehaviour
{
    public static GameViewMenu Instance {get; private set;}

    public Text waveCounter;

    public void InitWaveCounter()
    {
        waveCounter.text = "Wave :" + ResourceManager.currentWave + "/" + ResourceManager.winWaveCounter;
    }
    
    ///////////////// Resources ///////////////////////
    [SerializeField] private Text crystalCounter;
    [SerializeField] private Text ironCounter;
    [SerializeField] private Text gelCounter;
    ////////////////////////////////////////////////////


    /////////////////// Game View Menu ////////////////////
    public Slider wholeElectricitySlider;
    public Slider usingElectricitySlider;
    public GameViewMenu gameViewMenuReference;
    ///////////////////////////////////////////////////////


    ////////// Unit Managment Menu ////////////////////////
    private bool isUnitManageMenuOpened = false;
    private bool isMenuAllResourcesTabOpened = false;
    private bool isMenuCrystalTabOpened = false;
    private bool isMenuIronTabOpened = false;
    private bool isMenuGelTabOpened = false;
    [SerializeField] public UnitManageMenu unitManageMenuReference;
    [SerializeField] public Button unitManageMenuButton;
    ////////////////////////////////////////////////////////


    //////////// Buildings Managment Menu///////////////////
    private bool isBuildingsManageMenuOpened = false;    
    private bool isIndustrialBuildingsMenuOpened = false;
    private bool isMilitaryBuildingsMenuOpened = false;
    [SerializeField] public BuildingsManageMenu buildingsManageMenuReference;
    [SerializeField] public Button buildingsManageMenuButton;
    //////////////////////////////////////////////////////// 


    //////////// Buildings Creation Menu///////////////////
    [SerializeField] private BuildingCreationMenu buildingCreationMenuReference;
    [SerializeField] private Button buildingCreationMenuButton;
    //////////////////////////////////////////////////////// 



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
            unitManageMenuReference.ReloadMainUnitCount();
        }
    }









    #region Unit manage menu, building creation menu, buildings manage menu buttons

    private bool isButtonsInit = false;

    public void BuildingCreationMenu()
    {
        if (!isButtonsInit)
        {
            isButtonsInit = true;
            buildingCreationMenuReference.InitBuildingsCosts();
        }

        UIPannelManager.Instance.ResetPanels("BuildingCreationMenu");
    }

    public void UnitMenu()
    {
        TurnOnUnitManageMenu();
        UIPannelManager.Instance.ResetPanels("UnitManageMenu");
        unitManageMenuReference.ReloadPanel();
    }

    public void BuildingsManagmentMenu()
    {
        TurnOnBuildingsManageMenu();
        UIPannelManager.Instance.ResetPanels("BuildingsManageMenu");
        buildingsManageMenuReference.ReloadPanel();
    }

    #endregion


    #region Reloading Unit Manage Menu 

        public void ReloadUnitManageMenuInfoAfterGarageDestroying(List<MineShaft> shaftsToReloadSliders)
        {
            if (isUnitManageMenuOpened)
            {
                // Always need to reload because some units may be working on shafts
                unitManageMenuReference.ReloadMainUnitCount();
    
                if (isMenuAllResourcesTabOpened)
                {
                    unitManageMenuReference.ReloadCrystalSlider();   
                    unitManageMenuReference.ReloadGelSlider();
                    unitManageMenuReference.ReloadIronSlider();
                }

                else
                {
                    if (shaftsToReloadSliders.Count != 0)
                    {
                        for (int i = 0; i < shaftsToReloadSliders.Count; i ++)
                        {
                            unitManageMenuReference.FindSLiderAndReload(shaftsToReloadSliders[i]);
                        }
                    }
                }
            }
        }

        public void ReloadUnitManageMenuInfoAfterShaftDestroying(MineShaft mineShaft)
        {
            if (isUnitManageMenuOpened) // Reload everything in here
            {
                // If all Sliders menu was opened - reload - because total shaft capacity will decrease
                if (isMenuAllResourcesTabOpened)
                {
                    unitManageMenuReference.ReloadCrystalSlider();  
                }
                unitManageMenuReference.ReloadMainUnitCount();
            }
        }

        public void ReloadUnitManageMenuInfoAfterShaftExpand(MineShaft mineShaft)
        {
            if (isUnitManageMenuOpened)
            {
                if (isMenuAllResourcesTabOpened)
                {
                    switch (mineShaft.type)
                    {
                        case 1:
                        unitManageMenuReference.ReloadCrystalSlider();   
                        break;

                        case 2:
                        unitManageMenuReference.ReloadIronSlider();
                        break;

                        case 3:
                        unitManageMenuReference.ReloadGelSlider();
                        break;
                    }
                }

                else
                {
                    unitManageMenuReference.FindSLiderAndReload(mineShaft);
                }
            }
        }

        public void ReloadUnitManageMenuAfterUnitDeath(MineShaft shaft)
        {
            if (isUnitManageMenuOpened)
            {
                // Because 1 unit died
                unitManageMenuReference.ReloadMainUnitCount();

                // If he was working - reload slider with dead unit
                if (shaft)
                {
                    if (isMenuAllResourcesTabOpened)
                    {
                        unitManageMenuReference.ReloadCrystalSlider();   
                        unitManageMenuReference.ReloadGelSlider();
                        unitManageMenuReference.ReloadIronSlider();
                    }
                    else
                    {
                        unitManageMenuReference.FindSLiderAndReload(shaft);
                    }
                }
            }
        } 

    #endregion


















    public void IncreaseElectricityCountSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityCount <= wholeElectricitySlider.maxValue)
        {
            wholeElectricitySlider.value = electricityCount;
        }

        if ((electricityCount == 80 || electricityNeedCount == 80) || (electricityCount == 120 || electricityNeedCount == 120))
        {
            wholeElectricitySlider.maxValue += 50;
            usingElectricitySlider.maxValue += 50;
        }
    }

    public void DecreaseElectricityCountSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityCount <= wholeElectricitySlider.maxValue)
        {
            wholeElectricitySlider.value = electricityCount;
        }
    }


    public void IncreaseElectricityNeedSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityNeedCount <= usingElectricitySlider.maxValue)
        {
            usingElectricitySlider.value = electricityNeedCount;
        }

        if ((electricityCount == 80 || electricityNeedCount == 80) || (electricityCount == 120 || electricityNeedCount == 120))
        {
            wholeElectricitySlider.maxValue += 50;
            usingElectricitySlider.maxValue += 50;
        }
    }

    public void DecreaseElectricityNeedSlider(int electricityCount, int electricityNeedCount)
    {
        if (electricityNeedCount <= usingElectricitySlider.maxValue)
        {
            usingElectricitySlider.value = electricityNeedCount;
        }
    }


    #region Power Level Manipulation

        public void TurnOffUnitManageMenuButtonAndBuildingsManageMenuButton()
        {
            if (isUnitManageMenuOpened)
            {
                unitManageMenuReference.ExitMenu();
            }
            if (isBuildingsManageMenuOpened)
            {
                buildingsManageMenuReference.ExitMenu();
            }

            unitManageMenuButton.interactable = false;
            buildingsManageMenuButton.interactable = false;
        }

        public void TurnOnUnitManageMenuButtonAndBuildingsManageMenuButton()
        {
            unitManageMenuButton.interactable = true;
            buildingsManageMenuButton.interactable = true;
        }

    #endregion

    #region Bool variables mainaining

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

        InitWaveCounter();
    }



















































    public void LoadElectricityFromFile(int electricity, int electricity_max, int electricityNeed, int electricityNeed_max)
    {
        wholeElectricitySlider.maxValue = electricity_max;
        wholeElectricitySlider.value = electricity;
        
        usingElectricitySlider.maxValue = electricityNeed_max;
        usingElectricitySlider.value = electricityNeed;
    }














}