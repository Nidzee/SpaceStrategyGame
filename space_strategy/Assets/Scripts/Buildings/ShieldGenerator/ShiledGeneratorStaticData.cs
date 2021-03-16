using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiledGeneratorStaticData
{
    public static ShiledGeneratorMenu shieldGeneratorMenuReference; // Reference to UI panel
    
    public static int shieldGenerator_counter = 0; // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}       // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}       // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}       // Static field - Specific prefab for creating building
    
    public static Vector3 startScale;
    public static Vector3 standartScale;
    public static Vector3 scaleLevel2;
    public static Vector3 scaleLevel3;

    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.TripleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.shieldGeneratorPrefab;

        startScale  = new Vector3(1,1,1);
        standartScale = new Vector3(15,15,1);
        scaleLevel2 = new Vector3(20,20,1);
        scaleLevel3 = new Vector3(25,25,1);
    }
}
