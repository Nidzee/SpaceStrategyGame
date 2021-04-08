using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldGenerator : AliveGameUnit, IBuilding
{
    // public GameUnit gameUnit;
    public ShieldGeneratorData shieldGeneratorData;
    public ShieldGeneratorSavingData shieldGeneratorSavingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void Upgraded();
    public event Upgraded OnUpgraded = delegate{};

    public delegate void ShieldGeneraorDestroy(AliveGameUnit gameUnit);
    public event ShieldGeneraorDestroy OnSGDestroyed = delegate{};

    

    public void StartUpgrade()
    {
        StartCoroutine(shieldGeneratorData.UpgradeLogic());
    }

    public void EnableShield()
    {
        shieldGeneratorData.EnableShield();
    }

    public void DisableShield()
    {
        shieldGeneratorData.DisableShield();
    }


    public void InitStatsAfterBaseUpgrade()
    {
        shieldGeneratorData.InitStatsAfterBaseUpgrade();
        
        OnDamageTaken(this);
    }

    public override void UpgradeStats(int newHealth, int NewShield, int newDefense)
    {
        base.UpgradeStats(newHealth, NewShield, newDefense);
    }


    public void UpgradeToLvl2()
    {
        shieldGeneratorData.UpgradeToLvl2();

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl2_ShieldGenerator_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl2_ShieldGenerator_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl2_ShieldGenerator_Base_Lvl_3;
            break;
        }
        UpgradeStats(health, shield, defense);


        OnUpgraded();
        OnDamageTaken(this);
    }

    public void UpgradeToLvl3()
    {
        shieldGeneratorData.UpgradeToLvl3();

        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl3_ShieldGenerator_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl3_ShieldGenerator_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl3_ShieldGenerator_Base_Lvl_3;
            break;
        }
        UpgradeStats(health, shield, defense);


        OnUpgraded();
        OnDamageTaken(this);
    }


    // Reloads sliders if Turret Menu is opened
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

    // Function for creating building
    public void ConstructBuilding(Model model)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl1_ShieldGenerator_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl1_ShieldGenerator_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl1_ShieldGenerator_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);

        shieldGeneratorData = new ShieldGeneratorData(this);

        ShiledGeneratorStaticData.shieldGenerator_counter++;
        gameObject.name = "SG" + ShiledGeneratorStaticData.shieldGenerator_counter;





        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[3];
        info.mapPoints[0] = model.BTileZero.transform;
        info.mapPoints[1] = model.BTileOne.transform;
        info.mapPoints[2] = model.BTileTwo.transform;




        shieldGeneratorData.ConstructBuilding(model);


        // Add events here
        
        // OnDamageTaken += MineShaftStaticData.shaftMenuReference.ReloadSlidersHP_SP;
        // OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadShaftLevelVisuals; // update buttons and visuals
        // OnUpgraded += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;        // expands slider
        // OnUpgraded += mineShaftData.UpdateUI;
        // OnUnitManipulated += GameViewMenu.Instance.ReloadMainUnitCount;
        // OnUnitManipulated += MineShaftStaticData.shaftMenuReference.ReloadUnitSlider;
        // OnShaftDestroyed += GameViewMenu.Instance.unitManageMenuReference.RemoveCrystalScrollItem;
        // OnShaftDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;
        // OnUnitManipulated += GameViewMenu.Instance.unitManageMenuReference.ReloadCrystalSlider;
        // OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;



        // Redo into one function - UpgradeVisuals
        OnUpgraded += ShiledGeneratorStaticData.shieldGeneratorMenuReference.UpgradeVisuals;
        OnDamageTaken += ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnSGDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        // ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP(gameUnit);
        // ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadLevelManager();


        ResourceManager.Instance.shiledGeneratorsList.Add(this);
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShieldGeneratorMenu");
        
        shieldGeneratorData.Invoke();

        ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadPanel(this);
    }


    public void DestroyBuilding()
    {
        shieldGeneratorData.DestroyBuilding();


        // Call events here
        OnSGDestroyed(this);


        Destroy(gameObject);
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
}