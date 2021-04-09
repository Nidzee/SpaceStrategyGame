using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class MineShaft : AliveGameUnit, IBuilding
{
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public delegate void Upgraded(MineShaft shaft);
    public event Upgraded OnUpgraded = delegate{};
    public delegate void ShaftDestroy(AliveGameUnit gameUnit);
    public event ShaftDestroy OnShaftDestroyed = delegate{};
    public delegate void UnitManipulated();
    public event UnitManipulated OnUnitManipulated = delegate{};

    public MineShaftSavingData mineShaftSavingData;


    public int ID;
    public int rotation;  
    public int[] _shaftWorkersIDs;             // Units that are living here  
    public GameObject _tileOccupied;           // Reference to real MapTile on which building is set
    public GameObject _tileOccupied1;          // Reference to real MapTile on which building is set
    public List<Unit> unitsWorkers;            // List of Units that are working on this shaft
    public int capacity;
    public int type;
    public int level;
    public float upgradeTimer;

    public Unit _workerRef;                    // Reference for existing Unit object - for algorithm calculations
    public GameObject dispenser;               // Position of helper game object (for Unit FSM transitions)
    public bool isMenuOpened;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (name == "CS1")
            {
                SaveMineShaftData();

                GameHendler.Instance.mineShaftSavingData = this.mineShaftSavingData;

                switch(type)
                {
                    case 1:
                    ResourceManager.Instance.crystalShaftList.Remove(GetComponent<CrystalShaft>());
                    break;

                    case 2:
                    ResourceManager.Instance.ironShaftList.Remove(GetComponent<IronShaft>());
                    break;

                    case 3:
                    ResourceManager.Instance.gelShaftList.Remove(GetComponent<GelShaft>());
                    break;
                }

                Destroy(gameObject);
            }
        }
    }

    public void SaveMineShaftData()
    {
        mineShaftSavingData = new MineShaftSavingData();

        mineShaftSavingData.name = this.name;
        mineShaftSavingData.ID = this.ID;
        mineShaftSavingData.isShieldOn = this.isShieldOn;
        mineShaftSavingData.shieldPoints = this.shieldPoints;
        mineShaftSavingData.healthPoints = this.healthPoints;
        mineShaftSavingData.maxCurrentHealthPoints = this.maxCurrentHealthPoints;
        mineShaftSavingData.maxCurrentShieldPoints = this.maxCurrentShieldPoints;
        mineShaftSavingData.positionTileName = this._tileOccupied.name;
        mineShaftSavingData.rotation = rotation;
        mineShaftSavingData.shieldGeneratorInfluencers = this.shieldGeneratorInfluencers;


        mineShaftSavingData._shaftWorkersIDs = new int[unitsWorkers.Count];

        for (int i = 0; i < unitsWorkers.Count; i++)
        {
            mineShaftSavingData._shaftWorkersIDs[i] = unitsWorkers[i].ID;
        }



        mineShaftSavingData.rotation = rotation;

        mineShaftSavingData.type = type;
        mineShaftSavingData.capacity = capacity;
        mineShaftSavingData.level = level;



        mineShaftSavingData._tileOccupiedName = _tileOccupied.name;
        if(_tileOccupied1 != null)
        {
            mineShaftSavingData._tileOccupied1Name = _tileOccupied1.name;
        }

        mineShaftSavingData.upgradeTimer = upgradeTimer;
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

        Debug.Log("Unit is successfully added to work progress!"); 

        OnUnitManipulated();
    }
 
    public void RemoveWorkerViaSlider()
    {
        _workerRef = unitsWorkers[(unitsWorkers.Count)-1];

        RemoveUnit(_workerRef);

        ResourceManager.Instance.avaliableUnits.Add(_workerRef);
        
        _workerRef = null;
        
        Debug.Log("Removed Unit from WorkPlace!");

        OnUnitManipulated();
    }

    public void RemoveUnit(Unit unit)
    {
        unit.WorkPlace = null;     // Set workplace - null
        unitsWorkers.Remove(unit); // Remove from workers list

        OnUnitManipulated();
    }






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
            Debug.LogError("ERROR! - Invalid shaft level!!!!!");
        }
    }

    public void StartUpgrade()
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

        // Reloads unit manage sliders in menu (specific shafts tab or 3 sliders tab)
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);

        // Reload BUILDINGS_MANAGE_MENU menu sliders and SHAFT_MENU sliders
        OnDamageTaken(this);

        // Reloads SHAFT_MENU visuals
        OnUpgraded(this);
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

        // Reloads unit manage sliders in menu (specific shafts tab or 3 sliders tab)
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);

        // Reload BUILDINGS_MANAGE_MENU menu sliders and SHAFT_MENU sliders
        OnDamageTaken(this);

        // Reloads SHAFT_MENU visuals
        OnUpgraded(this);
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

        OnDamageTaken(this);
    }





    public Transform GetUnitDestination()
    {
        return dispenser.transform;
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(this);
    }

    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShaftMenu");
    }






    public virtual void ConstructBuilding(Model model)
    {
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

        level = 1;
        capacity = 3;
        rotation = model.rotation;
        HelperObjectInit();


        OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUpgraded += MineShaftStaticData.shaftMenuReference.UpdateUIAfterUpgrade;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;

        switch (type)
        {
            case 1:
            _tileOccupied = model.BTileZero;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            
            OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveCrystalScrollItem;
            OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
            OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadCrystalSlider;
            break;

            case 2:
            _tileOccupied = model.BTileZero;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
                
            OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveIronScrollItem;
            OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadIronSlider;
            OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
            break;

            case 3:
            _tileOccupied = model.BTileZero;
            _tileOccupied1 = model.BTileOne;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            
            OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveGelScrollItem;
            OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadGelSlider;
            OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
            break;
        }


        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public virtual void DestroyBuilding()
    {
        if (isMenuOpened)
        {
            MineShaftStaticData.shaftMenuReference.ExitMenu();
        }

        foreach (var unit in unitsWorkers)
        {
            unit.WorkPlace = null;
            ResourceManager.Instance.avaliableUnits.Add(unit);
        }

        unitsWorkers.Clear();


        switch (type)
        {
            case 1: // Crystal
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS1_crystal;
            break;

            case 2: // Iron
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS2_iron;
            break;

            case 3: // Gel
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS3_gel;
            _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
            break;
        }

        OnShaftDestroyed(this);

        OnUnitManipulated();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }






    public void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            dispenser = gameObject.transform.GetChild(0).gameObject;

            dispenser.tag = TagConstants.shaftDispenserTag;
            dispenser.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        }

        else
        {
            Debug.LogError("ERROR!       No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    public void UpdateUI()
    {
        if (isMenuOpened)
        {
            MineShaftStaticData.shaftMenuReference.UpdateUIAfterUpgrade(this);
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














   
    public void ConstructBuildingFromFile(MineShaftSavingData mineShaftSavingData)
    {
        name = mineShaftSavingData.name;

        InitGameUnitFromFile(
        mineShaftSavingData.healthPoints, 
        mineShaftSavingData.maxCurrentHealthPoints,
        mineShaftSavingData.shieldPoints,
        mineShaftSavingData.maxCurrentShieldPoints,
        mineShaftSavingData.deffencePoints,
        mineShaftSavingData.isShieldOn,
        mineShaftSavingData.shieldGeneratorInfluencers);


        InitMineShaftDataFromFile(mineShaftSavingData);

        OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;
        OnUpgraded += MineShaftStaticData.shaftMenuReference.UpdateUIAfterUpgrade; // update buttons and visuals
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;

        
        // Start timer
        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }
 
    public void InitMineShaftDataFromFile(MineShaftSavingData mineShaftSavingData)
    {
        HelperObjectInit();

        ID = mineShaftSavingData.ID;

        _tileOccupied = GameObject.Find(mineShaftSavingData._tileOccupiedName);
        if(mineShaftSavingData._tileOccupied1Name != "")
        {
            _tileOccupied1 = GameObject.Find(mineShaftSavingData._tileOccupied1Name);
        }

        _shaftWorkersIDs = mineShaftSavingData._shaftWorkersIDs;

        capacity = mineShaftSavingData.capacity;
        level = mineShaftSavingData.level;
        type = mineShaftSavingData.type;

        upgradeTimer = mineShaftSavingData.upgradeTimer;


        switch (type)
        {
            case 1:
            GetComponent<CrystalShaft>().ConstructShaftFromFile(mineShaftSavingData);
            break;

            case 2:
            GetComponent<IronShaft>().ConstructShaftFromFile(mineShaftSavingData);
            break;

            case 3:
            GetComponent<GelShaft>().ConstructShaftFromFile(mineShaftSavingData);
            break;
        }
    }

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

}