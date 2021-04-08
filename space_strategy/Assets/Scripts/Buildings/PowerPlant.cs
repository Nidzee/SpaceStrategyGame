using UnityEngine;

public class PowerPlant : AliveGameUnit, IBuilding
{
    public PowerPlantData powerPlantData;
    public PowerPlantSavingData powerPlantSavingData;    

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void PowerPlantDestroy(AliveGameUnit gameUnit);
    public event PowerPlantDestroy OnPowerPlantDestroyed = delegate{};


    public void InitStatsAfterBaseUpgrade()
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_1;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_2;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_3;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_3;
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
        powerPlantData.Invoke();
    }

    public void ConstructBuilding(Model model)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_1;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_2;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_PowerPlant_Base_Lvl_3;
            shield = StatsManager._maxShiled_PowerPlant_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_PowerPlant_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);

        powerPlantData = new PowerPlantData(this);

        PowerPlantStaticData.powerPlant_counter++;
        gameObject.name = "PP" + PowerPlantStaticData.powerPlant_counter;



        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;



        powerPlantData.ConstructBuilding(model);


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
        
        ResourceManager.Instance.DestroyPPandRemoveElectricityWholeCount();
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

}