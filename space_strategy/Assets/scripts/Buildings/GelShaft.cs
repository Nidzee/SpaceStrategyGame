using UnityEngine;

public class GelShaft : MineShaft
{
    private static int gelShaft_counter = 0;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTile_Optional {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static Tile_Type PlacingTileType {get; private set;}         // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}         // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}         // Static field - Specific prefab for creating building

    public static GameObject gelShaftResourcePrefab;                   // Static field - specific resource Prefab (from PrefabManager)

    private GameObject _tileOccupied = null;           // Reference to real MapTile on which building is set
    private GameObject _tileOccupied1 = null;          // Reference to real MapTile on which building is set

    

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.RS3_gel;
        PlacingTile_Optional = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.gelShaftPrefab;
        
        gelShaftResourcePrefab = PrefabManager.Instance.gelResourcePrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {        
        InitStaticsLevel_1();
        
        type = 3;

        gelShaft_counter++;
        this.gameObject.name = "GS" + GelShaft.gelShaft_counter;
        ResourceManager.Instance.gelShaftList.Add(this);

        _tileOccupied = model.BTileZero;                                     // grab reference to hex on which model is currently set
        _tileOccupied1 = model.BTileOne;                                     // grab reference to hex on which model is currently set
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;  // make this tile unwalkable for units and buildings
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings


        // Helper object reinitialization
        base.HelperObjectInit();
        gameObject.transform.GetChild(0).transform.position = _tileOccupied1.transform.position + OffsetConstants.buildingOffset;
        dispenser.transform.position = gameObject.transform.GetChild(0).transform.position;

        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    // Function for displaying info
    public override void Invoke() 
    {
        base.Invoke();

        shaftMenuReference.ReloadPanel(this);
    }



    // Correct logic
    public override void DestroyShaft()
    {
        base.DestroyShaft();

        ResourceManager.Instance.gelShaftList.Remove(this);

        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS3_gel;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        
        ReloadUnitManageMenuInfo();
        ReloadBuildingsManageMenuInfo();
        
        Destroy(gameObject);
        AstarPath.active.Scan();
    }

       
    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterShaftDestroying(this.name, this.type);
    }

}
