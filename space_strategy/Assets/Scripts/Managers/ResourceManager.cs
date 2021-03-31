using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    // Resources
    [SerializeField] private int resourceCrystalCount = 500; // Modify here to change start resource count
    [SerializeField] private int resourceIronCount = 500;    // Modify here to change start resource count
    [SerializeField] private int resourceGelCount = 500;     // Modify here to change start resource count


    // Unit Resources
    public List<Unit> unitsList;
    public List<Unit> avaliableUnits;
    public List<Unit> homelessUnits;


    // Buildings
    public List<CrystalShaft> crystalShaftList;
    public List<IronShaft> ironShaftList;
    public List<GelShaft> gelShaftList;
    public List<Garage> garagesList;
    public List<PowerPlant> powerPlantsList;
    public List<TurretLaser> laserTurretsList;
    public List<TurretMisile> misileTurretsList;
    public List<ShieldGenerator> shiledGeneratorsList;
    public Antenne antenneReference;
    public Base shtabReference;


    // Electricity
    private int electricityCount = 20;
    private int electricityNeedCount = 0;
    private bool isPowerOn = true;


    public List<EnemyBomber> enemiesBombers;




    public void SetHomelessUnitOnDeadUnitPlace(Garage newHome)
    {
        if (homelessUnits.Count != 0)
        {
            Unit unitRef = homelessUnits[(homelessUnits.Count)-1];

            newHome.AddHomelessUnit(unitRef);

            homelessUnits.Remove(unitRef);
            avaliableUnits.Add(unitRef);
        }
    }

    public Unit SetAvaliableUnitToWork(Unit workerRef)
    {
        if (avaliableUnits.Count == 0)
        {
            Debug.Log("There is no Avaliable Unit at the moment!");
            return null;
        }
        else
        {
            workerRef = avaliableUnits[(avaliableUnits.Count) - 1];

            avaliableUnits.Remove(workerRef);
            
            Debug.Log("Avaliable Unit is assigned succesfully!");
            
            return workerRef;
        }
    }

    public bool SetNewHomeForUnitFromDestroyedGarage(Unit unit, Garage destroyedGarage)
    {
        for (int i = 0; i < garagesList.Count; i++)
        {
            if (garagesList[i] == destroyedGarage)
            {
                continue; // Pass through same garage
            }

            if (garagesList[i].garageData._garageMembers.Count != 5)
            {
                if (garagesList[i].garageData._numberOfUnitsToCome != 0)
                {
                    Debug.Log("I found new home!");

                    garagesList[i].AddHomelessUnit(unit);

                    if (garagesList[i].garageData._isMenuOpened)
                    {
                        GarageStaticData.garageMenuReference.ReloadUnitManage();
                    }

                    return true;
                }
            }
        }

        return false;
    }






    public void AddCrystalResourcePoints()
    {
        resourceCrystalCount += 5;
        UpdateDisplayingResourcesCount();
    }

    public void AddIronResourcePoints()
    {
        resourceIronCount += 5;
        UpdateDisplayingResourcesCount();
    }

    public void AddGelResourcePoints()
    {
        resourceGelCount += 5;
        UpdateDisplayingResourcesCount();
    }


    private void UpdateDisplayingResourcesCount()
    {
        GameViewMenu.Instance.crystalCounter.text = resourceCrystalCount.ToString();
        GameViewMenu.Instance.ironCounter.text = resourceIronCount.ToString();
        GameViewMenu.Instance.gelCounter.text = resourceGelCount.ToString();
    }




    private void CheckElectricityForSliderExpand()
    {
        if (electricityCount == 80 || electricityNeedCount == 80)
        {
            GameViewMenu.Instance.wholeElectricitySlider.maxValue += 50;
            GameViewMenu.Instance.usingElectricitySlider.maxValue += 50;
        }

        if (electricityCount == 120 || electricityNeedCount == 120)
        {
            GameViewMenu.Instance.wholeElectricitySlider.maxValue += 50;
            GameViewMenu.Instance.usingElectricitySlider.maxValue += 50;
        }
    }

    public void CreatePPandAddElectricityWholeCount()
    {
        electricityCount += 20;
        
        if (electricityCount <= GameViewMenu.Instance.wholeElectricitySlider.maxValue)
        {
            GameViewMenu.Instance.wholeElectricitySlider.value = electricityCount;
        }
        
        CheckElectricityForSliderExpand();

        ElectricityLevelCheck();
    }

    public void DestroyPPandRemoveElectricityWholeCount()
    {
        electricityCount -= 20;

        if (electricityCount <= GameViewMenu.Instance.wholeElectricitySlider.maxValue)
        {
            GameViewMenu.Instance.wholeElectricitySlider.value = electricityCount;
        }

        ElectricityLevelCheck();
    }

    public void CreateBuildingAndAddElectricityNeedCount()
    {
        electricityNeedCount += 5;

        if (electricityNeedCount <= GameViewMenu.Instance.usingElectricitySlider.maxValue)
        {
            GameViewMenu.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        CheckElectricityForSliderExpand();

        ElectricityLevelCheck();
    }

    public void DestroyBuildingAndRemoveElectricityNeedCount()
    {
        electricityNeedCount -= 5;
        
        if (electricityNeedCount <= GameViewMenu.Instance.usingElectricitySlider.maxValue)
        {
            GameViewMenu.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        ElectricityLevelCheck();
    }

    public void CreateUnitAndAddElectricityNeedCount()
    {
        electricityNeedCount++;
        
        if (electricityNeedCount <= GameViewMenu.Instance.usingElectricitySlider.maxValue)
        {
            GameViewMenu.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        CheckElectricityForSliderExpand();

        ElectricityLevelCheck();
    }

    public void DestroyUnitAndRemoveElectricityNeedCount()
    {
        electricityNeedCount--;
        
        if (electricityNeedCount <= GameViewMenu.Instance.usingElectricitySlider.maxValue)
        {
            GameViewMenu.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        ElectricityLevelCheck();
    }

    private void ElectricityLevelCheck()
    {
        if (electricityCount < electricityNeedCount)
        {
            if (isPowerOn)
            {
                isPowerOn = false;
                TurnElectricityOFF();
            }
        }

        if (electricityCount > electricityNeedCount)
        {
            if (!isPowerOn)
            {
                isPowerOn = true;
                TurnElectricityON();
            }
        }
    }

    public bool IsPowerOn()
    {
        return isPowerOn;
    }

    private void TurnElectricityOFF()
    {
        Debug.Log("Power off!");
        // 1 - Turn off all turrets
        // 2 - Turn off buildings manage menu and unit manage menu
        // 3 - If antenne buttons on screen are active - disable them


        // 1 /////////////////////////////////////////
        TurnAllTurretOFF();



        // 2 /////////////////////////////////////////
        GameViewMenu.Instance.TurnOffUnitManageMenuButtonAndBuildingsManageMenuButton();



        // 3 /////////////////////////////////////////
        GameHendler.Instance.resourceDropButton.interactable = false;
        GameHendler.Instance.impusleAttackButton.interactable = false;

        GameHendler.Instance.antenneMenuReference.ReloadButoonManage();
    }

    private void TurnElectricityON()
    {
        Debug.Log("Power On!");

        TurnAllTurretsON();

        GameViewMenu.Instance.TurnOnUnitManageMenuButtonAndBuildingsManageMenuButton();

        
        if (GameHendler.Instance.resourceDropTimer == 0)
        {
            if (antenneReference)
            {
                GameHendler.Instance.resourceDropButton.interactable = true;
            }
        }
        if (GameHendler.Instance.impulsAttackTimer == 0)
        {
            if (antenneReference)
            {
                GameHendler.Instance.impusleAttackButton.interactable = true;
            }
        }

        GameHendler.Instance.antenneMenuReference.ReloadButoonManage();
    }

    private void TurnAllTurretsON()
    {
        foreach(var i in laserTurretsList)
        {
            i.turretData.isPowerON = true;
        }
        foreach(var i in misileTurretsList)
        {
            i.turretData.isPowerON = true;
        }
    }

    private void TurnAllTurretOFF()
    {
        foreach (var i in laserTurretsList)
        {
            i.turretData.isPowerON = false;
        }
        foreach (var i in misileTurretsList)
        {
            i.turretData.isPowerON = false;
        }
    }





    private int _crystalNeedForBuilding = 0;
    private int _ironNeedForBuilding = 0;
    private int _gelNeedForBuilding = 0;

    public void StoreResourceNeed(int crystalsNeed = 0, int ironNeed = 0, int gelNeed = 0)
    {
        _crystalNeedForBuilding = crystalsNeed;
        _ironNeedForBuilding = ironNeed;
        _gelNeedForBuilding = gelNeed;

    }

    public bool ChecResources(int crystalsNeed = 0, int ironNeed = 0, int gelNeed = 0)
    {
        if (resourceCrystalCount >= crystalsNeed && resourceIronCount >= ironNeed && resourceGelCount >= gelNeed)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void DeleteResourcesAfterAction()
    {
        resourceCrystalCount -= _crystalNeedForBuilding;
        resourceIronCount -= _ironNeedForBuilding; 
        resourceGelCount -= _gelNeedForBuilding;

        UpdateDisplayingResourcesCount();
    }

    public void DeleteResourcesAfterAction___1PressAction(int crystalsNeed = 0, int ironNeed = 0, int gelNeed = 0)
    {
        resourceCrystalCount -= crystalsNeed;
        resourceIronCount -= ironNeed; 
        resourceGelCount -= gelNeed;

        UpdateDisplayingResourcesCount();
    }




























    public void UpgradeStatisticsAfterBaseUpgrade()
    {
        foreach (var garage in garagesList)
        {
            garage.InitStatsAfterBaseUpgrade();
        }
        StatsManager.UpgradeStatisticsAfterBaseUpgrade___Garage();


        foreach (var cshaft in crystalShaftList)
        {
            cshaft.InitStatsAfterBaseUpgrade();
        }

        foreach (var ishaft in ironShaftList)
        {
            ishaft.InitStatsAfterBaseUpgrade();
        }

        foreach (var gshaft in gelShaftList)
        {
            gshaft.InitStatsAfterBaseUpgrade();
        }
        
        StatsManager.UpgradeStatisticsAfterBaseUpgrade___MineShaft();


        foreach (var pp in powerPlantsList)
        {
            pp.InitStatsAfterBaseUpgrade();
        }
        StatsManager.UpgradeStatisticsAfterBaseUpgrade___PowerPlant();

        if (antenneReference)
        {
            antenneReference.InitStatsAfterBaseUpgrade();
        }
        StatsManager.UpgradeStatisticsAfterBaseUpgrade___Antenne();


        foreach (var sg in shiledGeneratorsList)
        {
            sg.InitStatsAfterBaseUpgrade();
        }
        StatsManager.UpgradeStatisticsAfterBaseUpgrade___ShieldGenerator();

        foreach (var lt in laserTurretsList)
        {
            lt.InitStatsAfterShtabUpgrade();
        }
        StatsManager.UpgradeStatisticsAfterBaseUpgrade___LaserTurret();

        foreach (var mt in misileTurretsList)
        {
            mt.InitStatsAfterShtabUpgrade();
        }
        StatsManager.UpgradeStatisticsAfterBaseUpgrade___MisileTurret();

        
    }









    public void ConstructBuildingAndRescanMap()
    {
        AstarPath.active.Scan();
        
        foreach(var unit in unitsList)
        {
            unit.RebuildPath();
        }
        foreach(var enemy in enemiesBombers)
        {
            enemy.RebuildCurrentPath();
        }
    }


    public void DestroyPowerPlantAndRescanMap()
    {
        DestroyPPandRemoveElectricityWholeCount();
        AstarPath.active.Scan();
        
        foreach(var unit in unitsList)
        {
            unit.RebuildPath();
        }

        foreach(var enemy in enemiesBombers)
        {
            enemy.RebuildCurrentPath();
        }
    }

    public void DestroyBuildingAndRescanMap()
    {
        DestroyBuildingAndRemoveElectricityNeedCount();
        AstarPath.active.Scan();

        foreach(var unit in unitsList)
        {
            unit.RebuildPath();
        }
        foreach(var enemy in enemiesBombers)
        {
            enemy.RebuildCurrentPath();
        }
    }










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

        crystalShaftList = new List<CrystalShaft>();
        ironShaftList = new List<IronShaft>();
        gelShaftList = new List<GelShaft>();
        garagesList = new List<Garage>();

        unitsList = new List<Unit>();
        avaliableUnits = new List<Unit>();
        homelessUnits = new List<Unit>();

        // Base shtab = Instantiate(PrefabManager.Instance.basePrefab, new Vector3(8.660254f, 6f, 0f) + OffsetConstants.buildingOffset, Quaternion.identity).GetComponent<Base>();
        // ShtabStaticData.InitStaticFields();
        // shtab.ConstructBuilding(null);
        // ResourceManager.Instance.shtabReference = shtab;

        UpdateDisplayingResourcesCount();
    }
}
