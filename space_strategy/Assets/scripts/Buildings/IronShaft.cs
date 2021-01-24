using UnityEngine;

public class IronShaft : MineShaft
{
    [SerializeField] private RectTransform shaftPanelReference; // Reference to UI panel

    public static int ironShaft_counter = 0;          // For understanding which building number is this
    public static GameObject ironShaftResourcePrefab; // Static field - specific resource Prefab (from PrefabManager)
    public static Tile_Type placingTileType;          // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;          // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;          // Static field - Specific prefab for creating building

    public GameObject tileOccupied = null;            // Reference to real MapTile on which building is set


    public static void InitStaticFields()  // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.RS2_iron;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.ironShaftPrefab;
        ironShaftResourcePrefab = PrefabManager.Instance.ironResourcePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        ironShaft_counter++;
    
        this.gameObject.name = "IronShaft" + IronShaft.ironShaft_counter;
    }


    public override void Invoke() 
    {
        Debug.Log("Selected IronShaft - go menu now");
        //UIPannelManager.Instance.ResetPanels((int)InitPannelIndex.shaftPanel);
    }
}
