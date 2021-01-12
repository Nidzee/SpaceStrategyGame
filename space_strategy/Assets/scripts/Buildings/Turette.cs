using UnityEngine;

public class Turette : AliveGameUnit, IBuilding
{
    public static Tile_Type PlacingTileType = Tile_Type.FreeTile;
    public static BuildingType buildingType = BuildingType.SingleTileBuilding;
    public static GameObject buildingSprite = BuildingManager.Instance.turetteSprite;

    public GameObject TileOccupied = null; // Tile on which building is set

    public void Creation(Model model)
    {
        TileOccupied = model.BTileZero;
        buildingSprite = model.modelSprite;
        buildingSprite.transform.position = model.BTileZero.transform.position;

        //Aditional fields like ammo and so om
    }

    public void Invoke()
    {
        //GameObject gameObject = GameObject.Find("Square");
    }
}
