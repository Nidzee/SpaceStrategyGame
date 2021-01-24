using UnityEngine;

public class GelShaft : MineShaft
{
    [SerializeField] private RectTransform shaftPanelReference; // Reference to UI panel

    public static int gelShaft_counter = 0;          // For understanding which building number is this
    public static GameObject gelShaftResourcePrefab; // Static field - specific resource Prefab (from PrefabManager)
    public static Tile_Type placingTile_Optional;    // Static field - Tile type on whic building need to be placed
    public static Tile_Type placingTileType;         // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;         // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;         // Static field - Specific prefab for creating building
    
    public GameObject tileOccupied = null;           // Reference to real MapTile on which building is set
    public GameObject tileOccupied1 = null;          // Reference to real MapTile on which building is set


    public static void InitStaticFields()            // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.RS3_gel;
        placingTile_Optional = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = PrefabManager.Instance.gelShaftPrefab;
        gelShaftResourcePrefab = PrefabManager.Instance.gelResourcePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied1 = model.BTileOne; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;  // make this tile unwalkable for units and buildings
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        gelShaft_counter++;
        
        this.gameObject.name = "GelShaft" + GelShaft.gelShaft_counter;
    }


    public override void Invoke() 
    {
        Debug.Log("Selected GelShaft - go menu now");
        //UIPannelManager.Instance.ResetPanels((int)InitPannelIndex.shaftPanel);
    }
}
