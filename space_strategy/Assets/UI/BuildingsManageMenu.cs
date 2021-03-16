using System.Collections.Generic;
using UnityEngine;

public class BuildingsManageMenu : MonoBehaviour
{
    [SerializeField] private GameObject industrialPanel;
    [SerializeField] private GameObject militaryPanel;
    [SerializeField] private GameObject industrialBuildingsContent;
    [SerializeField] private GameObject militaryBuildingsContent;
    [SerializeField] private GameObject scrollBuildingItemPrefab;


    private List<GameObject> industrialScrollItems = new List<GameObject>();
    private List<GameObject> militaryScrollItems = new List<GameObject>();



    public void ReloadPanel()
    {
        GameViewMenu.Instance.TurnOnBuildingsManageMenu();
        
        SetPanel(0);
    }

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

    
    public void RemoveFromBuildingsMenu(GameUnit gameUnit)
    {
        // Logic can be easily moved to NewScrollItem script

        Debug.Log("Removing!");
        GameObject i = GameObject.Find("isi_" + gameUnit.name);

        if (i) // If objects exists - menu is opened
        {
            industrialScrollItems.Remove(i);
            Destroy(i);
        }

        i = GameObject.Find("msi_" + gameUnit.name);

        if (i) // If objects exists - menu is opened
        {
            Debug.Log("Removing: " + i.name);
            militaryScrollItems.Remove(i);
            Destroy(i);
        }
    }

    public void ReloadHPSP(GameUnit gameUnit)
    {
        GameObject i = GameObject.Find("isi_" + gameUnit.name);

        if (i) // If objects exists - menu is opened
        {
            i.GetComponent<NewBuildingScrollItemScript>().ReloadHPSP(gameUnit);
            return;
        }

        i = GameObject.Find("msi_" + gameUnit.name);

        if (i) // If objects exists - menu is opened
        {
            Debug.Log("Damage!");
            i.GetComponent<NewBuildingScrollItemScript>().ReloadHPSP(gameUnit);
            return;
        }

        Debug.Log("Didnt fin building to reload HP SP: " + gameUnit.name);
    }


    public void ReloadIndustrialMenu()
    {
        foreach (var i in industrialScrollItems)
        {
            Destroy(i);
        }

        industrialScrollItems.Clear();

        ReloadBase();
        ReloadAntenne();
        ReloadCrystalShafts();
        ReloadIronShafts();
        ReloadGelShafts();
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

        ReloadShieldGenerators();

        ReloadLaserTurrets();
        ReloadMisileTurrets();
    }


    public void ReplaceTurretScrollItem(Turette oldTurret, Turette newTurret)
    {
        GameObject i = GameObject.Find("msi_" + oldTurret.name);

        if (i)
        {
            NewBuildingScrollItemScript scrollItem = i.GetComponent<NewBuildingScrollItemScript>();

            scrollItem.ReloadHPSP(newTurret.gameUnit);
            scrollItem.buildingName.text = newTurret.name.ToString();
            scrollItem.name = "msi_" + newTurret.name.ToString();
        }
    }


#region INDUSTRIAL BUILDINGS REGION

    private void ReloadBase()
    {
        // Instantiating scrollItem prefab
        GameObject prefab = Instantiate(scrollBuildingItemPrefab);
        prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

        NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
        Base shtab = ResourceManager.Instance.shtabReference;


        scrollItem.ReloadHPSP(shtab.gameUnit);
        scrollItem.buildingName.text = shtab.name.ToString();
        scrollItem.name = "isi_" + shtab.name.ToString();


        industrialScrollItems.Add(prefab);
    }

    private void ReloadAntenne()
    {
        if (ResourceManager.Instance.antenneReference)
        {
            // Add info about antenne
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            Antenne antenne = ResourceManager.Instance.antenneReference;


            scrollItem.ReloadHPSP(antenne.gameUnit);
            scrollItem.buildingName.text = antenne.name.ToString();
            scrollItem.name = "isi_" + antenne.name.ToString();


            industrialScrollItems.Add(prefab);
        }
    }

