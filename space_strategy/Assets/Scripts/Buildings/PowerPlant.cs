using UnityEngine;

public class PowerPlant : AliveGameUnit, IBuilding
{
    // public GameUnit gameUnit;
    public PowerPlantData powerPlantData;
    public PowerPlantSavingData powerPlantSavingData;    

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void PowerPlantDestroy(AliveGameUnit gameUnit);
    public event PowerPlantDestroy OnPowerPlantDestroyed = delegate{};


    public void InitStatsAfterBaseUpgrade()
    {
        UpgradeStats(StatsManager._maxHealth_PowerPlant 
        + StatsManager._baseUpgradeStep_PowerPlant, StatsManager._maxShiled_PowerPlant 
        + StatsManager._baseUpgradeStep_PowerPlant, StatsManager._maxDeffencePoints_PowerPlant);

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
        powerPlantData.Invoke();
    }

    public void ConstructBuilding(Model model)
    {
        CreateGameUnit(StatsManager._maxHealth_PowerPlant, StatsManager._maxShiled_PowerPlant, StatsManager._maxDeffencePoints_PowerPlant);        
        powerPlantData = new PowerPlantData(this);

        PowerPlantStaticData.powerPlant_counter++;
        gameObject.name = "PP" + PowerPlantStaticData.powerPlant_counter;



        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;




        
        powerPlantData.ConstructBuilding(model);
        // myName = this.name;


        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnPowerPlantDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        ResourceManager.Instance.powerPlantsList.Add(this);
        ResourceManager.Instance.CreatePPandAddElectricityWholeCount();
    }

    public void DestroyBuilding()
    {
        ResourceManager.Instance.powerPlantsList.Remove(this);

        powerPlantData.DestroyBuilding();
        OnPowerPlantDestroyed(this);
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyPowerPlantAndRescanMap();
    }










    // private void UpdateUI()
    // {
    //     // reload everything here
    //     if (powerPlantData.isMenuOpened)
    //     {
    //         PowerPlantStaticData.powerPlantMenuReference.ReloadSlidersHP_SP();
    //     }

    //     // Reloads HP_SP sliders if buildings manage menu opened
    //     GameViewMenu.Instance.ReloadPowerPlantHPSP(this);
    // }

    // private void ReloadBuildingsManageMenuInfo()
    // {
    //     GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterPowerPlantDestroying(this);
    // }

}