using UnityEngine;
using System.Collections.Generic;

public class TurretLaser : Turette
{
    public static int turetteLaser_counter = 0;                    // For understanding which building number is this
    public static Tile_Type PlacingTileType {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}    // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}    // Static field - Specific prefab for creating building

    private GameObject tileOccupied = null;                        // Reference to real MapTile on which building is set










    // private void Awake() // For prefab test
    // {
    //     isCreated = true;
    // }


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.turetteLaserPrefab;
    }


    // Function for creating building
    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteLaser_counter++;

        this.gameObject.name = "TurretLaser" + TurretLaser.turetteLaser_counter;

        HelperObjectInit();
    }


    // Function for displaying info
    public override void Invoke()
    {
        base.Invoke();

        turretMenuReference.ReloadPanel(this);
    }


    // Attack pattern - TODO
    public override void Attack()
    {
        
    }
}