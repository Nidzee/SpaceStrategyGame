using UnityEngine;

public class MTStaticData
{
    public static int turetteMisile_counter = 0;                   // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}    // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}    // Static field - Specific prefab for creating building

    public static GameObject misilePrefab;                         // Static field - misile prefab

    public static ParticleSystem _misileLaunchParticles;

    
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.singleturetteMisilePrefab;
        
        misilePrefab = PrefabManager.Instance.misilePrefab;

        _misileLaunchParticles = PrefabManager.Instance.misileLaunchParticles;

        turetteMisile_counter = 0;
    }
}
