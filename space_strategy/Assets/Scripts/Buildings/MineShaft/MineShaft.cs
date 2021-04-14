using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MineShaft : AliveGameUnit, IBuilding
{
    // Events
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public delegate void Upgraded(MineShaft shaft);
    public delegate void ShaftDestroy(AliveGameUnit gameUnit);
    public delegate void UnitManipulated();
    public delegate void ShaftDestroyUnitManaging();
    public delegate void ShaftDestroyForUnitManageMenu(MineShaft shaft);
    public delegate void UnitManipulatedForUnitManageMenu(MineShaft shaft);
    public delegate void UpgradedForUnitManageMenu();

    public event DamageTaken OnDamageTaken = delegate{};
    public event UnitManipulated OnUnitManipulated = delegate{};
    public event UnitManipulatedForUnitManageMenu OnUnitManipulatedForUnitManageMenu = delegate{};
    public event Upgraded OnUpgraded = delegate{};
    public event UpgradedForUnitManageMenu OnUpgradedForUnitManageMenu = delegate{};
    public event ShaftDestroy OnShaftDestroyed = delegate{};
    public event ShaftDestroyUnitManaging OnShaftDestroyedUnitManipulations = delegate{};
    public event ShaftDestroyForUnitManageMenu OnShaftDestroyedForUnitManageMenu = delegate{};


    // Particular shaft data for saving
    public MineShaftSavingData mineShaftSavingData = null;
    public int rotation = 0;  
    public int[] _shaftWorkersIDs = null;
    public GameObject _tileOccupied = null;
    public GameObject _tileOccupied1 = null;
    public int capacity = 0;
    public int type = 0;
    public int level = 0;
    public float upgradeTimer = 0f;


    // Common shaft data
    public Unit _workerRef = null;
    public List<Unit> unitsWorkers = new List<Unit>();
    public GameObject dispenser;            // Init in inspector
    public bool isMenuOpened = false;


    // UI
    public GameObject canvas;               // Init in inspector
    public Slider healthBar;                // Init in inspector
    public Slider shieldhBar;               // Init in inspector


    public void SaveData()
    {
        mineShaftSavingData = new MineShaftSavingData();

        mineShaftSavingData.name = name;
        mineShaftSavingData.rotation = rotation;

        mineShaftSavingData.healthPoints = healthPoints;
        mineShaftSavingData.shieldPoints = shieldPoints;
        mineShaftSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        mineShaftSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        mineShaftSavingData.deffencePoints = deffencePoints;
        mineShaftSavingData.isShieldOn = isShieldOn;
        mineShaftSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        mineShaftSavingData._shaftWorkersIDs = new int[unitsWorkers.Count];
        for (int i = 0; i < unitsWorkers.Count; i++)
        {
            mineShaftSavingData._shaftWorkersIDs[i] = unitsWorkers[i].ID;
        }

        mineShaftSavingData.type = type;
        mineShaftSavingData.capacity = capacity;
        mineShaftSavingData.level = level;

        mineShaftSavingData._tileOccupiedName = _tileOccupied.name;
        if(_tileOccupied1 != null)
        {
            mineShaftSavingData._tileOccupied1Name = _tileOccupied1.name;
        }

        mineShaftSavingData.upgradeTimer = upgradeTimer;


        GameHendler.Instance.mineShaftsSaved.Add(mineShaftSavingData);
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyBuilding();
            return;
        }

        canvas.SetActive(true);

        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        StopCoroutine("UICanvasmaintaining");
        uiCanvasDissapearingTimer = 0f;
        StartCoroutine("UICanvasmaintaining");

        OnDamageTaken(this);
    }

    IEnumerator UICanvasmaintaining()
    {
        while (uiCanvasDissapearingTimer < 3)
        {
            uiCanvasDissapearingTimer += Time.deltaTime;
            yield return null;
        }
        uiCanvasDissapearingTimer = 0;
        canvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

    public void GetResourcesNeedToExpand(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        if (level == 1)
        {
            crystalNeed = StatsManager._crystalNeedForExpand_ToLvl2_Shaft;
            ironNeed = StatsManager._ironNeedForForExpand_ToLvl2_Shaft;
            gelNeed = StatsManager._gelNeedForForExpand_ToLvl2_Shaft;
        }

        else
        {
            crystalNeed = StatsManager._crystalNeedForExpand_ToLvl3_Shaft;
            ironNeed = StatsManager._ironNeedForForExpand_ToLvl3_Shaft;
            gelNeed = StatsManager._gelNeedForForExpand_ToLvl3_Shaft;
        }
    }

    public Transform GetUnitDestination()
    {
        return dispenser.transform;
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShaftMenu");

        MineShaftStaticData.shaftMenuReference.ReloadPanel(this);
    }


    #region Upgrade logic functions

    public IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += MineShaftStaticData._timerStep * Time.deltaTime;

            // Reload fill circles
            if (isMenuOpened)
            {
                switch(level)
                {
                    case 1:
                    MineShaftStaticData.shaftMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    MineShaftStaticData.shaftMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.LogError("Error!   Unknown uprading circle!");
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;

        if (level == 1)
        {
            UpgradeToLvl2();
        }
        else if (level == 2)
        {
            UpgradeToLvl3();
        }
        else
        {
            Debug.LogError("ERROR! - Invalid shaft level upgrading!");
        }
    }

    public void StartShaftUpgradeProcess()
    {
        StartCoroutine(UpgradeLogic());
    }

    public void UpgradeToLvl2()
    {
        level = 2;
        capacity = 5; 

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl2_Shaft_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl2_Shaft_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl2_Shaft_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl2_Shaft_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl2_Shaft_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl2_Shaft_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl2_Shaft_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl2_Shaft_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl2_Shaft_Base_Lvl_3;
            break;
        }

        UpgradeStats(health, shield, defense);

        // Reload main menu HP slider
        // Reload buildings Manage menu
        // Reload current slider in unit manage menu
        // Reload 3 TABS
        OnUpgraded(this);
        OnUpgradedForUnitManageMenu();
    }

    public void UpgradeToLvl3()
    {
        level = 3;
        capacity = 7; 
        
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl3_Shaft_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl3_Shaft_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl3_Shaft_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl3_Shaft_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl3_Shaft_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl3_Shaft_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl3_Shaft_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl3_Shaft_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl3_Shaft_Base_Lvl_3;
            break;
        }

        UpgradeStats(health, shield, defense);

        // Reload main menu HP slider
        // Reload buildings Manage menu
        // Reload current slider in unit manage menu
        // Reload 3 TABS
        OnUpgraded(this);
        OnUpgradedForUnitManageMenu();
    }

    public void InitStatsAfterBaseUpgrade()
    {
        int newHealth = 0;
        int newShield = 0;
        int newDefense = 0;
        
        switch (level)
        {
            case 1:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl1_Shaft_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl1_Shaft_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl1_Shaft_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl1_Shaft_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl1_Shaft_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl1_Shaft_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl1_Shaft_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl1_Shaft_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl1_Shaft_Base_Lvl_3;
                break;
            }
            break;

            case 2:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl2_Shaft_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl2_Shaft_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl2_Shaft_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl2_Shaft_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl2_Shaft_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl2_Shaft_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl2_Shaft_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl2_Shaft_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl2_Shaft_Base_Lvl_3;
                break;
            }
            break;

            case 3:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl3_Shaft_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl3_Shaft_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl3_Shaft_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl3_Shaft_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl3_Shaft_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl3_Shaft_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl3_Shaft_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl3_Shaft_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl3_Shaft_Base_Lvl_3;
                break;
            }
            break;
        }

        UpgradeStats(newHealth, newShield, newDefense);


        OnDamageTaken(this); // Kostul'
    }

    #endregion

    #region  Building constructing and destroying
    
    public virtual void ConstructBuilding(Model model)
    {
        #region Data initialization

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl1_Shaft_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl1_Shaft_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl1_Shaft_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl1_Shaft_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl1_Shaft_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl1_Shaft_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl1_Shaft_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl1_Shaft_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl1_Shaft_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);

        rotation = model.rotation; 
        capacity = 3;
        level = 1;

        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);

        switch (type)
        {
            case 1:
            _tileOccupied = model.BTileZero;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            break;

            case 2:
            _tileOccupied = model.BTileZero;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            break;

            case 3:
            _tileOccupied = model.BTileZero;
            _tileOccupied1 = model.BTileOne;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            break;
        }
        
        #endregion

        InitializeEventsAndBuildingMapData();

        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void ConstructBuildingFromFile(MineShaftSavingData mineShaftSavingData)
    {
        #region Data initialization

        InitGameUnitFromFile(
        mineShaftSavingData.healthPoints, 
        mineShaftSavingData.maxCurrentHealthPoints,
        mineShaftSavingData.shieldPoints,
        mineShaftSavingData.maxCurrentShieldPoints,
        mineShaftSavingData.deffencePoints,
        mineShaftSavingData.isShieldOn,
        mineShaftSavingData.shieldGeneratorInfluencers);

        name = mineShaftSavingData.name;

        rotation = mineShaftSavingData.rotation;
        _shaftWorkersIDs = mineShaftSavingData._shaftWorkersIDs;

        _tileOccupied = GameObject.Find(mineShaftSavingData._tileOccupiedName);
        if(mineShaftSavingData._tileOccupied1Name != "")
        {
            _tileOccupied1 = GameObject.Find(mineShaftSavingData._tileOccupied1Name);
        }
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        if (_tileOccupied1)
        {
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        }

        capacity = mineShaftSavingData.capacity;
        type = mineShaftSavingData.type;
        level = mineShaftSavingData.level;
        upgradeTimer = mineShaftSavingData.upgradeTimer;

        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);

        switch(type)
        {
            case 1:
            GetComponent<CrystalShaft>().ConstructShaftFromFile();
            break;

            case 2:
            GetComponent<IronShaft>().ConstructShaftFromFile();
            break;

            case 3:
            GetComponent<GelShaft>().ConstructShaftFromFile();
            break;
        }

        #endregion

        InitializeEventsAndBuildingMapData();

        // Start timer
        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }
 
    public void DestroyBuilding()
    {
        // Closing UI menu
        if (isMenuOpened)
        {
            MineShaftStaticData.shaftMenuReference.ExitMenu();
        }

        // Removing all units
        foreach (var unit in unitsWorkers)
        {
            unit.WorkPlace = null;
            ResourceManager.Instance.avaliableUnits.Add(unit);
        }
        unitsWorkers.Clear();

        
        // Deleting map data
        switch (type)
        {
            case 1: // Crystal
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS1_crystal;
            ResourceManager.Instance.crystalShaftList.Remove(this.GetComponent<CrystalShaft>());
            break;

            case 2: // Iron
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS2_iron;
            ResourceManager.Instance.ironShaftList.Remove(this.GetComponent<IronShaft>());
            break;

            case 3: // Gel
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS3_gel;
            _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
            ResourceManager.Instance.gelShaftList.Remove(this.GetComponent<GelShaft>());
            break;
        }


        OnShaftDestroyed(this);
        OnShaftDestroyedForUnitManageMenu(this);
        OnShaftDestroyedUnitManipulations();

        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    private void InitializeEventsAndBuildingMapData()
    {
        #region Eventes initalization
        // Reload menu
        // Reload buildings manage menu
        OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += BuildingsManageMenu.Instance.ReloadHPSP;


        // Reload slider in menu
        // Reload Unit count
        // Reload unit manage menu 3 TABS
        // Reload unit manage menu sliders
        OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulatedForUnitManageMenu += UnitManageMenu.Instance.FindSLiderAndReload;
        

        // Reload menu
        // Reload buildings manage menu
        // Reload 3 TABS
        // Reload unit manage menu current slider
        OnUpgraded += MineShaftStaticData.shaftMenuReference.UpdateUIAfterUpgrade;
        OnUpgraded += BuildingsManageMenu.Instance.ReloadHPSP;
        OnUpgraded += UnitManageMenu.Instance.FindSLiderAndReload;


        // Remove from buildings manag menu
        // Remove from scroll items in unit manage menu
        // Reload main unit count
        // Reload 3 TABS with particular type
        OnShaftDestroyed += BuildingsManageMenu.Instance.RemoveFromBuildingsMenu;
        OnShaftDestroyedUnitManipulations += GameViewMenu.Instance.ReloadMainUnitCount;
        OnShaftDestroyedForUnitManageMenu += UnitManageMenu.Instance.RemoveMineShaftFromScrollItems;

        switch (type)
        {
            case 1:
            OnShaftDestroyedUnitManipulations += UnitManageMenu.Instance.ReloadCrystalSlider;
            OnUnitManipulated += UnitManageMenu.Instance.ReloadCrystalSlider;
            OnUpgradedForUnitManageMenu += UnitManageMenu.Instance.ReloadCrystalSlider;
            break;

            case 2:
            OnShaftDestroyedUnitManipulations += UnitManageMenu.Instance.ReloadIronSlider;
            OnUnitManipulated += UnitManageMenu.Instance.ReloadIronSlider;
            OnUpgradedForUnitManageMenu += UnitManageMenu.Instance.ReloadIronSlider;
            break;

            case 3:
            OnShaftDestroyedUnitManipulations += UnitManageMenu.Instance.ReloadGelSlider;
            OnUnitManipulated += UnitManageMenu.Instance.ReloadGelSlider;
            OnUpgradedForUnitManageMenu += UnitManageMenu.Instance.ReloadGelSlider;
            break;
        }
        #endregion
        
        #region Building map info

        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        switch (type)
        {
            case 1:
            info.mapPoints = new Transform[1];
            info.mapPoints[0] = _tileOccupied.transform;
            break;

            case 2:
            info.mapPoints = new Transform[1];
            info.mapPoints[0] = _tileOccupied.transform;
            break;

            case 3:
            info.mapPoints = new Transform[2];
            info.mapPoints[0] = _tileOccupied.transform;
            info.mapPoints[1] = _tileOccupied1.transform;
            break;
        }
        
        #endregion
    }
    
    #endregion

    #region Unit managing

    public void CreateRelations()
    {
        for (int i = 0; i < _shaftWorkersIDs.Length; i++)
        {
            for (int j = 0; j < ResourceManager.Instance.unitsList.Count; j++)
            {
                if (_shaftWorkersIDs[i] == ResourceManager.Instance.unitsList[j].ID)
                {
                    Debug.Log("Add unit to shaft!" + this.name + "   " + ResourceManager.Instance.unitsList[j].name);

                    
                    ResourceManager.Instance.avaliableUnits.Remove(ResourceManager.Instance.unitsList[j]);


                    unitsWorkers.Add(ResourceManager.Instance.unitsList[j]);
                    ResourceManager.Instance.unitsList[j].WorkPlace = this;
                    j = 0;
                    break;
                }
            }
        }
    }

    public void AddWorkerViaSlider()
    {
        _workerRef = ResourceManager.Instance.SetAvaliableUnitToWork(_workerRef); // Initialize adding unit reference

        if (!_workerRef)
        {
            Debug.Log("No Units avaliable!");
            return;
        }
        
        _workerRef.WorkPlace = this;
        unitsWorkers.Add(_workerRef);
        ResourceManager.Instance.avaliableUnits.Remove(_workerRef);
        _workerRef = null;


        OnUnitManipulated();
        OnUnitManipulatedForUnitManageMenu(this);
    }
 
    public void RemoveWorkerViaSlider()
    {
        _workerRef = unitsWorkers[(unitsWorkers.Count)-1];
        _workerRef.WorkPlace = null;     // Set workplace - null
        unitsWorkers.Remove(_workerRef); // Remove from workers list
        ResourceManager.Instance.avaliableUnits.Add(_workerRef);
        _workerRef = null;


        OnUnitManipulated();
        OnUnitManipulatedForUnitManageMenu(this);
    }

    public void RemoveUnit(Unit unit)
    {
        unit.WorkPlace = null;     // Set workplace - null
        unitsWorkers.Remove(unit); // Remove from workers list


        OnUnitManipulated();
        OnUnitManipulatedForUnitManageMenu(this);
    }

    #endregion
}