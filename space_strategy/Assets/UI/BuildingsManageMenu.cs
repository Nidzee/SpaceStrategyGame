using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManageMenu : MonoBehaviour
{
    [SerializeField] private GameObject industrialPanel;
    [SerializeField] private GameObject militaryPanel;

    [SerializeField] private GameObject industrialBuildingsContent;
    [SerializeField] private GameObject militaryBuildingsContent;

    [SerializeField] private GameObject scrollBuildingItemPrefab;



    // INSDUSTRIAL scroll items/////////////////////////////////////////////////////////
    private List<GameObject> industrialScrollItems = new List<GameObject>();

    public GameObject baseScrollItemBuilding;
    public GameObject antenneScrollItemBuilding;
    public List<GameObject> crystalShaftsScrollItemsBuildings = new List<GameObject>();
    public List<GameObject> ironShaftsScrollItemsBuildings = new List<GameObject>();
    public List<GameObject> gelShaftsScrollItemsBuildings = new List<GameObject>();
    public List<GameObject> garagesScrollItemsBuildings = new List<GameObject>();
    public List<GameObject> powerPlantScrollItemsBuildings = new List<GameObject>();
    ////////////////////////////////////////////////////////////////////////////////////


    // MILITARY scroll items/////////////////////////////////////////////////////////////
    private List<GameObject> militaryScrollItems = new List<GameObject>();

    public List<GameObject> shiledGeneratorsScrollItemsBuildings = new List<GameObject>();
    public List<GameObject> laserTurretsScrollItemsBuildings = new List<GameObject>();
    public List<GameObject> misileTurretsScrollItemsBuildings = new List<GameObject>();
    /////////////////////////////////////////////////////////////////////////////////////


    // Reloads all Sliders on menu
    public void ReloadPanel()
    {
        GameHendler.Instance.isBuildingsMAnageMenuOpened = true;
        
        SetPanel(0);
    }

    // Sets panel to correct index
    public void SetPanel(int index)
    {
        if (index == 0)
        {
            Debug.Log("Switch to INDUSTRIAL Panel");

            GameHendler.Instance.isIndustrialBuildingsMenuOpened = true;
            GameHendler.Instance.isMilitaryBuildingsMenuOpened = false;

            ReloadIndustrialMenu();

            industrialPanel.SetActive(true);
            militaryPanel.SetActive(false);
        }

        else if (index == 1)
        {
            Debug.Log("Switch to MILITARY Panel");

            GameHendler.Instance.isIndustrialBuildingsMenuOpened = false;
            GameHendler.Instance.isMilitaryBuildingsMenuOpened = true;

            ReloadMilitaryMenu();

            industrialPanel.SetActive(false);
            militaryPanel.SetActive(true);
        }

        else
        {
            Debug.Log("Error - invalid index!");
        }
    }


    public void ReloadIndustrialMenu()
    {
        foreach (var i in industrialScrollItems)
        {
            Destroy(i);
        }
        industrialScrollItems.Clear();

        // Reload BASE
        ReloadBase();

        // Reload ANTENNE
        ReloadAntenne();

        // Reload rest Industrial buildings
        ReloadShafts();
        ReloadGarages();
        ReloadPowerPlants();
    }

    public void ReloadMilitaryMenu()
    {
        foreach (var i in militaryScrollItems)
        {
            Destroy(i);
        }
        militaryScrollItems.Clear();

        //Reload Shield Generators
        ReloadShieldGenerators();

        //Reload laser turrets
        ReloadLaserTurrets();

        //Reload misile turrets
        ReloadMisileTurrets();
    }


#region INDUSTRIAL BUILDINGS REGION

    private void ReloadBase()
    {
        GameObject prefab = Instantiate(scrollBuildingItemPrefab);
        prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);
        

        prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
        prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shtabReference.HealthPoints;

        prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
        prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shtabReference.ShieldPoints;



        prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = "BASE";

        baseScrollItemBuilding = prefab;

        industrialScrollItems.Add(prefab);
    }

    private void ReloadAntenne()
    {
        if (ResourceManager.Instance.antenneReference)
        {
            // Add info about antenne
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.antenneReference.HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.antenneReference.ShieldPoints;


            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = "AN1";

            antenneScrollItemBuilding = prefab;

            industrialScrollItems.Add(prefab);
        }
    }

    private void ReloadShafts()
    {
        // Reload Crystal shafts
        ReloadCrystalShafts();

        // Reload Iron shafts
        ReloadIronShafts();

        // Reload Gel shafts
        ReloadGelShafts();
    }

    private void ReloadCrystalShafts()
    {
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);



            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.crystalShaftList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.crystalShaftList[i].ShieldPoints;



            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.crystalShaftList[i].name.ToString();

            industrialScrollItems.Add(prefab);

            crystalShaftsScrollItemsBuildings.Add(prefab);
        }
    }

    private void ReloadIronShafts()
    {
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);



            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.ironShaftList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.ironShaftList[i].ShieldPoints;



            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.ironShaftList[i].name.ToString();

            industrialScrollItems.Add(prefab);

            ironShaftsScrollItemsBuildings.Add(prefab);
        }
    }

    private void ReloadGelShafts()
    {
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.gelShaftList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.gelShaftList[i].ShieldPoints;




            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.gelShaftList[i].name.ToString();

            industrialScrollItems.Add(prefab);

            gelShaftsScrollItemsBuildings.Add(prefab);
        }
    }

    private void ReloadGarages()
    {
        for (int i = 0; i < ResourceManager.Instance.garagesList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.garagesList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.garagesList[i].ShieldPoints;




            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.garagesList[i].name.ToString();

            industrialScrollItems.Add(prefab);

            garagesScrollItemsBuildings.Add(prefab);
        }
    }

    private void ReloadPowerPlants()
    {
        for (int i = 0; i < ResourceManager.Instance.powerPlantsList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.powerPlantsList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.powerPlantsList[i].ShieldPoints;




            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.powerPlantsList[i].name.ToString();

            industrialScrollItems.Add(prefab);

            powerPlantScrollItemsBuildings.Add(prefab);
        }
    }

    #region Remove industrial buildings

        public void RemoveAntenneFromBuildingsMenu()
        {
            // Maybe here problem 
            Destroy(antenneScrollItemBuilding);
        }

        public void RemoveCrystalShaftFromBuildingsMenu(string crystalShaftName)
        {
            foreach (var i in crystalShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == crystalShaftName)
                {
                    crystalShaftsScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

        public void RemoveIronShaftFromBuildingsMenu(string ironShaftName)
        {
            foreach (var i in ironShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == ironShaftName)
                {
                    ironShaftsScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

        public void RemoveGelShaftFromBuildingsMenu(string gelShaftName)
        {
            foreach (var i in gelShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == gelShaftName)
                {
                    gelShaftsScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

        public void RemoveGarageFromBuildingsMenu(string garageNme)
        {
            foreach (var i in garagesScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == garageNme)
                {
                    garagesScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

        public void RemovePowerPlantFromBuildingsMenu(string ppName)
        {
            foreach (var i in powerPlantScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == ppName)
                {
                    powerPlantScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

    #endregion

    #region Reload industrial HP/SP

        public void ReloadBaseHPSP()
        {
            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shtabReference.HealthPoints;

            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shtabReference.ShieldPoints;
        }

        public void ReloadAntenneHPSP()
        {
            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.antenneReference.HealthPoints;

            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.antenneReference.ShieldPoints;
        }

        public void ReloadCrystalShaftHPSP(CrystalShaft crystalShaft)
        {
            foreach (var i in crystalShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == crystalShaft.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = crystalShaft.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = crystalShaft.ShieldPoints;
                }
            }
        }

        public void ReloadIronShaftHPSP(IronShaft ironShaft)
        {
            foreach (var i in ironShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == ironShaft.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ironShaft.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ironShaft.ShieldPoints;
                }
            }
        }

        public void ReloadGelShaftHPSP(GelShaft gelShaft)
        {
            foreach (var i in gelShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == gelShaft.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = gelShaft.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = gelShaft.ShieldPoints;
                }
            }
        }

        public void ReloadGarageHPSP(Garage garage)
        {
            foreach (var i in garagesScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == garage.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = garage.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = garage.ShieldPoints;
                }
            }
        }

        public void ReloadPowerPlantHPSP(PowerPlant powerPlant)
        {
            foreach (var i in powerPlantScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == powerPlant.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = powerPlant.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = powerPlant.ShieldPoints;
                }
            }
        }

    #endregion

#endregion


#region MILITARY BUILDINGS REGION

    private void ReloadShieldGenerators()
    {
        for (int i = 0; i < ResourceManager.Instance.shiledGeneratorsList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(militaryBuildingsContent.transform, false);


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shiledGeneratorsList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shiledGeneratorsList[i].ShieldPoints;




            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.shiledGeneratorsList[i].name.ToString();

            militaryScrollItems.Add(prefab);

            shiledGeneratorsScrollItemsBuildings.Add(prefab);
        }
    }

    private void ReloadLaserTurrets()
    {
        for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(militaryBuildingsContent.transform, false);



            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.laserTurretsList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.laserTurretsList[i].ShieldPoints;




            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.laserTurretsList[i].name.ToString();

            militaryScrollItems.Add(prefab);

            laserTurretsScrollItemsBuildings.Add(prefab);
        }
    }

    private void ReloadMisileTurrets()
    {
        for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(militaryBuildingsContent.transform, false);



            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.misileTurretsList[i].HealthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.misileTurretsList[i].ShieldPoints;





            prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.misileTurretsList[i].name.ToString();

            militaryScrollItems.Add(prefab);

            misileTurretsScrollItemsBuildings.Add(prefab);
        }
    }

    #region Remove military buildings

        public void RemoveShieldGenerator(string shieldGeneratorName)
        {
            foreach (var i in shiledGeneratorsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == shieldGeneratorName)
                {
                    shiledGeneratorsScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

        public void RemoveLaserTurret(string laserTurretName)
        {
            foreach (var i in laserTurretsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == laserTurretName)
                {
                    laserTurretsScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

        public void RemoveMisileTurret(string misileTurretName)
        {
            foreach (var i in misileTurretsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == misileTurretName)
                {
                    misileTurretsScrollItemsBuildings.Remove(i);
                    Destroy(i);
                    return;
                }
            }
        }

    #endregion

    #region  Reload military HP/SP

        public void ReloadShieldGeneratorHPSP(ShieldGenerator shieldGenerator)
        {
            foreach (var i in shiledGeneratorsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == shieldGenerator.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = shieldGenerator.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = shieldGenerator.ShieldPoints;
                }
            }
        }

        public void ReloadLaserTurretHPSP(TurretLaser turretLaser)
        {
            foreach (var i in laserTurretsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == turretLaser.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = turretLaser.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = turretLaser.ShieldPoints;
                }
            }
        }

        public void ReloadMisileTurretHPSP(TurretMisile turretMisile)
        {
            foreach (var i in misileTurretsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == turretMisile.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = turretMisile.HealthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = turretMisile.ShieldPoints;
                }
            }
        }

    #endregion

#endregion

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");

        GameHendler.Instance.isBuildingsMAnageMenuOpened = false;
        GameHendler.Instance.isIndustrialBuildingsMenuOpened = false;
        GameHendler.Instance.isMilitaryBuildingsMenuOpened = false;
    }
}
