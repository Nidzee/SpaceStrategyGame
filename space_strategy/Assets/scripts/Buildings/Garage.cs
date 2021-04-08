using UnityEngine;

public class Garage : AliveGameUnit, IBuilding
{
    // public GameUnit gameUnit;
    public GarageData garageData;
    public GarageSavingData garageSavingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void UnitManipulated();
    public event UnitManipulated OnUnitManipulated = delegate{};

    public delegate void GarageDestroy(AliveGameUnit gameUnit);
    public event GarageDestroy OnGarageDestroyed = delegate{};





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (name == "G0")
            {
                garageSavingData = new GarageSavingData();

                garageSavingData.name = this.name;
                garageSavingData.ID = garageData.ID;

                garageSavingData.isShieldOn = this.isShieldOn;
                garageSavingData.shieldGeneratorInfluencers = this.shieldGeneratorInfluencers;
                garageSavingData.shieldPoints = this.shieldPoints;
                garageSavingData.healthPoints = this.healthPoints;
                garageSavingData.maxCurrentHealthPoints = this.maxCurrentHealthPoints;
                garageSavingData.maxCurrentShieldPoints = this.maxCurrentShieldPoints;
                garageSavingData.deffencePoints = this.deffencePoints;

                garageSavingData.positionTileName = garageData._tileOccupied.name;
                garageSavingData.rotation = garageData.roatation;


                garageSavingData._garageMembersIDs = new int[garageData._garageMembers.Count];
                for (int i = 0; i < garageData._garageMembers.Count; i++)
                {
                    garageSavingData._garageMembersIDs[i] = garageData._garageMembers[i].unitData.ID;
                }



                garageSavingData._queue = garageData._queue;
                garageSavingData._clicks = garageData._clicks;
                garageSavingData._timerForCreatingUnit = garageData._timerForCreatingUnit;
                garageSavingData._numberOfUnitsToCome = garageData._numberOfUnitsToCome;


                garageSavingData._tileOccupied1Name = garageData._tileOccupied1.name;
                garageSavingData._tileOccupiedName = garageData._tileOccupied.name;

                GameHendler.Instance.garageData = this.garageSavingData;








                ResourceManager.Instance.garagesList.Remove(this);

                Destroy(gameObject);








                // DestroyBuilding();



                // DestroyBuilding();
                
                // BuildingMapInfo temp = GetComponent<BuildingMapInfo>();

                // foreach (var mapPoint in temp.mapPoints)
                // {
                //     Debug.Log(mapPoint.name);
                // }
            }
        }
    }


    public void StartUnitCreation()
    {
        garageData.StartUnitCreation();
    }

    public void CreateUnit()
    {
        Unit unit = GameObject.Instantiate(UnitStaticData.unitPrefab, transform.position, Quaternion.identity).GetComponent<Unit>();
        unit.CreateInGarage(this);

        OnUnitManipulated();
    }

    public void RemoveUnit(Unit deadUnit)
    {
        garageData.RemoveUnit(deadUnit);
        OnUnitManipulated();
    }

    public void AddHomelessUnit(Unit newUnit)
    {
        garageData.AddHomelessUnit(newUnit);
        OnUnitManipulated();        
    }

    public void AddCreatedByButtonUnit(Unit newUnit)
    {
        garageData.AddCreatedByButtonUnit(newUnit);
        OnUnitManipulated();
    }

    

    public Transform GetUnitDestination()
    {
        return garageData.GetUnitDestination();
    }

    public void InitStatsAfterBaseUpgrade()
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
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


    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("GarageMenu");
        
        GarageStaticData.garageMenuReference.ReloadPanel(this);
    }

    public void ConstructBuilding(Model model)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
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

        garageData = new GarageData(this);
        
        garageData.ConstructBuilding(model);
        name = "G" + GarageStaticData.garage_counter;
        GarageStaticData.garage_counter++;



        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = model.BTileZero.transform;
        info.mapPoints[1] = model.BTileOne.transform;




        OnDamageTaken += GarageStaticData.garageMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += GarageStaticData.garageMenuReference.ReloadUnitManage;
        // OnUnitManipulated += ResourceManager.Instance.SetHomelessUnitOnDeadUnitPlace(newHome);
        // OnUnitManipulated += garageData.AddHomelessUnitAfterBuildingConstruction; // It only executes once above
        OnUnitManipulated(); // Because we can add Homeless units here

        OnGarageDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        ResourceManager.Instance.garagesList.Add(this);
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void DestroyBuilding()
    {
        garageData.DestroyBuilding();

        OnGarageDestroyed(this);
        OnUnitManipulated();

        Destroy(gameObject);
        ResourceManager.Instance.garagesList.Remove(this);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
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


        garageData = new GarageData(this);
        garageData.InitGarageDataFromFile(garageSavedInfo);



        if (garageSavedInfo._timerForCreatingUnit != 0)
        {
            StartCoroutine(garageData.CreateUnitLogic());
        }



        OnDamageTaken += GarageStaticData.garageMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        OnUnitManipulated += GarageStaticData.garageMenuReference.ReloadUnitManage;
        OnGarageDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        ResourceManager.Instance.garagesList.Add(this);
    }

    public void CreateRelations()
    {
        for (int i = 0; i < garageData._garageMembersIDs.Length; i++)
        {
            for (int j = 0; j < ResourceManager.Instance.unitsList.Count; j++)
            {
                if (garageData._garageMembersIDs[i] == ResourceManager.Instance.unitsList[j].unitData.ID)
                {
                    Debug.Log("Add unit to garage!" + this.name + "   " + ResourceManager.Instance.unitsList[j].name);

                    ResourceManager.Instance.homelessUnits.Remove(ResourceManager.Instance.unitsList[j]);

                    garageData._garageMembers.Add(ResourceManager.Instance.unitsList[j]);
                    ResourceManager.Instance.unitsList[j].unitData.home = this;
                    j = 0;
                    break;
                }
            }
        }
    }
}