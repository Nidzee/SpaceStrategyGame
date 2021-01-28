using UnityEngine;

public class IronShaft : MineShaft
{
    private static int ironShaft_counter = 0;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}          // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}          // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}          // Static field - Specific prefab for creating building

    private static GameObject ironShaftResourcePrefab;                   // Static field - specific resource Prefab (from PrefabManager)

    private GameObject tileOccupied = null;            // Reference to real MapTile on which building is set


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.RS2_iron;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.ironShaftPrefab;
        
        ironShaftResourcePrefab = PrefabManager.Instance.ironResourcePrefab;
    }


    // Function for creating building
    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        ironShaft_counter++;
    
        this.gameObject.name = "IronShaft" + IronShaft.ironShaft_counter;

        base.HelperObjectInit();
    }


    // Function for displaying info
    public override void Invoke() 
    {
        base.Invoke();

        shaftMenuReference.ReloadPanel(this);
    }
}
