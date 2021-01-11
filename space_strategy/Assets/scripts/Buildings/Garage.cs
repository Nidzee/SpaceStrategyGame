using UnityEngine;

public class Garage :  AliveGameUnit, IBuilding
{
    public static Tile_Type PlacingTileType = Tile_Type.FreeTile;
    public static BuildingType buildingType = BuildingType.DoubleTileBuilding;
    public static GameObject buildingSprite = GameHendler.Instance.garage;

    public GameObject TileOccupied = null; // Tile on which building is set
    public GameObject TileOccupied1 = null; // Tile on which building is set


    public void Creation(Model model)
    {
        TileOccupied = model.BTileZero;
        TileOccupied1 = model.BTileOne;
        buildingSprite = model.modelSprite;

        buildingSprite.transform.position = model.BTileZero.transform.position + new Vector3 (0,0,-0.1f);
        buildingSprite.transform.rotation = Quaternion.Euler(0f, 0f, (model.rotation*60));
    }

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
