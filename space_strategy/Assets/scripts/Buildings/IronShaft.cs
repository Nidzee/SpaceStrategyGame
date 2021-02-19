using UnityEngine;

public class IronShaft : MineShaft
{
    private static int ironShaft_counter = 0;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}          // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}          // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}          // Static field - Specific prefab for creating building

    public static GameObject ironShaftResourcePrefab;                    // Static field - specific resource Prefab (from PrefabManager)

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
        HealthPoints = 100;
        ShieldPoints = 100;
        type = 2;
        capacity = 3;
        level = 1;


        ironShaft_counter++;
        this.gameObject.name = "IS" + IronShaft.ironShaft_counter;
        ResourceManager.Instance.ironShaftList.Add(this);

        tileOccupied = model.BTileZero;                                    // grab reference to hex on which model is currently set
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings


        base.HelperObjectInit();

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

        ResourceManager.Instance.ironShaftList.Remove(this);

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS2_iron;

        ReloadUnitManageMenuInfo();
        ReloadBuildingsManageMenuInfo();
        
        Destroy(gameObject);
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
