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
        GameViewMenu.Instance.TurnOnBuildingsManageMenu();
        
        SetPanel(0);
    }

    // Sets panel to correct index
    public void SetPanel(int index)
    {        
        if (index == 0)
        {
            GameViewMenu.Instance.TurnOnIndustrialBuildingsTab();

            ReloadIndustrialMenu();

            industrialPanel.SetActive(true);
            militaryPanel.SetActive(false);
        }

        else if (index == 1)
        {
            GameViewMenu.Instance.TurnOnMilitaryBuildingsTab();

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

        baseScrollItemBuilding = null;
        antenneScrollItemBuilding = null;
        crystalShaftsScrollItemsBuildings.Clear();
        ironShaftsScrollItemsBuildings.Clear();
        gelShaftsScrollItemsBuildings.Clear();
        garagesScrollItemsBuildings.Clear();
        powerPlantScrollItemsBuildings.Clear();


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

        shiledGeneratorsScrollItemsBuildings.Clear();
        laserTurretsScrollItemsBuildings.Clear();
        misileTurretsScrollItemsBuildings.Clear();


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
        

        prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.shtabReference.maxCurrentHealthPoints;
        prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shtabReference.healthPoints;

        prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.shtabReference.maxCurrentShieldPoints;
        prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shtabReference.shieldPoints;



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


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.antenneReference.healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.antenneReference.shieldPoints;


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



            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.crystalShaftList[i].maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.crystalShaftList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.crystalShaftList[i].maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.crystalShaftList[i].shieldPoints;



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



            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.ironShaftList[i].maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.ironShaftList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.ironShaftList[i].maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.ironShaftList[i].shieldPoints;



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


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.gelShaftList[i].maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.gelShaftList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.gelShaftList[i].maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.gelShaftList[i].shieldPoints;




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


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.garagesList[i].maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.garagesList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.garagesList[i].maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.garagesList[i].shieldPoints;




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


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.powerPlantsList[i].maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.powerPlantsList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.powerPlantsList[i].maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.powerPlantsList[i].shieldPoints;




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
            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.shtabReference.maxCurrentHealthPoints;
            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shtabReference.healthPoints;

            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.shtabReference.maxCurrentShieldPoints;
            baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shtabReference.shieldPoints;
        }

        public void ReloadAntenneHPSP()
        {
            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentHealthPoints;
            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.antenneReference.healthPoints;

            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentShieldPoints;
            antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.antenneReference.shieldPoints;
        }

        public void ReloadCrystalShaftHPSP(CrystalShaft crystalShaft)
        {
            foreach (var i in crystalShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == crystalShaft.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = crystalShaft.maxCurrentHealthPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = crystalShaft.healthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = crystalShaft.maxCurrentShieldPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = crystalShaft.shieldPoints;
                }
            }
        }

        public void ReloadIronShaftHPSP(IronShaft ironShaft)
        {
            foreach (var i in ironShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == ironShaft.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ironShaft.maxCurrentHealthPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ironShaft.healthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ironShaft.maxCurrentShieldPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ironShaft.shieldPoints;
                }
            }
        }

        public void ReloadGelShaftHPSP(GelShaft gelShaft)
        {
            foreach (var i in gelShaftsScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == gelShaft.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = gelShaft.maxCurrentHealthPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = gelShaft.healthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = gelShaft.maxCurrentShieldPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = gelShaft.shieldPoints;
                }
            }
        }

        public void ReloadGarageHPSP(Garage garage)
        {
            foreach (var i in garagesScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == garage.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = garage.maxCurrentHealthPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = garage.healthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = garage.maxCurrentShieldPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = garage.shieldPoints;
                }
            }
        }

        public void ReloadPowerPlantHPSP(PowerPlant powerPlant)
        {
            foreach (var i in powerPlantScrollItemsBuildings)
            {
                if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == powerPlant.name)
                {
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = powerPlant.maxCurrentHealthPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = powerPlant.healthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = powerPlant.maxCurrentShieldPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = powerPlant.shieldPoints;
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


            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.shiledGeneratorsList[i].maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shiledGeneratorsList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.shiledGeneratorsList[i].maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shiledGeneratorsList[i].shieldPoints;




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



            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.laserTurretsList[i].maxCurrentHealthPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.laserTurretsList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.laserTurretsList[i].maxCurrentShieldPoints;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.laserTurretsList[i].shieldPoints;


            prefab.GetComponent<BuildingScrollItemScript>().buildingRef = ResourceManager.Instance.laserTurretsList[i];


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
            prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.misileTurretsList[i].healthPoints;

            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = 100;
            prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.misileTurretsList[i].shieldPoints;





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
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = shieldGenerator.maxCurrentHealthPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = shieldGenerator.healthPoints;

                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = shieldGenerator.maxCurrentShieldPoints;
                    i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = shieldGenerator.shieldPoints;
                }
            }
        }

        public void ReloadLaserTurretHPSP(TurretLaser turretLaser)
        {
            for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
            {
                if (turretLaser == ResourceManager.Instance.laserTurretsList[i])
                {
                    laserTurretsScrollItemsBuildings[i].GetComponent<BuildingScrollItemScript>().buildingRef = turretLaser;
                    var temp = laserTurretsScrollItemsBuildings[i];

                    temp.GetComponent<BuildingScrollItemScript>().buildingName.text = turretLaser.name;

                    temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = turretLaser.maxCurrentHealthPoints;
                    temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = turretLaser.healthPoints;

                    temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = turretLaser.maxCurrentShieldPoints;
                    temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = turretLaser.shieldPoints;
                }
            }
        }

        public void ReloadMisileTurretHPSP(TurretMisile turretMisile)
        {
            for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
            {
                if (turretMisile == ResourceManager.Instance.misileTurretsList[i])
                {
                    misileTurretsScrollItemsBuildings[i].GetComponent<BuildingScrollItemScript>().buildingRef = turretMisile;
                    var temp = misileTurretsScrollItemsBuildings[i];

                    temp.GetComponent<BuildingScrollItemScript>().buildingName.text = turretMisile.name;

                    temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = turretMisile.maxCurrentHealthPoints;
                    temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = turretMisile.healthPoints;

                    temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = turretMisile.maxCurrentShieldPoints;
                    temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = turretMisile.shieldPoints;
                }
            }
        }

    #endregion

#endregion

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");

        GameViewMenu.Instance.TurnOffBuildingsManageMenu();
    }
}
