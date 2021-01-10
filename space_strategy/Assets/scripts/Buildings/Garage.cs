using UnityEngine;

public class Garage :  AliveGameUnit, IBuilding
{
    public static Tile_Type PlacingTileType = Tile_Type.FreeTile;
    public static BuildingType buildingType = BuildingType.DoubleTileBuilding;
    public static Sprite sprite;

    public GameObject TileOccupied = null; // Tile on which building is set
    public GameObject TileOccupied1 = null; // Tile on which building is set

    public void CreateUnit() // Create new Unit in Garage
    {

    }

    public void RemoveUnit() // Remove Unit if it is dead
    {

    }

    public void DestroyGarage() // Destroy building and set all Units to HOMELESS status
    {

    }

    public void AddHomelessUnit() // After creating Garage check for Homeless Units and add them to this Garage
    {

    }

    public void Invoke()
    {
        //this.PlacingTile = Tile_Type.FreeTile;
    }
}
