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





    private static int _crystalNeedForBuilding = 0;
    private static int _ironNeedForBuilding = 0;
    private static int _gelNeedForBuilding = 0;

    private static int _crystalNeedForExpand_ToLvl2 = 0;
    private static int _ironNeedForForExpand_ToLvl2 = 0;
    private static int _gelNeedForForExpand_ToLvl2 = 0;

    private static int _crystalNeedForExpand_ToLvl3 = 100;
    private static int _ironNeedForForExpand_ToLvl3 = 100;
    private static int _gelNeedForForExpand_ToLvl3 = 100;


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
        _crystalNeedForExpand_ToLvl2 = 5;
        _ironNeedForForExpand_ToLvl2 = 5;
        _gelNeedForForExpand_ToLvl2 = 5;

        turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl2.ToString() + " " + _ironNeedForForExpand_ToLvl2.ToString() +" "+_gelNeedForForExpand_ToLvl2.ToString();
    }

    public static void InitCost_ToLvl3()
    {
        _crystalNeedForExpand_ToLvl3 = 10;
        _ironNeedForForExpand_ToLvl3 = 10;
        _gelNeedForForExpand_ToLvl3 = 10;

        turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl3.ToString() + " " + _ironNeedForForExpand_ToLvl3.ToString() +" "+_gelNeedForForExpand_ToLvl3.ToString();
    }




    private static int baseUpgradeStep = 30;

    public static void UpgradeStatisticsAfterBaseUpgrade()
    {
        maxHealth_Lvl1 += baseUpgradeStep;
        maxHealth_Lvl2 += baseUpgradeStep;
        maxHealth_Lvl3 += baseUpgradeStep;

        maxShiled_Lvl1 += baseUpgradeStep;
        maxShiled_Lvl2 += baseUpgradeStep;
        maxShiled_Lvl3 += baseUpgradeStep;
    }

    public void InitStatisticsAfterBaseUpgrade()
    {
        switch (level)
        {
            case 1:
            healthPoints = ((maxHealth_Lvl1 + baseUpgradeStep) * healthPoints) / maxHealth_Lvl1;
            maxCurrentHealthPoints = (maxHealth_Lvl1 + baseUpgradeStep);

            shieldPoints = ((maxShiled_Lvl1 + baseUpgradeStep) * shieldPoints) / maxShiled_Lvl1;
            maxCurrentShieldPoints = (maxShiled_Lvl1 + baseUpgradeStep);

            deffencePoints = deffencePoints_Lvl1; // not changing at all
            break;

            case 2:
            healthPoints = ((maxHealth_Lvl2 + baseUpgradeStep) * healthPoints) / maxHealth_Lvl2;
            maxCurrentHealthPoints = (maxHealth_Lvl2 + baseUpgradeStep);

            shieldPoints = ((maxShiled_Lvl2 + baseUpgradeStep) * shieldPoints) / maxShiled_Lvl2;
            maxCurrentShieldPoints = (maxShiled_Lvl2 + baseUpgradeStep);

            deffencePoints = deffencePoints_Lvl2; // not changing at all
            break;

            case 3:
            healthPoints = ((maxHealth_Lvl3 + baseUpgradeStep) * healthPoints) / maxHealth_Lvl3;
            maxCurrentHealthPoints = (maxHealth_Lvl3 + baseUpgradeStep);

            shieldPoints = ((maxShiled_Lvl3 + baseUpgradeStep) * shieldPoints) / maxShiled_Lvl3;
            maxCurrentShieldPoints = (maxShiled_Lvl3 + baseUpgradeStep);

            deffencePoints = deffencePoints_Lvl3; // not changing at all
            break;
        }
        

        // reload everything here
        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadMisileTurretHPSP_Misile(this);
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
        GameViewMenu.Instance.ReloadMisileTurretHPSP_Misile(this);
    }

    private static int maxHealth_Lvl1 = 100; 
    private static int maxHealth_Lvl2 = 120; 
    private static int maxHealth_Lvl3 = 130;

    private static int maxShiled_Lvl1 = 100; 
    private static int maxShiled_Lvl2 = 120; 
    private static int maxShiled_Lvl3 = 130;

    private static int deffencePoints_Lvl1 = 7; 
    private static int deffencePoints_Lvl2 = 8; 
    private static int deffencePoints_Lvl3 = 9;



    public void InitStaticsLevel_1()
    {
        level = 1;

        healthPoints = maxHealth_Lvl1;
        maxCurrentHealthPoints = maxHealth_Lvl1;

        shieldPoints = maxShiled_Lvl1;
        maxCurrentShieldPoints = maxShiled_Lvl1;

        deffencePoints = deffencePoints_Lvl1;
    }

    public void InitStaticsLevel_2()
    {
        level = 2; 

        healthPoints = (maxHealth_Lvl2 * healthPoints) / maxHealth_Lvl1;
        maxCurrentHealthPoints = maxHealth_Lvl2;

        shieldPoints = (maxShiled_Lvl2 * shieldPoints) / maxShiled_Lvl1;
        maxCurrentShieldPoints = maxShiled_Lvl2;

        deffencePoints = deffencePoints_Lvl2;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadMisileTurretHPSP_Misile(this);
    }

    public void InitStaticsLevel_3()
    {
        level = 3;

        healthPoints = (maxHealth_Lvl3 * healthPoints) / maxHealth_Lvl2;
        maxCurrentHealthPoints = maxHealth_Lvl3;

        shieldPoints = (maxShiled_Lvl3 * shieldPoints) / maxShiled_Lvl2;
        maxCurrentShieldPoints = maxShiled_Lvl3;

        deffencePoints = deffencePoints_Lvl3;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadMisileTurretHPSP_Misile(this);
    }
























    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.singleturetteMisilePrefab;
        
        misilePrefab = PrefabManager.Instance.misilePrefab;
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