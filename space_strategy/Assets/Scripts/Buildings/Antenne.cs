using UnityEngine;

public class Antenne :  AliveGameUnit, IBuilding
{
    [SerializeField] private RectTransform powerPlantPanelReference; // Reference to UI panel
    
    public static int antenne_counter = 0;   // For understanding which building number is this
    public static Tile_Type placingTileType; // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType; // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab; // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    
    private GameObject tileOccupied = null;  // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null; // Reference to real MapTile on which building is set


    public static void InitStaticFields()    // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = PrefabManager.Instance.antennePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        antenne_counter++;
        
        this.gameObject.name = "Antenne" + Antenne.antenne_counter;
    }


    public void Invoke() // Function for displaying info
    {
        Debug.Log("Selected Antenne - go menu now");
    }
}
