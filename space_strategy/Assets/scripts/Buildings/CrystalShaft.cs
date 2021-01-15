using UnityEngine;
using System.Collections.Generic;

public class CrystalShaft : MineShaft
{
    public static GameObject crystalShaftResourcePrefab;
    public static int crystalShaft_counter = 0;
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    
    //private int capacity = 5;
    public GameObject tileOccupied = null; // Tile on which building is set


    public static void InitStaticFields()
    {
        placingTileType = Tile_Type.RS1_crystal;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = BuildingManager.Instance.garagePrefab; // crystalShaftPrefab
    }

    public void Creation(Model model)
    {
        crystalShaft_counter++;

        tileOccupied = model.BTileZero;

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        this.gameObject.tag = "Building";
        this.gameObject.name = "CrystalShaft" + CrystalShaft.crystalShaft_counter;
    }

    public override void AddWorker()
    {
        base.AddWorker();
        base.workerRef.resourcePrefab = crystalShaftResourcePrefab;
        base.workerRef = null;
    }

    public override void Invoke() 
    {

    }
}
