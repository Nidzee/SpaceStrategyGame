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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (name == "G1")
            {
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
        UpgradeStats(StatsManager._maxHealth_Garage 
        + StatsManager._baseUpgradeStep_Garage, StatsManager._maxShiled_Garage 
        + StatsManager._baseUpgradeStep_Garage, StatsManager._maxDeffensePoints_Garage);
        
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
        CreateGameUnit(StatsManager._maxHealth_Garage, StatsManager._maxShiled_Garage, StatsManager._maxDeffensePoints_Garage);
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













    public void SaveData()
    {
        // garageSavingData
    }

    public void ConstructBuildingFromFile(GarageSavingData garageSavedInfo)
    {
        GameObject BTileZero = GameObject.Find(garageSavedInfo.positionTileName);

        Instantiate(GarageStaticData.BuildingPrefab, 
        BTileZero.transform.position + OffsetConstants.buildingOffset, 
        Quaternion.Euler(0f, 0f, (garageSavedInfo.rotation*60)));


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
                    garageData._garageMembers.Add(ResourceManager.Instance.unitsList[j]);
                    ResourceManager.Instance.unitsList[j].Home = this;
                    break;
                }
            }
        }
    }
}