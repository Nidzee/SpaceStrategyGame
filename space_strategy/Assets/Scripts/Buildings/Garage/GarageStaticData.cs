using UnityEngine;

public class GarageStaticData
{
    public static GarageMenu garageMenuReference;                    // Reference to UI panel (same field for all Garages)
    public static int garage_counter = 0;                            // For understanding which building number is this    
    public const float _timerStep = 0.5f;
    public const int _garageCapacity = 5;                            // Constant field - All garages have same capacity
    
    public static Tile_Type PlacingTileType {get; private set;}      // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}      // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}      // Static field - Specific prefab for creating building

    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.garagePrefab;
    }
}
