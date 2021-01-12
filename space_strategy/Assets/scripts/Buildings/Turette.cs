using UnityEngine;

public class Turette : AliveGameUnit, IBuilding
{
    public static Tile_Type PlacingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingSprite;

    public GameObject myOwnTuretteSprite = null;
    public GameObject TileOccupied = null; // Tile on which building is set

    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingSprite = BuildingManager.Instance.turetteSprite;
    }

    public void Creation(Model model)
    {
        TileOccupied = model.BTileZero;

        TileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        myOwnTuretteSprite = model.modelSprite;
        
        GameObject go = GameObject.Instantiate (myOwnTuretteSprite, 
                        model.BTileZero.transform.position + new Vector3 (0,0,-0.1f), 
                        Quaternion.Euler(0f, 0f, (model.rotation*60)), this.transform);

        go.tag = "Building";

        //Aditional fields like ammo and so om
    }

    public void Invoke()
    {
        
    }
}
