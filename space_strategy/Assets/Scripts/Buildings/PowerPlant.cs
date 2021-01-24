using UnityEngine;

public class PowerPlant :  AliveGameUnit, IBuilding
{
    [SerializeField] private RectTransform powerPlantPanelReference; // Reference to UI panel
    
    public static int powerPlant_counter = 0; // For understanding which building number is this
    public static Tile_Type placingTileType;  // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;  // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;  // Static field - Specific prefab for creating building

    private GameObject tileOccupied = null;   // Reference to real MapTile on which building is set


    public static void InitStaticFields()     // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.powerPlantPrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        powerPlant_counter++;
        
        this.gameObject.name = "PowerPlant" + PowerPlant.powerPlant_counter;
        
        ResourceManager.Instance.IncreaseElectricityCount();
    }

    public void Invoke() // Function for displaying info
    {
        Debug.Log("Selected PowerPlant - go menu now");
    }

    public void Destruction()
    {
        ResourceManager.Instance.DecreaseElectricityCount();
    }
}
