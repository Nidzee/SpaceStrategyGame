using UnityEngine;

public class PowerPlantStaticData
{
    public static PowerPlantMenu powerPlantMenuReference;
    public static int powerPlant_counter = 0; // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}  // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}  // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}  // Static field - Specific prefab for creating building

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.powerPlantPrefab;
    }
}
