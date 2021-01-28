using UnityEngine;

public class PowerPlant :  AliveGameUnit, IBuilding
{
    private static PowerPlantMenu powerPlantMenuReference;
    private static int powerPlant_counter = 0; // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}  // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}  // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}  // Static field - Specific prefab for creating building

    private GameObject tileOccupied = null;   // Reference to real MapTile on which building is set


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.powerPlantPrefab;
    }


    // Function for creating building
    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        powerPlant_counter++;
        
        this.gameObject.name = "PowerPlant" + PowerPlant.powerPlant_counter;
        
        ResourceManager.Instance.IncreaseElectricityCount();
    }


    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("PowerPlantMenu");
        
        if (!powerPlantMenuReference) // executes once
        {
            powerPlantMenuReference = GameObject.Find("PowerPlantMenu").GetComponent<PowerPlantMenu>();
        }

        powerPlantMenuReference.ReloadPanel(this);
    }
}
