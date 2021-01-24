using UnityEngine;

public class CrystalShaft : MineShaft
{
    [SerializeField] private RectTransform shaftPanelReference; // Reference to UI panel

    public static int crystalShaft_counter = 0;          // For understanding which building number is this
    public static GameObject crystalShaftResourcePrefab; // Static field - specific resource Prefab (from PrefabManager)
    public static Tile_Type placingTileType;             // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;             // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;             // Static field - Specific prefab for creating building
    
    public GameObject tileOccupied = null;               // Reference to real MapTile on which building is set


    public static void InitStaticFields()                // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.RS1_crystal;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.crystalShaftPrefab;
        crystalShaftResourcePrefab = PrefabManager.Instance.crystalResourcePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        crystalShaft_counter++;
        
        this.gameObject.name = "CrystalShaft" + CrystalShaft.crystalShaft_counter;
    }


    public override void Invoke() 
    {
        Debug.Log("Selected CrystalShaft - go menu now");
        //shaftPanelReference.GetComponent<ShaftPanel>().shaftRef = this;
        // UIPannelManager.Instance.ResetPanels((int)InitPannelIndex.shaftPanel);
        // shaftPanelReference.GetComponent<ShaftPanel>().ReloadPanel(this);
    }
}
