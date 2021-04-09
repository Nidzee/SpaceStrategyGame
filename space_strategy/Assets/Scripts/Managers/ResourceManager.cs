using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    // Resources
    [SerializeField] public int resourceCrystalCount = 500; // Modify here to change start resource count
    [SerializeField] public int resourceIronCount = 500;    // Modify here to change start resource count
    [SerializeField] public int resourceGelCount = 500;     // Modify here to change start resource count

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

    // Enemies
    public List<EnemyBomber> enemiesBombers;



    #region DO NOT TOUCH EVER! - Unit managing (Adding to new garage after death of garage...)
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

            if (garagesList[i]._garageMembers.Count != 5)
            {
                if (garagesList[i]._numberOfUnitsToCome != 0)
                {
                    Debug.Log("I found new home!");

                    garagesList[i].AddHomelessUnit(unit);

                    if (garagesList[i]._isMenuOpened)
                    {
                        GarageStaticData.garageMenuReference.ReloadUnitManage();
                    }

                    return true;
                }
            }
        }

        return false;
    }
    #endregion

    #region  Electricity maintaining

    public void CreatePPandAddElectricityWholeCount()
    {
        electricityCount += 20;
        GameViewMenu.Instance.IncreaseElectricityCountSlider(electricityCount, electricityNeedCount);
        ElectricityLevelCheck();
    }

    public void DestroyPPandRemoveElectricityWholeCount()
    {
        electricityCount -= 20;
        GameViewMenu.Instance.DecreaseElectricityCountSlider(electricityCount, electricityNeedCount);
        ElectricityLevelCheck();
    }

    public void CreateBuildingAndAddElectricityNeedCount()
    {
        electricityNeedCount += 5;
        GameViewMenu.Instance.IncreaseElectricityNeedSlider(electricityCount, electricityNeedCount);
        ElectricityLevelCheck();
    }

    public void DestroyBuildingAndRemoveElectricityNeedCount()
    {
        electricityNeedCount -= 5;
        GameViewMenu.Instance.DecreaseElectricityNeedSlider(electricityCount, electricityNeedCount);
        ElectricityLevelCheck();
    }

    public void CreateUnitAndAddElectricityNeedCount()
    {
        electricityNeedCount++;
        GameViewMenu.Instance.IncreaseElectricityNeedSlider(electricityCount, electricityNeedCount);
        ElectricityLevelCheck();
    }

    public void DestroyUnitAndRemoveElectricityNeedCount()
    {
        electricityNeedCount--;
        GameViewMenu.Instance.DecreaseElectricityNeedSlider(electricityCount, electricityNeedCount);
        ElectricityLevelCheck();
    }

    public void ElectricityLevelCheck()
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
        TurnAllTurretOFF();

        GameViewMenu.Instance.TurnOffUnitManageMenuButtonAndBuildingsManageMenuButton();

        if (antenneReference)
        {
            GameHendler.Instance.TurnAntenneButtonsToUnavaliable();
        }
    }

    private void TurnElectricityON()
    {
        TurnAllTurretsON();

        GameViewMenu.Instance.TurnOnUnitManageMenuButtonAndBuildingsManageMenuButton();

        if (antenneReference)
        {
            GameHendler.Instance.TurnAntenneButtonsBackToLife();
        }
    }

    private void TurnAllTurretsON()
    {
        foreach(var i in laserTurretsList)
        {
            i.TurnTurretON();
        }
        foreach(var i in misileTurretsList)
        {
            i.TurnTurretON();
        }
    }

    private void TurnAllTurretOFF()
    {
        foreach (var i in laserTurretsList)
        {
            i.TurnTurretOFF();
        }
        foreach (var i in misileTurretsList)
        {
            i.TurnTurretOFF();
        }
    }
    #endregion

    #region  Resources managing

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
        return (resourceCrystalCount >= crystalsNeed && resourceIronCount >= ironNeed && resourceGelCount >= gelNeed);
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
        GameViewMenu.Instance.UpdateResourcesCount(resourceCrystalCount, resourceIronCount, resourceGelCount);
    }

    public string UnitSittuation()
    {
        return avaliableUnits.Count +"/"+ unitsList.Count;
    }

    #endregion


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

    public void DestroyBuildingAndRescanMap()
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


    public void UpgradeStatisticsAfterBaseUpgrade()
    {
        // Upgradind garages
        foreach (var garage in garagesList)
            garage.InitStatsAfterBaseUpgrade();


        // Upgrading Shafts
        foreach (var cshaft in crystalShaftList)
            cshaft.InitStatsAfterBaseUpgrade();
        foreach (var ishaft in ironShaftList)
            ishaft.InitStatsAfterBaseUpgrade();
        foreach (var gshaft in gelShaftList)
            gshaft.InitStatsAfterBaseUpgrade();


        // Upgrading power plant
        foreach (var pp in powerPlantsList)
            pp.InitStatsAfterBaseUpgrade();


        // Upgrading antenne
        if (antenneReference)
            antenneReference.InitStatsAfterBaseUpgrade();


        // Upgrading shield generators
        foreach (var sg in shiledGeneratorsList)
            sg.InitStatsAfterBaseUpgrade();


        // Upgrading turrets LASER
        foreach (var lt in laserTurretsList)
            lt.InitStatsAfterShtabUpgrade();
        
        
        // Upgrading turrets MISILE
        foreach (var mt in misileTurretsList)
            mt.InitStatsAfterShtabUpgrade();
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

        UpdateDisplayingResourcesCount();
    }























    public void LoadFromFile(SaveData saveData)
    {
        // Resources
        resourceCrystalCount = saveData.crystalResourceCount; // Modify here to change start resource count
        resourceIronCount = saveData.ironResourceCount;   // Modify here to change start resource count
        resourceGelCount = saveData.gelResourceCount;     // Modify here to change start resource count

        // Unit Resources
        unitsList = new List<Unit>();
        avaliableUnits = new List<Unit>();
        homelessUnits = new List<Unit>();

        // Buildings
        crystalShaftList = new List<CrystalShaft>();
        ironShaftList = new List<IronShaft>();
        gelShaftList = new List<GelShaft>();
        garagesList = new List<Garage>();
        powerPlantsList = new List<PowerPlant>();
        laserTurretsList = new List<TurretLaser>();
        misileTurretsList = new List<TurretMisile>();
        shiledGeneratorsList = new List<ShieldGenerator>();
        
        antenneReference = null;

        // Electricity
        electricityCount = saveData.electricity;
        electricityNeedCount = saveData.electricityNeed;

        GameViewMenu.Instance.LoadElectricityFromFile( electricityCount,  saveData.electricity_max,  electricityNeedCount,  saveData.electricityNeed_max);
        GameViewMenu.Instance.UpdateResourcesCount(resourceCrystalCount, resourceIronCount, resourceGelCount);

        isPowerOn = saveData.IsPowerOn;

        // Enemies
        enemiesBombers = new List<EnemyBomber>();
    }
}