using UnityEngine;
using System.Collections.Generic;

public class CrystalShaft : MineShaft
{
    public static int crystalShaft_counter = 0; // For Debug and iterations (OPTIONAL)

    public static GameObject crystalShaftResourcePrefab; // resource prefab - got from PrefabManager
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    
    public GameObject tileOccupied = null; // Tile on which building is set - before setruction - set to TileFree!

    public static void InitStaticFields() // Do not touch!
    {
        placingTileType = Tile_Type.RS1_crystal;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.crystalShaftPrefab;
        crystalShaftResourcePrefab = PrefabManager.Instance.crystalResourcePrefab;
    }

    public void Creation(Model model)
    {
        crystalShaft_counter++;

        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        this.gameObject.tag = "Building";
        this.gameObject.name = "CrystalShaft" + CrystalShaft.crystalShaft_counter;

        // gameObject.transform.GetChild(0).tag = "ShaftRadius";
        // dispenserPosition = gameObject.transform.GetChild(0).transform.position;
    }




    public override void AddWorkerViaSlider()
    {
        base.AddWorkerViaSlider();
    }




    public override void Invoke() 
    {
        // UI logic
    }
}
