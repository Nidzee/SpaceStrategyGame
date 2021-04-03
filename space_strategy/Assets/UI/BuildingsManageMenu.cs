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

    



    public void RemoveFromBuildingsMenu(AliveGameUnit gameUnit)
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

    public void ReloadHPSP(AliveGameUnit gameUnit)
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

    public void ReplaceTurretScrollItem(Turette oldTurret, Turette newTurret)
    {
        GameObject i = GameObject.Find("msi_" + oldTurret.name);

        if (i)
        {
            NewBuildingScrollItemScript scrollItem = i.GetComponent<NewBuildingScrollItemScript>();

            scrollItem.ReloadHPSP(newTurret);
            scrollItem.buildingName.text = newTurret.name.ToString();
            scrollItem.name = "msi_" + newTurret.name.ToString();
        }
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



#region INDUSTRIAL BUILDINGS REGION

    private void ReloadBase()
    {
        // Instantiating scrollItem prefab
        GameObject prefab = Instantiate(scrollBuildingItemPrefab);
        prefab.gameObject.transform.SetParent(industrialBuildingsContent.transform, false);

        NewBuildingScrollItemScript scrollItem = prefab.GetComponent<NewBuildingScrollItemScript>();
        Base shtab = ResourceManager.Instance.shtabReference;


        scrollItem.ReloadHPSP(shtab);
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


            scrollItem.ReloadHPSP(antenne);
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


            scrollItem.ReloadHPSP(crystalShaft);
            scrollItem.buildingName.text = crystalShaft.name.ToString();
            scrollItem.name = "isi_" + crystalShaft.name.ToString();


            industrialScrollItems.Add(prefab);
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


            scrollItem.ReloadHPSP(isronShaft);
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


            scrollItem.ReloadHPSP(gelShaft);
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


            scrollItem.ReloadHPSP(garage);
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


            scrollItem.ReloadHPSP(powerPlant);
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


            scrollItem.ReloadHPSP(sg);
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


            scrollItem.ReloadHPSP(turret);
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


            scrollItem.ReloadHPSP(turret);
            scrollItem.buildingName.text = turret.name.ToString();
            scrollItem.name = "msi_" + turret.name.ToString();


            militaryScrollItems.Add(prefab);
        }
    }

#endregion


    public void ExitMenu()
    {
        GameViewMenu.Instance.TurnOffBuildingsManageMenu();

        UIPannelManager.Instance.ResetPanels("GameView");
    }
}