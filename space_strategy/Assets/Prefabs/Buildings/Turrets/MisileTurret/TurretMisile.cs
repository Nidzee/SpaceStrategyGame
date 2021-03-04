using UnityEngine;
using UnityEngine.UI;

public class TurretMisile : Turette
{
    public static int turetteMisile_counter = 0;                   // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}    // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}    // Static field - Specific prefab for creating building

    public static GameObject misilePrefab;                         // Static field - misile prefab
    
    public bool isFired = false;
    public float coolDownTimer = 1f;

    public ParticleSystem _misileLaunchParticles;


    private static int _crystalNeedForBuilding;
    private static int _ironNeedForBuilding;
    private static int _gelNeedForBuilding;

    private static int _crystalNeedForExpand_ToLvl2;
    private static int _ironNeedForForExpand_ToLvl2;
    private static int _gelNeedForForExpand_ToLvl2;

    private static int _crystalNeedForExpand_ToLvl3;
    private static int _ironNeedForForExpand_ToLvl3;
    private static int _gelNeedForForExpand_ToLvl3;

    private static int _maxHealth_Lvl1; 
    private static int _maxHealth_Lvl2; 
    private static int _maxHealth_Lvl3;

    private static int _maxShiled_Lvl1; 
    private static int _maxShiled_Lvl2; 
    private static int _maxShiled_Lvl3;

    private static int _defensePoints_Lvl1; 
    private static int _defensePoints_Lvl2; 
    private static int _defensePoints_Lvl3;

    private static int _baseUpgradeStep;


    public static string GetResourcesNeedToBuildAsText()
    {
        return _crystalNeedForBuilding.ToString() + " " + _ironNeedForBuilding.ToString() +" "+_gelNeedForBuilding.ToString();
    }

    public static void GetResourcesNeedToBuild(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        crystalNeed = _crystalNeedForBuilding;
        ironNeed = _ironNeedForBuilding;
        gelNeed = _gelNeedForBuilding;
    }

    public static void GetResourcesNeedToExpand(out int crystalNeed, out int ironNeed, out int gelNeed, TurretMisile mt)
    {
        if (mt.level == 1)
        {
            crystalNeed = _crystalNeedForExpand_ToLvl2;
            ironNeed = _ironNeedForForExpand_ToLvl2;
            gelNeed = _gelNeedForForExpand_ToLvl2;
        }
        else
        {
            crystalNeed = _crystalNeedForExpand_ToLvl3;
            ironNeed = _ironNeedForForExpand_ToLvl3;
            gelNeed = _gelNeedForForExpand_ToLvl3;
        }
    }


    public static void InitCost_ToLvl2()
    {
        turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl2.ToString() + " " + _ironNeedForForExpand_ToLvl2.ToString() +" "+_gelNeedForForExpand_ToLvl2.ToString();
    }

    public static void InitCost_ToLvl3()
    {
        turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl3.ToString() + " " + _ironNeedForForExpand_ToLvl3.ToString() +" "+_gelNeedForForExpand_ToLvl3.ToString();
    }


    public static void UpgradeStatisticsAfterBaseUpgrade()
    {
        _maxHealth_Lvl1 += _baseUpgradeStep;
        _maxHealth_Lvl2 += _baseUpgradeStep;
        _maxHealth_Lvl3 += _baseUpgradeStep;

        _maxShiled_Lvl1 += _baseUpgradeStep;
        _maxShiled_Lvl2 += _baseUpgradeStep;
        _maxShiled_Lvl3 += _baseUpgradeStep;
    }

