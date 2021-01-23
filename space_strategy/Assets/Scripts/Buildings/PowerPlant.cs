using UnityEngine;

public class PowerPlant :  AliveGameUnit, IBuilding
{
    public GameObject powerPlantPanelReference;
    
    public static int powerPlant_counter = 0;
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    
    private GameObject tileOccupied = null;

    public static void InitStaticFields() // Untouchable
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.powerPlantPrefab;
    }

    public void Creation(Model model)     // Untouchable
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        powerPlant_counter++;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "PowerPlant" + PowerPlant.powerPlant_counter;
    }

    public void Invoke() // Function for displaying info
    {
        Debug.Log("Selected PowerPlant - go menu now");
    }
}
