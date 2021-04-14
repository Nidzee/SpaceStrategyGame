using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}
    private void Awake()
    {
        Debug.Log("RESOURCE MANAGER START WORKING");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("WAVES")]
    public static int currentWave = 1;
    public static int winWaveCounter = 2;

    [Header("RESOURCES")]
    public int resourceCrystalCount; 
    public int resourceIronCount;
    public int resourceGelCount;

    [Header("ELECTRICITY")]
    public int electricityCount;
    public int electricityCount_max;

    public int electricityNeedCount;
    public int electricityNeedCount_max;
    
    public bool isPowerOn;
    public bool isAntenneOnceCreated;

    [Header("UNIT *RESOURCE*")]
    public List<Unit> unitsList;
    public List<Unit> avaliableUnits;
    public List<Unit> homelessUnits;

    [Header("BUILDINGS")]
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

    [Header("ENEMIES")]
    public List<EnemyBomber> enemiesBombers;




    #region DO NOT TOUCH EVER! - Unit managing (Adding to new garage after death of garage...)
    
    public void SetHomelessUnitOnDeadUnitPlace(Garage newHome) // Correct
    {
        if (homelessUnits.Count != 0)
        {
            // Finds homeless unit
            Unit unitRef = homelessUnits[(homelessUnits.Count)-1];

            // Add homeless unit to particular home
            newHome.AddHomelessUnit(unitRef);

            // Unit maintaining
            homelessUnits.Remove(unitRef);
            avaliableUnits.Add(unitRef);
        }
    }

    public Unit SetAvaliableUnitToWork(Unit workerRef) // Correct
    {
        if (avaliableUnits.Count == 0)
        {
            return null;
        }
        else
        {
            // Set avaliable unit
            workerRef = avaliableUnits[(avaliableUnits.Count) - 1];

            // Remove from list of avaliable units
            avaliableUnits.Remove(workerRef);
            
            // Return reference to avaliable unit
            return workerRef;
        }
    }

    public bool SetNewHomeForUnitFromDestroyedGarage(Unit unit, Garage destroyedGarage) // Correct
    {
        // looping through all garages
        for (int i = 0; i < garagesList.Count; i++)
        {
            // Pass through same "destroyedGarage" garage
            if (garagesList[i] == destroyedGarage)
            {
                continue; 
            }

            // If we found garage && it is not full && it has "_numberOfUnitsToCome" because юніти ще не створились але ми наклацали
            // 5 - maximum garage members count
            if ((garagesList[i]._garageMembers.Count != 5) && (garagesList[i]._clicksOnCreateUnitButton != 5))
            {
                garagesList[i].AddHomelessUnit(unit);

                return true;
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

    public void AddResourceDrop()
    {
        resourceCrystalCount += 100;
        resourceIronCount += 100;
        resourceGelCount += 100;

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

    public void CheckForEndOfWave()
    {
        if (enemiesBombers.Count == 0)
        {
            currentWave++;
            GameViewMenu.Instance.UpdateWaveCounter();
        
            if (ResourceManager.currentWave == ResourceManager.winWaveCounter)
            {
                Time.timeScale = 0f;
                UIPannelManager.Instance.Loose();
                return;
            }

            EnemySpawner.Instance.RestartEnemySpawnTimer();
        }
    }

    public void BashAllEnemies()
    {
        foreach(var item in enemiesBombers)
        {
            item.isBashIntersects = true;
        }
    }


    public void InitStartData()
    {
        antenneReference = null;
        shtabReference = null;

        crystalShaftList = new List<CrystalShaft>();
        ironShaftList = new List<IronShaft>();
        gelShaftList = new List<GelShaft>();
        garagesList = new List<Garage>();
        powerPlantsList = new List<PowerPlant>();
        laserTurretsList = new List<TurretLaser>();
        misileTurretsList = new List<TurretMisile>();
        shiledGeneratorsList = new List<ShieldGenerator>();

        enemiesBombers = new List<EnemyBomber>();

        unitsList = new List<Unit>();
        avaliableUnits = new List<Unit>();
        homelessUnits = new List<Unit>();

        resourceCrystalCount = 500; 
        resourceIronCount = 500; 
        resourceGelCount = 500; 

        electricityCount         = 20;
        electricityCount_max     = 100;
        electricityNeedCount     = 0;
        electricityNeedCount_max = 100;

        isPowerOn = true;
        isAntenneOnceCreated = false;

        UpdateDisplayingResourcesCount();
    }

    public void LoadFromFile(ResourcesSavingData saveData)
    {
        crystalShaftList = new List<CrystalShaft>();
        ironShaftList = new List<IronShaft>();
        gelShaftList = new List<GelShaft>();
        garagesList = new List<Garage>();
        powerPlantsList = new List<PowerPlant>();
        laserTurretsList = new List<TurretLaser>();
        misileTurretsList = new List<TurretMisile>();
        shiledGeneratorsList = new List<ShieldGenerator>();
        enemiesBombers = new List<EnemyBomber>();

        unitsList = new List<Unit>();
        avaliableUnits = new List<Unit>();
        homelessUnits = new List<Unit>();
        
        antenneReference = null;
        shtabReference = null;


        resourceCrystalCount = saveData.crystalResourceCount; // Modify here to change start resource count
        resourceIronCount = saveData.ironResourceCount;   // Modify here to change start resource count
        resourceGelCount = saveData.gelResourceCount;     // Modify here to change start resource count

        electricityCount = saveData.electricity;
        electricityCount_max = saveData.electricity_max;

        electricityNeedCount = saveData.electricityNeed;
        electricityNeedCount_max = saveData.electricityNeed_max;

        isPowerOn = saveData.IsPowerOn;
        isAntenneOnceCreated = saveData.isAntenneOnceCreated;

        winWaveCounter = saveData.winWaveCounter;
        currentWave = saveData.currentWave;

    
        UnitStaticData.unit_counter = saveData.unitCounter;
        GarageStaticData.garage_counter = saveData.gagareCounter;
        CSStaticData.crystalShaft_counter = saveData.crystalShaftCounter;
        ISStaticData.ironShaft_counter = saveData.ironShaftCounter;
        GSStaticData.gelShaft_counter = saveData.gelShaftCounter;
        PowerPlantStaticData.powerPlant_counter = saveData.ppCounter;
        ShiledGeneratorStaticData.shieldGenerator_counter = saveData.sgCounter;
        LTStaticData.turetteLaser_counter = saveData.ltCounter;
        MTStaticData.turetteMisile_counter = saveData.mtCounter;

        GameViewMenu.Instance.InitData();
    }
}