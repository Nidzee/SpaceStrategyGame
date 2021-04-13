using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Garage : AliveGameUnit, IBuilding
{
    // Events
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public delegate void GarageDestroy(AliveGameUnit gameUnit);
    public delegate void UnitManipulated();
    public delegate void GarageDestroyForUnitManipulations();

    public event DamageTaken OnDamageTaken = delegate{};
    public event UnitManipulated OnUnitManipulated = delegate{};
    public event GarageDestroy OnGarageDestroyed = delegate{};
    public event GarageDestroyForUnitManipulations OnGarageDestroyedForUnitManip = delegate{};


    // Particular garage variables for saving
    public GarageSavingData garageSavingData = null;
    private int[] _garageMembersIDs = null; 
    private GameObject _tileOccupied = null;
    private GameObject _tileOccupied1 = null; 
    private float _timerForCreatingUnit = 0f;
    private int _queue = 0;                              
    public int _clicksOnCreateUnitButton = 0;
    private int _roatation = 0;


    // Cummon for all garages
    private Unit _unitRef = null;
    public List<Unit> _garageMembers = new List<Unit>();
    public bool isMenuOpened = false;
    public GameObject angar;            // Init in inspector
    public GameObject relaxPoint1;      // Init in inspector
    public GameObject relaxPoint2;      // Init in inspector
    public GameObject relaxPoint3;      // Init in inspector
    public GameObject relaxPoint4;      // Init in inspector
    public GameObject relaxPointCENTER; // Init in inspector


    // UI
    public GameObject canvas;           // Init in inspector
    public Slider healthBar;            // Init in inspector
    public Slider shieldhBar;           // Init in inspector


    public void SaveData()
    {
        garageSavingData = new GarageSavingData();

        garageSavingData.healthPoints = healthPoints;
        garageSavingData.shieldPoints = shieldPoints;
        garageSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        garageSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        garageSavingData.deffencePoints = deffencePoints;
        garageSavingData.isShieldOn = isShieldOn;
        garageSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        garageSavingData.name = name;
        garageSavingData.rotation = _roatation;


        garageSavingData._garageMembersIDs = new int[_garageMembers.Count];
        for (int i = 0; i < _garageMembers.Count; i++)
        {
            garageSavingData._garageMembersIDs[i] = _garageMembers[i].ID;
        }


        garageSavingData._queue = _queue;
        garageSavingData._clicksOnCreateUnitButton = _clicksOnCreateUnitButton;
        garageSavingData._timerForCreatingUnit = _timerForCreatingUnit;

        garageSavingData._tileOccupied1_name = _tileOccupied1.name;
        garageSavingData._tileOccupied_name = _tileOccupied.name;
        
        GameHendler.Instance.garagesSaved.Add(garageSavingData);
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

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("GarageMenu");
        
        GarageStaticData.garageMenuReference.ReloadPanel(this);
    }

    private void OnTriggerEnter2D(Collider2D collider) // Damage
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

    public void InitStatsAfterBaseUpgrade()
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Garage_Base_Lvl_1;
            shield = StatsManager._maxShiled_Garage_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_Garage_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Garage_Base_Lvl_2;
            shield = StatsManager._maxShiled_Garage_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_Garage_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Garage_Base_Lvl_3;
            shield = StatsManager._maxShiled_Garage_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_Garage_Base_Lvl_3;
            break;
        }

        UpgradeStats(health, shield, defense);

        OnDamageTaken(this); // KOSTUL'
    }


    #region Building constructing and destroying

    public void ConstructBuilding(Model model)
    {
        #region Data initializing
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Garage_Base_Lvl_1;
            shield = StatsManager._maxShiled_Garage_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_Garage_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Garage_Base_Lvl_2;
            shield = StatsManager._maxShiled_Garage_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_Garage_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Garage_Base_Lvl_3;
            shield = StatsManager._maxShiled_Garage_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_Garage_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);


        name = "G" + GarageStaticData.garage_counter;
        GarageStaticData.garage_counter++;

        _tileOccupied = model.BTileZero;
        _tileOccupied1 = model.BTileOne;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        _roatation = model.rotation;

        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);

        AddHomelessUnitAfterGarageConstruction();

        #endregion

        GarageEventsAndBuildingMapInfoInitialization();


        ResourceManager.Instance.garagesList.Add(this);
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void ConstructBuildingFromFile(GarageSavingData garageSavedInfo)
    {
        #region Data initializing

        InitGameUnitFromFile(
        garageSavedInfo.healthPoints, 
        garageSavedInfo.maxCurrentHealthPoints,
        garageSavedInfo.shieldPoints,
        garageSavedInfo.maxCurrentShieldPoints,
        garageSavedInfo.deffencePoints,
        garageSavedInfo.isShieldOn,
        garageSavedInfo.shieldGeneratorInfluencers);


        name = garageSavedInfo.name;

        _tileOccupied = GameObject.Find(garageSavedInfo._tileOccupied_name);
        _tileOccupied1 = GameObject.Find(garageSavedInfo._tileOccupied1_name);
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        _timerForCreatingUnit = garageSavedInfo._timerForCreatingUnit;
        _queue = garageSavedInfo._queue;                          
        _clicksOnCreateUnitButton = garageSavedInfo._clicksOnCreateUnitButton;
        _roatation = garageSavedInfo.rotation;


        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(false);

        _garageMembersIDs = garageSavedInfo._garageMembersIDs;

        #endregion
        
        GarageEventsAndBuildingMapInfoInitialization();


        ResourceManager.Instance.garagesList.Add(this);
        if (garageSavedInfo._timerForCreatingUnit != 0)
        {
            StartCoroutine(UnitCreationProcess());
        }
    }

    private void GarageEventsAndBuildingMapInfoInitialization()
    {
        OnDamageTaken += GarageStaticData.garageMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += GarageStaticData.garageMenuReference.ReloadUnitManage;
        OnGarageDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        OnGarageDestroyedForUnitManip += GameViewMenu.Instance.ReloadMainUnitCount;


        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = _tileOccupied.transform;
        info.mapPoints[1] = _tileOccupied1.transform;
    }

    public void DestroyBuilding()
    {
        DestroyBuildingData();

        OnGarageDestroyed(this);
        OnGarageDestroyedForUnitManip();

        Destroy(gameObject);
        ResourceManager.Instance.garagesList.Remove(this);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    public void DestroyBuildingData()
    {
        // Looping through all garage members
        foreach (var unit in _garageMembers)
        {
            // If we found new home at run-time - dont delete work
            bool temp = ResourceManager.Instance.SetNewHomeForUnitFromDestroyedGarage(unit, this);

            if (temp)
            {
                // Home is changed
                // WorkPlace is still the same
            }
            else
            {
                // Delete home
                // If he was working - delete work
                
                unit.Home = null;

                if (unit.WorkPlace)
                {
                    unit.WorkPlace.RemoveUnit(unit);
                    ResourceManager.Instance.homelessUnits.Add(unit);
                }
                else
                {
                    ResourceManager.Instance.avaliableUnits.Remove(unit);
                    ResourceManager.Instance.homelessUnits.Add(unit);
                }
            }
        }
        
        _garageMembers.Clear();

        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            GarageStaticData.garageMenuReference.ExitMenu();
        }
    }

    #endregion

    #region Unit Manipulating

        public void AddUnitsFromFileToGarageFromFile()
        {
            Unit unit = null;

            // Looping through all garage members ID's
            for (int i = 0; i < _garageMembersIDs.Length; i++)
            {
                // Looping through all units
                for (int j = 0; j < ResourceManager.Instance.unitsList.Count; j++)
                {
                    // Set unit reference
                    unit = ResourceManager.Instance.unitsList[j];

                    // If gagarge have unit with same ID as particular unit from file
                    if (_garageMembersIDs[i] == unit.ID)
                    {
                        ResourceManager.Instance.homelessUnits.Remove(unit);
                        _garageMembers.Add(unit);
                        unit.Home = this;

                        j = 0;
                        break;
                    }
                }
            }
        }

        public void AddHomelessUnitAfterGarageConstruction()////////////////////////////////////////////////////////////////////////////
        {
            // Loop through all homeless units
            if (ResourceManager.Instance.homelessUnits.Count != 0)
            {
                for (int i = 0; i < 5; i++) // 5 is garage maximum capacity
                {
                    // Sets homeless unit from list
                    _unitRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];
                    ResourceManager.Instance.homelessUnits.Remove(_unitRef); // Decrements list of homeless units
                    ResourceManager.Instance.avaliableUnits.Add(_unitRef);


                    // Add homeless unit to garage
                    AddHomelessUnit(_unitRef); // Can be modifyed because 5 times call event "OnUnitManipulated()"


                    // Check there are still homeless units (decrements above!)
                    if (ResourceManager.Instance.homelessUnits.Count == 0)
                    {
                        return;
                    }
                }
            }
        }

        public void ManipulationsAfterCreateUnitButtonPress() // Correct
        {
            _queue++;                    // Increments queue
            _clicksOnCreateUnitButton++; // Clicks increment

            if (_queue == 1)
            {
                StartCoroutine(UnitCreationProcess());
            }
        }

        public IEnumerator UnitCreationProcess() // Correct
        {
            // Endless loop
            while (true)
            {
                // While tormer is not over
                while (_timerForCreatingUnit < 1)
                {
                    // Increments loading 
                    _timerForCreatingUnit += GarageStaticData._timerStep * Time.deltaTime;

                    // Reload loading bar in menu
                    if (isMenuOpened)
                    {
                        GarageStaticData.garageMenuReference.loadingBar.fillAmount = _timerForCreatingUnit;
                    }

                    yield return null;
                }

                // Reset loading bar
                _timerForCreatingUnit = 0f;
                
                // Create Unit after loading is over
                CreateUnit();

                // Decrements queue and check if it was last one in queue
                _queue--;
                if (_queue == 0)
                {
                    yield break;
                }
            }
        }

        public void CreateUnit() // Creation of unit
        {
            Unit unit = GameObject.Instantiate(UnitStaticData.unitPrefab, transform.position, Quaternion.identity).GetComponent<Unit>();
            
            unit.CreateInGarage(this);

            OnUnitManipulated();
        }

        public void RemoveDeadUnitFromGarage(Unit deadUnit) // Removes dead unit from garage
        {
            deadUnit.Home = null;
            _garageMembers.Remove(deadUnit);
            _clicksOnCreateUnitButton--;

            OnUnitManipulated();
        }

        public void AddHomelessUnit(Unit newUnit) // Adds homeless unit after garage member dies
        {
            newUnit.Home = this;
            _garageMembers.Add(newUnit);
            _clicksOnCreateUnitButton++;

            OnUnitManipulated();        
        }

        public void AddCreatedByButtonUnit(Unit newUnit) // Only data manipulations 
        {
            newUnit.Home = this;
            _garageMembers.Add(newUnit);
        }

        public Transform GetUnitDestination()
        {
            return angar.transform;
        }

    #endregion
}