    public void InitStatsAfterBaseUpgrade()
    {
        switch (level)
        {
            case 1:
            healthPoints = ((_maxHealth_Lvl1 + _baseUpgradeStep) * healthPoints) / _maxHealth_Lvl1;
            maxCurrentHealthPoints = (_maxHealth_Lvl1 + _baseUpgradeStep);

            shieldPoints = ((_maxShiled_Lvl1 + _baseUpgradeStep) * shieldPoints) / _maxShiled_Lvl1;
            maxCurrentShieldPoints = (_maxShiled_Lvl1 + _baseUpgradeStep);

            deffencePoints = _defensePoints_Lvl1; // not changing at all
            break;

            case 2:
            healthPoints = ((_maxHealth_Lvl2 + _baseUpgradeStep) * healthPoints) / _maxHealth_Lvl2;
            maxCurrentHealthPoints = (_maxHealth_Lvl2 + _baseUpgradeStep);

            shieldPoints = ((_maxShiled_Lvl2 + _baseUpgradeStep) * shieldPoints) / _maxShiled_Lvl2;
            maxCurrentShieldPoints = (_maxShiled_Lvl2 + _baseUpgradeStep);

            deffencePoints = _defensePoints_Lvl2; // not changing at all
            break;

            case 3:
            healthPoints = ((_maxHealth_Lvl3 + _baseUpgradeStep) * healthPoints) / _maxHealth_Lvl3;
            maxCurrentHealthPoints = (_maxHealth_Lvl3 + _baseUpgradeStep);

            shieldPoints = ((_maxShiled_Lvl3 + _baseUpgradeStep) * shieldPoints) / _maxShiled_Lvl3;
            maxCurrentShieldPoints = (_maxShiled_Lvl3 + _baseUpgradeStep);

            deffencePoints = _defensePoints_Lvl3; // not changing at all
            break;
        }
        

        // reload everything here
        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadMisileTurretHPSP(this);
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadMisileTurretHPSP(this);
    }


    public void InitStaticsLevel_1()
    {
        level = 1;

        healthPoints = _maxHealth_Lvl1;
        maxCurrentHealthPoints = _maxHealth_Lvl1;

        shieldPoints = _maxShiled_Lvl1;
        maxCurrentShieldPoints = _maxShiled_Lvl1;

        deffencePoints = _defensePoints_Lvl1;
    }

    public void InitStaticsLevel_2()
    {
        level = 2; 

        healthPoints = (_maxHealth_Lvl2 * healthPoints) / _maxHealth_Lvl1;
        maxCurrentHealthPoints = _maxHealth_Lvl2;

        shieldPoints = (_maxShiled_Lvl2 * shieldPoints) / _maxShiled_Lvl1;
        maxCurrentShieldPoints = _maxShiled_Lvl2;

        deffencePoints = _defensePoints_Lvl2;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadMisileTurretHPSP(this);
    }

    public void InitStaticsLevel_3()
    {
        level = 3;

        healthPoints = (_maxHealth_Lvl3 * healthPoints) / _maxHealth_Lvl2;
        maxCurrentHealthPoints = _maxHealth_Lvl3;

        shieldPoints = (_maxShiled_Lvl3 * shieldPoints) / _maxShiled_Lvl2;
        maxCurrentShieldPoints = _maxShiled_Lvl3;

        deffencePoints = _defensePoints_Lvl3;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadMisileTurretHPSP(this);
    }


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.singleturetteMisilePrefab;
        
        misilePrefab = PrefabManager.Instance.misilePrefab;

        _crystalNeedForBuilding = 60;
        _ironNeedForBuilding = 60;
        _gelNeedForBuilding = 60;

        _crystalNeedForExpand_ToLvl2 = 30;
        _ironNeedForForExpand_ToLvl2 = 30;
        _gelNeedForForExpand_ToLvl2 = 30;

        _crystalNeedForExpand_ToLvl3 = 40;
        _ironNeedForForExpand_ToLvl3 = 40;
        _gelNeedForForExpand_ToLvl3 = 40;

        _maxHealth_Lvl1 = 100; 
        _maxHealth_Lvl2 = 120; 
        _maxHealth_Lvl3 = 130;

        _maxShiled_Lvl1 = 100; 
        _maxShiled_Lvl2 = 120; 
        _maxShiled_Lvl3 = 130;

        _defensePoints_Lvl1 = 7; 
        _defensePoints_Lvl2 = 8; 
        _defensePoints_Lvl3 = 9;

        _baseUpgradeStep = 30;
    }


    public override void DestroyTurret()
    {
        base.DestroyTurret();

        ResourceManager.Instance.misileTurretsList.Remove(this);


        ReloadBuildingsManageMenuInfo();

        Destroy(gameObject);
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___TurretMisile(this);
    }
}