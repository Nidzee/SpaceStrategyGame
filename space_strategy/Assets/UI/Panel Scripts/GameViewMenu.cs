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
            // DontDestroyOnLoad(gameObject);
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
    private bool isBuildingsManageMenuOpened = false;    
    private bool isIndustrialBuildingsMenuOpened = false;
    private bool isMilitaryBuildingsMenuOpened = false;
    [SerializeField] private BuildingsManageMenu buildingsManageMenuReference;
    [SerializeField] private Button buildingsManageMenuButton;
    //////////////////////////////////////////////////////// 




    [SerializeField] private Button buildingCreationMenuButton;

    // private bool isCreateBuildingButtonInteractible = false;

    [SerializeField] public Text crystalCounter;
    [SerializeField] public Text ironCounter;
    [SerializeField] public Text gelCounter;




    public void TurnBuildingsCreationButtonON()
    {
        buildingCreationMenuButton.interactable = true;
        // isCreateBuildingButtonInteractible = true;
    }

    public void TurnBuildingsCreationButtonOFF()
    {
        buildingCreationMenuButton.interactable = false;
        // isCreateBuildingButtonInteractible = false;
    }








    public void ReloadMainUnitCount()
    {
        unitManageMenuReference.ReloadMainUnitCount();
    }


    #region Bottom 3 buttons managing
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

    #endregion


    #region Dmagae HP/SP sliders reloading logic

        public void ReloadBaseHP_SPAfterDamage()
        {
            if (isBuildingsManageMenuOpened)
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
            if (isBuildingsManageMenuOpened)
            {
                // Drop some code here
                if (isIndustrialBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.ReloadAntenneHPSP();
                }
            }
        }

        public void ReloadShaftHP_SPAfterDamage(MineShaft mineShaft)
        {
            if (isBuildingsManageMenuOpened)
            {
                // Drop some code here
                if (isIndustrialBuildingsMenuOpened)
                {
                    switch (mineShaft.type)
                    {
                        case 1:
                        buildingsManageMenuReference.ReloadCrystalShaftHPSP((CrystalShaft)mineShaft);
                        break;
                        
                        case 2:
                        buildingsManageMenuReference.ReloadIronShaftHPSP((IronShaft)mineShaft);
                        break;
                        
                        case 3:
                        buildingsManageMenuReference.ReloadGelShaftHPSP((GelShaft)mineShaft);
                        break;
                    }
                }
            }
        }

        public void ReloadGarageHP_SPAfterDamage(Garage garage)
        {
            if (isBuildingsManageMenuOpened)
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
            if (isBuildingsManageMenuOpened)
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
            if (isBuildingsManageMenuOpened)
            {
                // Drop some code here
                if (isMilitaryBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.ReloadShieldGeneratorHPSP(shieldGenerator);
                }
            }
        }
        
        public void ReloadMisileTurretHPSP_Misile(TurretMisile turretMisile)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isMilitaryBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.ReloadMisileTurretHPSP(turretMisile);
                }
            }
        }

        public void ReloadMisileTurretHPSP_Laser(TurretLaser turretLaser)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isMilitaryBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.ReloadLaserTurretHPSP(turretLaser);
                }
            }
        }

    #endregion


    #region Reloaading Buildings Manage Menu after some building destroying

        public void ReloadBuildingsManageMenuInfo___AfterAntenneDestroying()
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isIndustrialBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.RemoveAntenneFromBuildingsMenu();
                }
            }
        }

        public void ReloadBuildingsManageMenuInfo___AfterGarageDestroying(Garage garage)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isIndustrialBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.RemoveGarageFromBuildingsMenu(garage.name);
                }
            }
        }

        public void ReloadBuildingsManageMenuInfo___AfterShaftDestroying(string mineName, int type)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isIndustrialBuildingsMenuOpened)
                {
                    switch (type)
                    {
                        case 1:
                        buildingsManageMenuReference.RemoveCrystalShaftFromBuildingsMenu(mineName);
                        break;
                        
                        case 2:
                        buildingsManageMenuReference.RemoveIronShaftFromBuildingsMenu(mineName);
                        break;
                        
                        case 3:
                        buildingsManageMenuReference.RemoveGelShaftFromBuildingsMenu(mineName);
                        break;
                    }
                }
            }
        }

        public void ReloadBuildingsManageMenuInfo___AfterPowerPlantDestroying(PowerPlant powerPlant)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isIndustrialBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.RemovePowerPlantFromBuildingsMenu(powerPlant.name);
                }
            }
        }

        public void ReloadBuildingsManageMenuInfo___AfterShieldGeneratorDestroying(ShieldGenerator shieldGenerator)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isMilitaryBuildingsMenuOpened)
                {
                    buildingsManageMenuReference.RemoveShieldGenerator(shieldGenerator.name);
                }
            }
        }

        public void ReloadBuildingsManageMenuInfo___TurretLaser(TurretLaser turretLaser)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isMilitaryBuildingsMenuOpened)
                {
                    // Drop some code here
                    buildingsManageMenuReference.RemoveLaserTurret(turretLaser.name);
                }
            }
        }

        public void ReloadBuildingsManageMenuInfo___TurretMisile(TurretMisile turretMisile)
        {
            if (isBuildingsManageMenuOpened)
            {
                if (isMilitaryBuildingsMenuOpened)
                {
                    // Drop some code here
                    buildingsManageMenuReference.RemoveMisileTurret(turretMisile.name);
                }
            }
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

                // Else means that 1 of the specific shafts tabs was opened and we need to reload specific scroll items
                else
                {
                    switch (mineShaft.type)
                    {
                        case 1:
                        if (isMenuCrystalTabOpened)
                        {
                            unitManageMenuReference.RemoveCrystalScrollItem((CrystalShaft)mineShaft);
                        }
                        break;
                        
                        case 2:
                        if (isMenuIronTabOpened)
                        {
                            unitManageMenuReference.RemoveIronScrollItem((IronShaft)mineShaft);
                        }
                        break;
                        
                        case 3:
                        if (isMenuGelTabOpened)
                        {
                            unitManageMenuReference.RemoveGelScrollItem((GelShaft)mineShaft);
                        }
                        break;
                    }
                }

                // Reload Units becasu units without workplace - became avaliable
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
            // Make buttons inactive
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


        public bool CheckForUnitManageMenuOpened()
        {
            return isUnitManageMenuOpened;
        }

    #endregion
}