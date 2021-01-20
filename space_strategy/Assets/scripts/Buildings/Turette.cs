using UnityEngine;

public class Turette : AliveGameUnit, IBuilding
{
    public static int turret_counter = 0; // for easy Debug
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;

    public GameObject tileOccupied = null; // Tile on which building is set



    public static void InitStaticFields()
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.turettePrefab;
    }

    public void Creation(Model model)
    {
        turret_counter++;

        tileOccupied = model.BTileZero;

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "Turret" + Turette.turret_counter;

        //Aditional fields like ammo and so om
    }

    public void Invoke()
    {
        
    }
}