    private void ReloadCrystalShafts()
    {
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            CrystalShaft crystalShaft = ResourceManager.Instance.crystalShaftList[i];


            scrollItem.ReloadHPSP(crystalShaft.gameUnit);
            scrollItem.buildingName.text = crystalShaft.name.ToString();
            scrollItem.name = "isi_" + crystalShaft.name.ToString();


            industrialScrollItems.Add(prefab);

            // Put On... inside Crystal Constructor

            // if (!crystalShaft.KOSTUL)
            // {
            //     crystalShaft.BIGKOSTUL(prefab);
            // }

            // if (crystalShaft.KOSTUL)
            // {
            //     crystalShaft.RemoveFromKOSTUL();
            //     crystalShaft.ReinitKOSTUL(prefab);
            // }

            // crystalShaft.OnDamageTaken += prefab.GetComponent<NewBuildingScrollItemScript>().ReloadHPSP;
            // // crystalShaft.OnShaftDestroyed += prefab.GetComponent<NewBuildingScrollItemScript>().RemoveScrollItem;
            // crystalShaft.OnShaftDestroyed += RemoveCrystalShaftFromBuildingsMenu;
        }
    }

    private void ReloadIronShafts()
    {
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++)
        {
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            IronShaft isronShaft = ResourceManager.Instance.ironShaftList[i];


            scrollItem.ReloadHPSP(isronShaft.gameUnit);
            scrollItem.buildingName.text = isronShaft.name.ToString();
            scrollItem.name = "isi_" + isronShaft.name.ToString();


            industrialScrollItems.Add(prefab);
        }
    }

    private void ReloadGelShafts()
    {
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            GelShaft gelShaft = ResourceManager.Instance.gelShaftList[i];


            scrollItem.ReloadHPSP(gelShaft.gameUnit);
            scrollItem.buildingName.text = gelShaft.name.ToString();
            scrollItem.name = "isi_" + gelShaft.name.ToString();


            industrialScrollItems.Add(prefab);
        }
    }

    private void ReloadGarages()
    {
        for (int i = 0; i < ResourceManager.Instance.garagesList.Count; i++)
        {
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            Garage garage = ResourceManager.Instance.garagesList[i];


            scrollItem.ReloadHPSP(garage.gameUnit);
            scrollItem.buildingName.text = garage.name.ToString();
            scrollItem.name = "isi_" + garage.name.ToString();


            industrialScrollItems.Add(prefab);
        }
    }

    private void ReloadPowerPlants()
    {
        for (int i = 0; i < ResourceManager.Instance.powerPlantsList.Count; i++)
        {
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            PowerPlant powerPlant = ResourceManager.Instance.powerPlantsList[i];


            scrollItem.ReloadHPSP(powerPlant.gameUnit);
            scrollItem.buildingName.text = powerPlant.name.ToString();
            scrollItem.name = "isi_" + powerPlant.name.ToString();


            industrialScrollItems.Add(prefab);
        }
    }

#endregion



