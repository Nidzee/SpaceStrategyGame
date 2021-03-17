using UnityEngine;

public class PowerPlant : MonoBehaviour, IAliveGameUnit, IBuilding
{
    public GameUnit gameUnit;
    public PowerPlantData powerPlantData;
    public PowerPlantSavingData powerPlantSavingData;    

    public delegate void DamageTaken(GameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void PowerPlantDestroy(GameUnit gameUnit);
    public event PowerPlantDestroy OnPowerPlantDestroyed = delegate{};


    public void InitStatsAfterBaseUpgrade()
    {
        gameUnit.UpgradeStats(StatsManager._maxHealth_PowerPlant 
        + StatsManager._baseUpgradeStep_PowerPlant, StatsManager._maxShiled_PowerPlant 
        + StatsManager._baseUpgradeStep_PowerPlant, StatsManager._maxDeffencePoints_PowerPlant);

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
        powerPlantData.Invoke();
    }

    public void ConstructBuilding(Model model)
    {
        gameUnit = new GameUnit(StatsManager._maxHealth_PowerPlant, StatsManager._maxShiled_PowerPlant, StatsManager._maxDeffencePoints_PowerPlant);        
        powerPlantData = new PowerPlantData(this);

        PowerPlantStaticData.powerPlant_counter++;
        gameObject.name = "PP" + PowerPlantStaticData.powerPlant_counter;

        
        powerPlantData.ConstructBuilding(model);
        gameUnit.name = this.name;


        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnPowerPlantDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;


        ResourceManager.Instance.powerPlantsList.Add(this);
        ResourceManager.Instance.CreatePPandAddElectricityWholeCount();
    }

    public void DestroyBuilding()
    {
        ResourceManager.Instance.powerPlantsList.Remove(this);

        powerPlantData.DestroyBuilding();
        OnPowerPlantDestroyed(gameUnit);
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyPPandRemoveElectricityWholeCount();
        AstarPath.active.Scan();
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