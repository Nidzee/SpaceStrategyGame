using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameViewMenu : MonoBehaviour
{
    public static GameViewMenu Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


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
    [SerializeField] private UnitManageMenu unitManageMenuReference;
    [SerializeField] private Button unitManageMenuButton;
    ////////////////////////////////////////////////////////



    //////////// Buildings Managment Menu///////////////////
    private bool isBuildingsMAnageMenuOpened = false;    
    private bool isIndustrialBuildingsMenuOpened = false;
    private bool isMilitaryBuildingsMenuOpened = false;
    [SerializeField] private BuildingsManageMenu buildingsManageMenuReference;
    [SerializeField] private Button buildingsManageMenuButton;
    //////////////////////////////////////////////////////// 




    [SerializeField] private Button buildingCreationMenuButton;

    private bool isCreateBuildingButtonInteractible = false;

    [SerializeField] public Text crystalCounter;
    [SerializeField] public Text ironCounter;
    [SerializeField] public Text gelCounter;


    // Button activation managment
    private void Update()
    {
        if (GameHendler.Instance.SelectedHex != null && !isCreateBuildingButtonInteractible)
        {
            buildingCreationMenuButton.interactable = true;
            isCreateBuildingButtonInteractible = true;
        }
        else if (!GameHendler.Instance.SelectedHex && isCreateBuildingButtonInteractible)
        {
            buildingCreationMenuButton.interactable = false;
            isCreateBuildingButtonInteractible = false;
        }
    }





    // Opens Building Creation Menu
    public void BuildingCreationMenu()
    {
        UIPannelManager.Instance.ResetPanels("BuildingCreationMenu");
    }

    // Opens Units Menu - TODO
    public void UnitMenu()
    {
        UIPannelManager.Instance.ResetPanels("UnitManageMenu");
        
        unitManageMenuReference.ReloadPanel();
    }

    // Opens Buildings Menu - TODO
    public void BuildingsManagmentMenu()
    {
        UIPannelManager.Instance.ResetPanels("BuildingsManageMenu");
        
        buildingsManageMenuReference.ReloadPanel();
    }



    public void ReloadMainUnitCount()
    {
        unitManageMenuReference.ReloadMainUnitCount();
    }



    public void ReloadBaseHP_SPAfterGamage()
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadBaseHPSP();
            }
        }
    }

    public void ReloadAntenneHP_SPAfterDamage()
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadAntenneHPSP();
            }
        }
    }

    public void ReloadCrystalShaftHP_SPAfterDamage(CrystalShaft crystalShaft)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadCrystalShaftHPSP(crystalShaft);
            }
        }
    }

    public void ReloadIronShaftHP_SPAfterDamage(IronShaft ironShaft)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadIronShaftHPSP(ironShaft);
            }
        }
    }

    public void ReloadGelShaftHP_SPAfterDamage(GelShaft gelShaft)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadGelShaftHPSP(gelShaft);
            }
        }
    }

    public void ReloadGarageHP_SPAfterDamage(Garage garage)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadGarageHPSP(garage);
            }
        }
    }

    public void ReloadPowerPlantHP_SPAfterDamage(PowerPlant powerPlant)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadPowerPlantHPSP(powerPlant);
            }
        }
    }

    public void ReloadShieldGeneratorHP_SPAfterDamage(ShieldGenerator shieldGenerator)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (isMilitaryBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadShieldGeneratorHPSP(shieldGenerator);
            }
        }
    }



    public void ReloadBuildingsManageMenuInfo_Garage(Garage garage)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.RemoveGarageFromBuildingsMenu(garage.name);
            }
        }
    }

    public void ReloadUnitManageMenuInfo_Garage(List<MineShaft> shaftsToReloadSliders)
    {
        if (isUnitManageMenuOpened)
        {
            // Always need to reload because some units may be working on shafts
            unitManageMenuReference.ReloadMainUnitCount();
 
            if (isMenuAllResourcesTabOpened)
            {
                ReloadMenuSlider();
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

    private void ReloadMenuSlider()
    {
        unitManageMenuReference.ReloadCrystalSlider();   
        unitManageMenuReference.ReloadGelSlider();
        unitManageMenuReference.ReloadIronSlider();
    }



    public void ReloadBuildingsManageMenuInfo_Antenne()
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isIndustrialBuildingsMenuOpened)
            {
                buildingsManageMenuReference.RemoveAntenneFromBuildingsMenu();
            }
        }
    }



    public void ReloadUnitManageMenuInfo_CrystalShaft(CrystalShaft crystalShaft)
    {
        if (isUnitManageMenuOpened) // Reload everything in here
        {
            // If all Sliders menu was opened - reload - because total shaft capacity will decrease
            if (isMenuAllResourcesTabOpened)
            {
                unitManageMenuReference.ReloadCrystalSlider();  
            }

            // If crystal Tab was opened - reload whole tab - to delete dead shaft
            if (isMenuCrystalTabOpened)
            {
                // GameHendler.Instance.unitManageMenuReference.ReloadCrystalTab();
                unitManageMenuReference.RemoveCrystalScrollItem(crystalShaft);
            }

            // Reload Units becasu units without workplace - became avaliable
            unitManageMenuReference.ReloadMainUnitCount();
        }
    }

    public void ReloadBuildingsManageMenuInfo_CrystalShaft(CrystalShaft crystalShaft)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isIndustrialBuildingsMenuOpened)
            {
                // Drop some code here
                buildingsManageMenuReference.RemoveCrystalShaftFromBuildingsMenu(crystalShaft.name);
            }
        }
    }


    public void ReloadUnitManageMenuInfo_GelShaft(GelShaft gelShaft)
    {
        if (isUnitManageMenuOpened) // Reload everything in here
        {
            // If all Sliders menu was opened - reload - because total shaft capacity will decrease
            if (isMenuAllResourcesTabOpened)
            {
                unitManageMenuReference.ReloadGelSlider();
            }

            // If crystal Tab was opened - reload whole tab - to delete dead shaft
            if (isMenuGelTabOpened)
            {
                // GameHendler.Instance.unitManageMenuReference.ReloadGelTab();
                unitManageMenuReference.RemoveGelScrollItem(gelShaft);
            }

            // Reload Units becasu units without workplace - became avaliable
            unitManageMenuReference.ReloadMainUnitCount();
        }
    }

    public void ReloadBuildingsManageMenuInfo_GelShaft(GelShaft gelShaft)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isIndustrialBuildingsMenuOpened)
            {
                // Drop some code here
                buildingsManageMenuReference.RemoveGelShaftFromBuildingsMenu(gelShaft.name);
            }
        }
    }


    public void ReloadUnitManageMenuInfo_IronShaft(IronShaft ironShaft)
    {
        if (isUnitManageMenuOpened) // Reload everything in here
        {
            // If all Sliders menu was opened - reload - because total shaft capacity will decrease
            if (isMenuAllResourcesTabOpened)
            {
                unitManageMenuReference.ReloadIronSlider();
            }

            // If crystal Tab was opened - reload whole tab - to delete dead shaft
            if (isMenuIronTabOpened)
            {
                // GameHendler.Instance.unitManageMenuReference.ReloadIronTab();
                unitManageMenuReference.RemoveIronScrollItem(ironShaft);
            }

            // Reload Units becasu units without workplace - became avaliable
            unitManageMenuReference.ReloadMainUnitCount();
        }
    }

    public void ReloadBuildingsManageMenuInfo_IronShaft(IronShaft ironShaft)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isIndustrialBuildingsMenuOpened)
            {
                // Drop some code here
               buildingsManageMenuReference.RemoveIronShaftFromBuildingsMenu(ironShaft.name);
            }
        }
    }


    public void ReloadMineShaftSLiders(MineShaft mineShaft)
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

            unitManageMenuReference.FindSLiderAndReload(mineShaft);
        }
    }




    public void ReloadBuildingsManageMenuInfo_PowerPlant(PowerPlant powerPlant)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isIndustrialBuildingsMenuOpened)
            {
                // Drop some code here
                buildingsManageMenuReference.RemovePowerPlantFromBuildingsMenu(powerPlant.name);
            }
        }
    }


    public void ReloadBuildingsManageMenuInfo_ShieldGenerator(ShieldGenerator shieldGenerator)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isMilitaryBuildingsMenuOpened)
            {
                // Drop some code here
                buildingsManageMenuReference.RemoveShieldGenerator(shieldGenerator.name);
            }
        }
    }




    public void ReloadUnitManageMenu(MineShaft shaft)
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




    public void TurnOnBuildingsManageMenu()
    {
        isBuildingsMAnageMenuOpened = true;
    }

    public void TurnOffBuildingsManageMenu()
    {
        isBuildingsMAnageMenuOpened = false;
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





    public void TurnOffUnitManageMenu()
    {
        isMenuAllResourcesTabOpened = false;
        isMenuCrystalTabOpened = false;
        isMenuGelTabOpened = false;
        isMenuIronTabOpened = false;

        isUnitManageMenuOpened = false;
    }

    public void TurnOnUnitManageMenu()
    {
        isUnitManageMenuOpened = true;

        isMenuAllResourcesTabOpened = false;
        isMenuCrystalTabOpened = false;
        isMenuIronTabOpened = false;
        isMenuGelTabOpened = false;
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


    public bool CheckForUnitManageMenuOpened()
    {
        return isUnitManageMenuOpened;
    }





    public void TurnOffUnitManageMenuButtonAndBuildingsManageMenuButton()
    {
        if (isUnitManageMenuOpened)
        {
            unitManageMenuReference.ExitMenu();
        }
        if (isBuildingsMAnageMenuOpened)
        {
            buildingsManageMenuReference.ExitMenu();
        }
        // Make buttons inactive
        unitManageMenuButton.interactable = false;
        buildingsManageMenuButton.interactable = false;
    }

    public void TurnOnUnitManageMenuButtonAndBuildingsManageMenuButton()
    {
        unitManageMenuButton.interactable = true;
        buildingsManageMenuButton.interactable = true;
    }





    public void ReloadMisileTurretHPSP_Misile(TurretMisile turretMisile)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isMilitaryBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadMisileTurretHPSP(turretMisile);
            }
        }
    }

    public void ReloadMisileTurretHPSP_Laser(TurretLaser turretLaser)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isMilitaryBuildingsMenuOpened)
            {
                buildingsManageMenuReference.ReloadLaserTurretHPSP(turretLaser);
            }
        }
    }

    public void ReloadBuildingsManageMenuInfo_TurretLaser(TurretLaser turretLaser)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isMilitaryBuildingsMenuOpened)
            {
                // Drop some code here
                buildingsManageMenuReference.RemoveLaserTurret(turretLaser.name);
            }
        }
    }

    public void ReloadBuildingsManageMenuInfo_TurretMisile(TurretMisile turretMisile)
    {
        if (isBuildingsMAnageMenuOpened)
        {
            if (isMilitaryBuildingsMenuOpened)
            {
                // Drop some code here
                buildingsManageMenuReference.RemoveMisileTurret(turretMisile.name);
            }
        }
    }

}
