using UnityEngine;

public class CrystalShaft : MineShaft
{
    private static int crystalShaft_counter = 0;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}             // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}             // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}             // Static field - Specific prefab for creating building

    public static GameObject crystalShaftResourcePrefab;                    // Static field - specific resource Prefab (from PrefabManager)

    private GameObject _tileOccupied = null;                                 // Reference to real MapTile on which building is set





    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         if (name == "CS1")
    //         {
    //             Debug.Log("Damage");
    //             TakeDamage(10);
    //         }
    //     }
    // }




    






















    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.RS1_crystal;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.crystalShaftPrefab;

        // Initializing resource prefab
        crystalShaftResourcePrefab = PrefabManager.Instance.crystalResourcePrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        InitStaticsLevel_1();

        type = 1;

        crystalShaft_counter++;
        this.gameObject.name = "CS" + CrystalShaft.crystalShaft_counter;  
        ResourceManager.Instance.crystalShaftList.Add(this);

        _tileOccupied = model.BTileZero;                                    // grab reference to hex on which model is currently set
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile; // make this tile unwalkable for units and buildings

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

        ResourceManager.Instance.crystalShaftList.Remove(this);

        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS1_crystal;

        ReloadUnitManageMenuInfo();
        ReloadBuildingsManageMenuInfo();

        Destroy(gameObject);
        AstarPath.active.Scan();
    }

    
    // Because if Shaft gets destroyed - all workers become avaliable and sliders must be reloaded
    private void ReloadUnitManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftDestroying(this);
    }

    // Because if Shaft gets destroyed - it needs to dissappear from buildings scroll items
    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterShaftDestroying(this.name, this.type);
    }
}
