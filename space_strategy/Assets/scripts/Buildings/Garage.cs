using UnityEngine;

public class Garage :  AliveGameUnit, IBuilding
{
    public static Tile_Type PlacingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingSprite;

    public GameObject myOwnGarageSprite = null;
    public GameObject TileOccupied = null; // Tile on which building is set
    public GameObject TileOccupied1 = null; // Tile on which building is set


    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingSprite = BuildingManager.Instance.garageSprite;
    }

    public void Creation(Model model)
    {
        TileOccupied = model.BTileZero;
        TileOccupied1 = model.BTileOne;

        TileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        TileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        myOwnGarageSprite = model.modelSprite;
        
        GameObject.Instantiate (myOwnGarageSprite, 
            model.BTileZero.transform.position + new Vector3 (0,0,-0.1f), 
            Quaternion.Euler(0f, 0f, (model.rotation*60)), this.transform);
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
