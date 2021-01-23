using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator :  AliveGameUnit, IBuilding
{
    public GameObject shieldGeneratorPanelReference;
    
    public static int shieldGenerator_counter = 0;
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    
    private GameObject tileOccupied = null;
    private GameObject tileOccupied1 = null;
    private GameObject tileOccupied2 = null;


    public static void InitStaticFields() // Untouchable
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.TripleTileBuilding;
        buildingPrefab = PrefabManager.Instance.shieldGeneratorPrefab;
    }

    public void Creation(Model model)     // Untouchable
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied2 = model.BTileTwo;

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        shieldGenerator_counter++;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "ShieldGenerator" + ShieldGenerator.shieldGenerator_counter;
    }

    public void Invoke() // Function for displaying info
    {
        Debug.Log("Selected ShieldGenerator - go menu now");
    }
}