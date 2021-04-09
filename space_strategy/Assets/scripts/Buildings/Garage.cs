using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Garage : AliveGameUnit, IBuilding
{
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public delegate void UnitManipulated();
    public event UnitManipulated OnUnitManipulated = delegate{};
    public delegate void GarageDestroy(AliveGameUnit gameUnit);
    public event GarageDestroy OnGarageDestroyed = delegate{};
    public GarageSavingData garageSavingData;


    public int ID;
    public int[] _garageMembersIDs;             // Units that are living here    
    public GameObject _tileOccupied;              // Reference to real MapTile on which building is set
    public GameObject _tileOccupied1;             // Reference to real MapTile on which building is set
    public List<Unit> _garageMembers;             // Units that are living here    
    public float _timerForCreatingUnit;
    public int _queue;                              
    public int _clicks;
    public int _numberOfUnitsToCome;
    public int roatation;

    public Unit _unitRef;
    public bool _isMenuOpened;
    public GameObject angar;
    public GameObject relaxPoint1;
    public GameObject relaxPoint2;
    public GameObject relaxPoint3;
    public GameObject relaxPoint4;
    public GameObject relaxPointCENTER;





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (name == "G0")
            {
                SaveGarageData();

                GameHendler.Instance.garageData = garageSavingData;

                ResourceManager.Instance.garagesList.Remove(this);

                Destroy(gameObject);
            }
        }
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("GarageMenu");
        
        GarageStaticData.garageMenuReference.ReloadPanel(this);
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

        OnDamageTaken(this);
    }

    public void SaveGarageData()
    {
        garageSavingData = new GarageSavingData();

        garageSavingData.healthPoints = healthPoints;
        garageSavingData.shieldPoints = shieldPoints;
        garageSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
        garageSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
        garageSavingData.deffencePoints = deffencePoints;
        garageSavingData.isShieldOn = isShieldOn;
        garageSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        garageSavingData.name = this.name;
        garageSavingData.ID = ID;
        garageSavingData.rotation = roatation;


        garageSavingData._garageMembersIDs = new int[_garageMembers.Count];
        for (int i = 0; i < _garageMembers.Count; i++)
        {
            garageSavingData._garageMembersIDs[i] = _garageMembers[i].ID;
        }



        garageSavingData._queue = _queue;
        garageSavingData._clicks = _clicks;
        garageSavingData._timerForCreatingUnit = _timerForCreatingUnit;
        garageSavingData._numberOfUnitsToCome = _numberOfUnitsToCome;


        garageSavingData._tileOccupied1_name = _tileOccupied1.name;
        garageSavingData._tileOccupied_name = _tileOccupied.name;
    }











    public void ConstructBuilding(Model model)
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

        CreateGameUnit(health, shield, defense);

        
        
        name = "G" + GarageStaticData.garage_counter;
        GarageStaticData.garage_counter++;


        _tileOccupied = model.BTileZero;
        _tileOccupied1 = model.BTileOne;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        roatation = model.rotation;

        HelperObjectInit();

        AddHomelessUnitAfterBuildingConstruction();



        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = model.BTileZero.transform;
        info.mapPoints[1] = model.BTileOne.transform;




        OnDamageTaken += GarageStaticData.garageMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += GarageStaticData.garageMenuReference.ReloadUnitManage;
        OnUnitManipulated(); // Because we can add Homeless units here

        OnGarageDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        ResourceManager.Instance.garagesList.Add(this);
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void ConstructBuildingFromFile(GarageSavingData garageSavedInfo)
    {
        name = garageSavedInfo.name;

        InitGameUnitFromFile(
        garageSavedInfo.healthPoints, 
        garageSavedInfo.maxCurrentHealthPoints,
        garageSavedInfo.shieldPoints,
        garageSavedInfo.maxCurrentShieldPoints,
        garageSavedInfo.deffencePoints,
        garageSavedInfo.isShieldOn,
        garageSavedInfo.shieldGeneratorInfluencers);


        ID = garageSavedInfo.ID;
        roatation = garageSavedInfo.rotation;

        _tileOccupied = GameObject.Find(garageSavedInfo._tileOccupied_name);
        _tileOccupied1 = GameObject.Find(garageSavedInfo._tileOccupied1_name);
        
        _garageMembersIDs = garageSavedInfo._garageMembersIDs;

        _timerForCreatingUnit = garageSavedInfo._timerForCreatingUnit;
        _queue = garageSavedInfo._queue;
        _clicks = garageSavedInfo._clicks;
        _numberOfUnitsToCome = garageSavedInfo._numberOfUnitsToCome;
        
        HelperObjectInit();



        if (garageSavedInfo._timerForCreatingUnit != 0)
        {
            StartCoroutine(CreateUnitLogic());
        }



        OnDamageTaken += GarageStaticData.garageMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += GarageStaticData.garageMenuReference.ReloadUnitManage;
        OnGarageDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        ResourceManager.Instance.garagesList.Add(this);
    }

    public void DestroyBuilding()
    {
        DestroyBuildingData();

        OnGarageDestroyed(this);
        OnUnitManipulated();

        Destroy(gameObject);
        ResourceManager.Instance.garagesList.Remove(this);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    public void DestroyBuildingData()
    {
        List<MineShaft> shaftsToReloadSliders = new List<MineShaft>();

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
                    MineShaft sameWorkplace = unit.WorkPlace;
                    unit.WorkPlace.RemoveUnit(unit);
                    ResourceManager.Instance.homelessUnits.Add(unit);

                    // If garage destroys but unit which was garage member was working and its shaft menu was open
                    if (sameWorkplace.isMenuOpened)
                    {
                        MineShaftStaticData.shaftMenuReference.ReloadUnitSlider();
                    }
                    else
                    {
                        shaftsToReloadSliders.Add(sameWorkplace);
                    }
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

        if (_isMenuOpened)
        {
            GarageStaticData.garageMenuReference.ExitMenu();
        }

        // По суті - коли ми ремуваємо юніта - то викликається івент і все там собі оновлює
        // REDO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterGarageDestroying(shaftsToReloadSliders);
    }

    public void CreateRelations()
    {
        for (int i = 0; i < _garageMembersIDs.Length; i++)
        {
            for (int j = 0; j < ResourceManager.Instance.unitsList.Count; j++)
            {
                if (_garageMembersIDs[i] == ResourceManager.Instance.unitsList[j].ID)
                {
                    Debug.Log("Add unit to garage!" + this.name + "   " + ResourceManager.Instance.unitsList[j].name);

                    ResourceManager.Instance.homelessUnits.Remove(ResourceManager.Instance.unitsList[j]);

                    _garageMembers.Add(ResourceManager.Instance.unitsList[j]);
                    ResourceManager.Instance.unitsList[j].Home = this;
                    j = 0;
                    break;
                }
            }
        }
    }













    
    private void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            angar = gameObject.transform.GetChild(0).gameObject;

            angar.tag = TagConstants.garageAngarTag;
            angar.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            angar.transform.position = _tileOccupied1.transform.position;

            relaxPoint1 = angar.transform.GetChild(0).gameObject;
            relaxPoint2 = angar.transform.GetChild(1).gameObject;
            relaxPoint3 = angar.transform.GetChild(2).gameObject;
            relaxPoint4 = angar.transform.GetChild(3).gameObject;
            relaxPointCENTER = angar.transform.GetChild(4).gameObject;            
        }

        else
        {
            Debug.LogError("ERROR!      No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

#region Unit Manipulating

    public void AddHomelessUnitAfterBuildingConstruction()
    {
        if (ResourceManager.Instance.homelessUnits.Count != 0)
        {
            for (int i = 0; i < GarageStaticData._garageCapacity; i++)
            {
                _unitRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];

                AddHomelessUnit(_unitRef);

                ResourceManager.Instance.homelessUnits.Remove(_unitRef);
                ResourceManager.Instance.avaliableUnits.Add(_unitRef);

                // Check there are still homeless units (decrements above!)
                if (ResourceManager.Instance.homelessUnits.Count == 0)
                {
                    // Reload unit images because we add new unit
                    if (_isMenuOpened)
                    {
                        GarageStaticData.garageMenuReference.ReloadPanel(this);
                    }

                    // GameViewMenu.Instance.ReloadMainUnitCount();

                    return;
                }
            }
        }
    }

    public void StartUnitCreation()
    {
        _queue++;                    // Increments queue
        _numberOfUnitsToCome--;       // Decrease number of incoming homeless units
        _clicks++;                    // Clicks increment

        if (_queue == 1)
        {
            StartCoroutine(CreateUnitLogic());
        }
    }

    public IEnumerator CreateUnitLogic()
    {
        while (true)
        {
            while (_timerForCreatingUnit < 1)
            {
                _timerForCreatingUnit += GarageStaticData._timerStep * Time.deltaTime;

                if (_isMenuOpened)
                {
                    GarageStaticData.garageMenuReference.loadingBar.fillAmount = _timerForCreatingUnit;
                }

                yield return null;
            }

            _timerForCreatingUnit = 0f;
            
            CreateUnit();

            _queue--;

            if (_queue == 0)
            {
                yield break;
            }
        }
    }

    public void CreateUnit()
    {
        Unit unit = GameObject.Instantiate(UnitStaticData.unitPrefab, transform.position, Quaternion.identity).GetComponent<Unit>();
        unit.CreateInGarage(this);

        OnUnitManipulated();
    }

    public void RemoveUnit(Unit deadUnit)
    {
        deadUnit.Home = null;
        _garageMembers.Remove(deadUnit);

        _clicks--;
        _numberOfUnitsToCome++;

        OnUnitManipulated();
    }

    public void AddHomelessUnit(Unit newUnit)
    {
        newUnit.Home = this;
        _garageMembers.Add(newUnit);
        
        _clicks++;
        _numberOfUnitsToCome--;

        OnUnitManipulated();        
    }

    public void AddCreatedByButtonUnit(Unit newUnit)
    {
        newUnit.Home = this;
        _garageMembers.Add(newUnit);

        OnUnitManipulated();
    }

    public Transform GetUnitDestination()
    {
        return angar.transform;
    }

#endregion
}