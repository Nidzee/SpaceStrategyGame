using UnityEngine;

public class TurretLaser : Turette
{
    public static int turetteLaser_counter = 0;                    // For understanding which building number is this
    public static Tile_Type PlacingTileType {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}    // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}    // Static field - Specific prefab for creating building


    public float barrelTurnSpeed = 200f;
    public bool isLasersEnabled = false; 



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
        if (GameHendler.Instance.isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (GameHendler.Instance.isMilitaryBuildingsMenuOpened)
            {
                GameHendler.Instance.buildingsManageMenuReference.ReloadLaserTurretHPSP(this);
            }
        }
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.singleTuretteLaserPrefab;
    }


    public override void DestroyTurret()
    {
        base.DestroyTurret();

        ResourceManager.Instance.laserTurretsList.Remove(this);

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        ReloadBuildingsManageMenuInfo();

        Destroy(gameObject);
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        if (GameHendler.Instance.isBuildingsMAnageMenuOpened)
        {
            if (GameHendler.Instance.isMilitaryBuildingsMenuOpened)
            {
                // Drop some code here
                GameHendler.Instance.buildingsManageMenuReference.RemoveLaserTurret(this.name);
            }
        }
    }
}