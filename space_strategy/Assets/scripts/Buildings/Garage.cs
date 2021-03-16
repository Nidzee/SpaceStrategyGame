using UnityEngine;

public class Garage :  MonoBehaviour, IAliveGameUnit, IBuilding
{
    public GameUnit gameUnit;
    public GarageData garageData;
    public GarageSavingData garageSavingData;

    public delegate void DamageTaken(GameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void UnitManipulated();
    public event UnitManipulated OnUnitManipulated = delegate{};

    public delegate void GarageDestroy(GameUnit gameUnit);
    public event GarageDestroy OnGarageDestroyed = delegate{};





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (name == "G1")
            {
                // TakeDamage(10);
                DestroyBuilding();
            }
        }
    }


    public void StartUnitCreation()
    {
        garageData.StartUnitCreation(this);
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
        garageData.AddHomelessUnit(newUnit, this);
        OnUnitManipulated();        
    }

    public void AddCreatedByButtonUnit(Unit newUnit)
    {
        garageData.AddCreatedByButtonUnit(newUnit, this);
        OnUnitManipulated();
    }

    

    public Transform GetUnitDestination()
    {
        return garageData.GetUnitDestination();
    }

    public void InitStatsAfterBaseUpgrade()
    {
        gameUnit.UpgradeStats(StatsManager._maxHealth_Garage 
        + StatsManager._baseUpgradeStep_Garage, StatsManager._maxShiled_Garage 
        + StatsManager._baseUpgradeStep_Garage, StatsManager._maxDeffensePoints_Garage);
        
        OnDamageTaken(gameUnit);
    }

    public void TakeDamage(int damagePoints)
    {
        if (!gameUnit.TakeDamage(damagePoints))
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(gameUnit);
    }


    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("GarageMenu");
        
        GarageStaticData.garageMenuReference.ReloadPanel(this);
    }

    public void ConstructBuilding(Model model)
    {
        gameUnit = new GameUnit(StatsManager._maxHealth_Garage, StatsManager._maxShiled_Garage, StatsManager._maxDeffensePoints_Garage);
        garageData = new GarageData();
        
        garageData.ConstructBuilding(model);
        gameObject.name = "G" + GarageStaticData.garage_counter;
        gameUnit.name = this.name;

        garageData.AddHomelessUnitAfterBuildingConstruction(this);
        // HelperObjectInit();
        garageData.HelperObjectInit(this);

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
        garageData.DestroyBuilding(this);

        OnGarageDestroyed(gameUnit);
        OnUnitManipulated();

        Destroy(gameObject);
        ResourceManager.Instance.garagesList.Remove(this);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        AstarPath.active.Scan();
    }

}