#region MILITARY BUILDINGS REGION

    private void ReloadShieldGenerators()
    {
        for (int i = 0; i < ResourceManager.Instance.shiledGeneratorsList.Count; i++)
        {
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(militaryBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            ShieldGenerator sg = ResourceManager.Instance.shiledGeneratorsList[i];


            scrollItem.ReloadHPSP(sg.gameUnit);
            scrollItem.buildingName.text = sg.name.ToString();
            scrollItem.name = "msi_" + sg.name.ToString();


            militaryScrollItems.Add(prefab);
        }
    }









    private void ReloadLaserTurrets()
    {
        for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
        {
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(militaryBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            TurretLaser turret = ResourceManager.Instance.laserTurretsList[i];


            scrollItem.ReloadHPSP(turret.gameUnit);
            scrollItem.buildingName.text = turret.name.ToString();
            scrollItem.name = "msi_" + turret.name.ToString();


            militaryScrollItems.Add(prefab);
        }
    }

    private void ReloadMisileTurrets()
    {
        for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
        {
            GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            prefab.gameObject.transform.SetParent(militaryBuildingsContent.transform, false);

            NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
            TurretMisile turret = ResourceManager.Instance.misileTurretsList[i];


            scrollItem.ReloadHPSP(turret.gameUnit);
            scrollItem.buildingName.text = turret.name.ToString();
            scrollItem.name = "msi_" + turret.name.ToString();


            militaryScrollItems.Add(prefab);
        }
    }






















        // public void RemoveLaserTurret(string laserTurretName)
        // {
        //     foreach (var i in laserTurretsScrollItemsBuildings)
        //     {
        //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == laserTurretName)
        //         {
        //             laserTurretsScrollItemsBuildings.Remove(i);
        //             Destroy(i);
        //             return;
        //         }
        //     }
        // }

        // public void RemoveMisileTurret(string misileTurretName)
        // {
        //     foreach (var i in misileTurretsScrollItemsBuildings)
        //     {
        //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == misileTurretName)
        //         {
        //             misileTurretsScrollItemsBuildings.Remove(i);
        //             Destroy(i);
        //             return;
        //         }
        //     }
        // }


        // public void ReloadLaserTurretHPSP(TurretLaser turretLaser)
        // {
        //     for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
        //     {
        //         if (turretLaser == ResourceManager.Instance.laserTurretsList[i])
        //         {
        //             laserTurretsScrollItemsBuildings[i].GetComponent<BuildingScrollItemScript>().buildingRef = turretLaser;
        //             var temp = laserTurretsScrollItemsBuildings[i];

        //             temp.GetComponent<BuildingScrollItemScript>().buildingName.text = turretLaser.name;

        //             temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = turretLaser.maxCurrentHealthPoints;
        //             temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = turretLaser.healthPoints;

        //             temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = turretLaser.maxCurrentShieldPoints;
        //             temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = turretLaser.shieldPoints;
        //         }
        //     }
        // }

        // public void ReloadMisileTurretHPSP(TurretMisile turretMisile)
        // {
        //     for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
        //     {
        //         if (turretMisile == ResourceManager.Instance.misileTurretsList[i])
        //         {
        //             misileTurretsScrollItemsBuildings[i].GetComponent<BuildingScrollItemScript>().buildingRef = turretMisile;
        //             var temp = misileTurretsScrollItemsBuildings[i];

        //             temp.GetComponent<BuildingScrollItemScript>().buildingName.text = turretMisile.name;

        //             temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = turretMisile.maxCurrentHealthPoints;
        //             temp.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = turretMisile.healthPoints;

        //             temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = turretMisile.maxCurrentShieldPoints;
        //             temp.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = turretMisile.shieldPoints;
        //         }
        //     }
        // }

#endregion


    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");

        GameViewMenu.Instance.TurnOffBuildingsManageMenu();
    }
}



    // public void RemoveAntenneFromBuildingsMenu(GameUnit gameUnit)
        // {
        //     // Maybe here problem 
        //     Destroy(antenneScrollItemBuilding);
        // }

        // public void RemoveIronShaftFromBuildingsMenu(GameUnit gameUnit)
        // {
        //     foreach (var i in ironShaftsScrollItemsBuildings)
        //     {
        //         if (i.GetComponent<NewBuildingScrollItemScript>().buildingName.text == gameUnit.name)
        //         {
        //             ironShaftsScrollItemsBuildings.Remove(i);
        //             Destroy(i);
        //             return;
        //         }
        //     }
        // }

        // public void RemoveGelShaftFromBuildingsMenu(GameUnit gameUnit)
        // {
        //     foreach (var i in gelShaftsScrollItemsBuildings)
        //     {
        //         if (i.GetComponent<NewBuildingScrollItemScript>().buildingName.text == gameUnit.name)
        //         {
        //             gelShaftsScrollItemsBuildings.Remove(i);
        //             Destroy(i);
        //             return;
        //         }
        //     }
        // }

        // public void RemoveGarageFromBuildingsMenu(GameUnit gameUnit) 
        // {
        //     foreach (var i in garagesScrollItemsBuildings)
        //     {
        //         if (i.GetComponent<NewBuildingScrollItemScript>().buildingName.text == gameUnit.name)
        //         {
        //             garagesScrollItemsBuildings.Remove(i);
        //             Destroy(i);
        //             return;
        //         }
        //     }
        // }

        // public void RemovePowerPlantFromBuildingsMenu(GameUnit gameUnit)
        // {
        //     foreach (var i in powerPlantScrollItemsBuildings)
        //     {
        //         if (i.GetComponent<NewBuildingScrollItemScript>().buildingName.text == gameUnit.name)
        //         {
        //             powerPlantScrollItemsBuildings.Remove(i);
        //             Destroy(i);
        //             return;
        //         }
        //     }
        // }


    // #region Reload industrial HP/SP

    //     // public void ReloadBaseHPSP()
    //     // {
    //     //     baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.shtabReference.maxCurrentHealthPoints;
    //     //     baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shtabReference.healthPoints;

    //     //     baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.shtabReference.maxCurrentShieldPoints;
    //     //     baseScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shtabReference.shieldPoints;
    //     // }

    //     // public void ReloadAntenneHPSP()
    //     // {
    //     //     // antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentHealthPoints;
    //     //     // antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.antenneReference.healthPoints;

    //     //     // antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentShieldPoints;
    //     //     // antenneScrollItemBuilding.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.antenneReference.shieldPoints;
    //     // }

    //     // public void ReloadCrystalShaftHPSP(CrystalShaft crystalShaft)
    //     // {
    //     //     foreach (var i in crystalShaftsScrollItemsBuildings)
    //     //     {
    //     //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == crystalShaft.name)
    //     //         {
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = crystalShaft.aliveGameUnitData.maxCurrentHealthPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = crystalShaft.aliveGameUnitData.healthPoints;

    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = crystalShaft.aliveGameUnitData.maxCurrentShieldPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = crystalShaft.aliveGameUnitData.shieldPoints;
    //     //         }
    //     //     }
    //     // }

    //     // public void ReloadIronShaftHPSP(IronShaft ironShaft)
    //     // {
    //     //     foreach (var i in ironShaftsScrollItemsBuildings)
    //     //     {
    //     //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == ironShaft.name)
    //     //         {
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ironShaft.aliveGameUnitData.maxCurrentHealthPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ironShaft.aliveGameUnitData.healthPoints;

    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ironShaft.aliveGameUnitData.maxCurrentShieldPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ironShaft.aliveGameUnitData.shieldPoints;
    //     //         }
    //     //     }
    //     // }

    //     // public void ReloadGelShaftHPSP(GelShaft gelShaft)
    //     // {
    //     //     foreach (var i in gelShaftsScrollItemsBuildings)
    //     //     {
    //     //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == gelShaft.name)
    //     //         {
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = gelShaft.aliveGameUnitData.maxCurrentHealthPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = gelShaft.aliveGameUnitData.healthPoints;

    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = gelShaft.aliveGameUnitData.maxCurrentShieldPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = gelShaft.aliveGameUnitData.shieldPoints;
    //     //         }
    //     //     }
    //     // }

    //     // public void ReloadPowerPlantHPSP(PowerPlant powerPlant)
    //     // {
    //     //     foreach (var i in powerPlantScrollItemsBuildings)
    //     //     {
    //     //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == powerPlant.name)
    //     //         {
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = powerPlant.aliveGameUnitData.maxCurrentHealthPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = powerPlant.aliveGameUnitData.healthPoints;

    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = powerPlant.aliveGameUnitData.maxCurrentShieldPoints;
    //     //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = powerPlant.aliveGameUnitData.shieldPoints;
    //     //         }
    //     //     }
    //     // }

    // #endregion




    // // Instantiating scrollItem prefab
            // GameObject prefab = Instantiate(scrollBuildingItemPrefab);
            // prefab.gameObject.transform.SetParent(militaryBuildingsContent.transform, false);


            // prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = ResourceManager.Instance.shiledGeneratorsList[i].maxCurrentHealthPoints;
            // prefab.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = ResourceManager.Instance.shiledGeneratorsList[i].healthPoints;

            // prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = ResourceManager.Instance.shiledGeneratorsList[i].maxCurrentShieldPoints;
            // prefab.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = ResourceManager.Instance.shiledGeneratorsList[i].shieldPoints;




            // prefab.GetComponent<BuildingScrollItemScript>().buildingName.text = ResourceManager.Instance.shiledGeneratorsList[i].name.ToString();

            // militaryScrollItems.Add(prefab);

            // shiledGeneratorsScrollItemsBuildings.Add(prefab);

    // public void RemoveShieldGenerator(string shieldGeneratorName)
    // {
    //     foreach (var i in shiledGeneratorsScrollItemsBuildings)
    //     {
    //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == shieldGeneratorName)
    //         {
    //             shiledGeneratorsScrollItemsBuildings.Remove(i);
    //             Destroy(i);
    //             return;
    //         }
    //     }
    // }

    // public void ReloadShieldGeneratorHPSP(ShieldGenerator shieldGenerator)
    // {
    //     foreach (var i in shiledGeneratorsScrollItemsBuildings)
    //     {
    //         if (i.GetComponent<BuildingScrollItemScript>().buildingName.text == shieldGenerator.name)
    //         {
    //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.maxValue = shieldGenerator.maxCurrentHealthPoints;
    //             i.GetComponent<BuildingScrollItemScript>().buildingHPslider.value = shieldGenerator.healthPoints;

    //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.maxValue = shieldGenerator.maxCurrentShieldPoints;
    //             i.GetComponent<BuildingScrollItemScript>().buildingSPslider.value = shieldGenerator.shieldPoints;
    //         }
    //     }
    // }
