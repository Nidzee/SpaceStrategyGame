using UnityEngine;

public class CSStaticData
{
    public static int crystalShaft_counter;                            // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}             // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}             // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}             // Static field - Specific prefab for creating building

    public static GameObject crystalShaftResourcePrefab;                    // Static field - specific resource Prefab (from PrefabManager)
   
    public static void InitStaticFields()
    {
        crystalShaft_counter = 0;

        PlacingTileType = Tile_Type.RS1_crystal;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.crystalShaftPrefab;

        // Initializing resource prefab
        crystalShaftResourcePrefab = PrefabManager.Instance.crystalResourcePrefab;
    }
}
