using UnityEngine;

public class GelShaft : MineShaft
{
    private static int gelShaft_counter = 0;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTile_Optional {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static Tile_Type PlacingTileType {get; private set;}         // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}         // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}         // Static field - Specific prefab for creating building

    private static GameObject gelShaftResourcePrefab;                   // Static field - specific resource Prefab (from PrefabManager)

    private GameObject tileOccupied = null;           // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;          // Reference to real MapTile on which building is set


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
        HealthPoints = 100;
        ShieldPoints = 100;
        type = 3;

        ResourceManager.Instance.gelShaftList.Add(this);

        tileOccupied = model.BTileZero; // grab reference to hex on which model is currently set
        tileOccupied1 = model.BTileOne; // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;  // make this tile unwalkable for units and buildings
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

        gelShaft_counter++;
        
        this.gameObject.name = "GS" + GelShaft.gelShaft_counter;

        base.HelperObjectInit();
        capacity = 3; 
        
        gameObject.transform.GetChild(0).transform.position = tileOccupied1.transform.position + OffsetConstants.buildingOffset;
        dispenserPosition = gameObject.transform.GetChild(0).transform.position;
    }

    // Function for displaying info
    public override void Invoke() 
    {
        base.Invoke();

        shaftMenuReference.ReloadPanel(this);
    }





    public override void Upgrade()
    {
        base.Upgrade();

        if (GameHendler.Instance.isMenuGelTabOpened)
        {
            GameHendler.Instance.unitManageMenuReference.ReloadGelTab();
        }
    }





    public override void DestroyShaft()
    {
        base.DestroyShaft();

        ResourceManager.Instance.gelShaftList.Remove(this);

        if (isMenuOpened)
        {
            // Close Menu panel if it is opened
            shaftMenuReference.ExitMenu();
        }

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (GameHendler.Instance.isMenuGelTabOpened)
        {
            GameHendler.Instance.unitManageMenuReference.ReloadGelTab();
        }



        
        /////////////////////////////////////////////////////////////////////////////////////////////////


        // Reload Unit Manage Menu - Sliders because shaft destrys - it means that capacity bacome lower
        ReloadMenuSlider(); // Here becasue shaft destroys



        /////////////////////////////////////////////////////////////////////////////////////////////////






        
        Destroy(gameObject);
    }

}
