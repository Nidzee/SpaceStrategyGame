using UnityEngine;

public class GSStaticData
{
    public static int gelShaft_counter;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTile_Optional {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static Tile_Type PlacingTileType {get; private set;}         // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}         // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}         // Static field - Specific prefab for creating building

    public static GameObject gelShaftResourcePrefab;                   // Static field - specific resource Prefab (from PrefabManager)


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        gelShaft_counter = 0;

        PlacingTileType = Tile_Type.RS3_gel;
        PlacingTile_Optional = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.gelShaftPrefab;
        
        gelShaftResourcePrefab = PrefabManager.Instance.gelResourcePrefab;
    }

}
