using UnityEngine;

public class AntenneStaticData
{
    public static AntenneMenu antenneMenuReference;
    
    public static Tile_Type PlacingTileType {get; private set;} // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;} // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;} // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    

    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.antennePrefab;
    }
}
