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








    public override void TakeDamage(float damagePoints)
    {
        HealthPoints -= damagePoints;
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