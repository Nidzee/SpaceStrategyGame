using UnityEngine;

public class Turette : IBuilding
{
    public static Tile_Type PlacingTileType = Tile_Type.FreeTile;
    public static BuildingType buildingType = BuildingType.SingleTileBuilding;
    public static Sprite sprite;

    public GameObject TileOccupied = null; // Tile on which building is set

    public void Invoke()
    {

    }
}
