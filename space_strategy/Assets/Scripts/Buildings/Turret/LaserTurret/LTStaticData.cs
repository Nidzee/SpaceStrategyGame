using UnityEngine;

public class LTStaticData
{
    public static int turetteLaser_counter = 0;                   // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}    // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}    // Static field - Specific prefab for creating building
    
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.singleTuretteLaserPrefab;

        turetteLaser_counter = 0;
    }
}
