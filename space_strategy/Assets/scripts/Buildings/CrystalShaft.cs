using UnityEngine;

public class CrystalShaft : MineShaft
{
    private static int crystalShaft_counter = 0;                            // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}             // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}             // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}             // Static field - Specific prefab for creating building

    public static GameObject crystalShaftResourcePrefab;                    // Static field - specific resource Prefab (from PrefabManager)

    private GameObject tileOccupied = null;                                 // Reference to real MapTile on which building is set



    public override void TakeDamage(float damagePoints)
    {
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads sliders if Damage taken
        if (isMenuOpened)
        {
            shaftMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        if (GameHendler.Instance.isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (GameHendler.Instance.isIndustrialBuildingsMenuOpened)
            {
                // reload hp_sp slider
                GameHendler.Instance.buildingsManageMenuReference.ReloadCrystalShaftHPSP(this);
            }
        }
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.RS1_crystal;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.crystalShaftPrefab;

        crystalShaftResourcePrefab = PrefabManager.Instance.crystalResourcePrefab; // Initializing resource prefab
    }

    // Function for creating building
    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;
        type = 1;
        capacity = 3; 
        level = 1;


        crystalShaft_counter++;
        this.gameObject.name = "CS" + CrystalShaft.crystalShaft_counter;  
        ResourceManager.Instance.crystalShaftList.Add(this);

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

        ResourceManager.Instance.crystalShaftList.Remove(this);

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS1_crystal;

        ReloadUnitManageMenuInfo();
        ReloadBuildingsManageMenuInfo();

        Destroy(gameObject);
    }

    
    private void ReloadUnitManageMenuInfo()
    {
        if (GameHendler.Instance.isUnitManageMenuOpened) // Reload everything in here
        {
            // If all Sliders menu was opened - reload - because total shaft capacity will decrease
            if (GameHendler.Instance.isMenuAllResourcesTabOpened)
            {
                GameHendler.Instance.unitManageMenuReference.ReloadCrystalSlider();  
            }

            // If crystal Tab was opened - reload whole tab - to delete dead shaft
            if (GameHendler.Instance.isMenuCrystalTabOpened)
            {
                // GameHendler.Instance.unitManageMenuReference.ReloadCrystalTab();
                GameHendler.Instance.unitManageMenuReference.RemoveCrystalScrollItem(this);
            }

            // Reload Units becasu units without workplace - became avaliable
            GameHendler.Instance.unitManageMenuReference.ReloadMainUnitCount();
        }
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        if (GameHendler.Instance.isBuildingsMAnageMenuOpened)
        {
            if (GameHendler.Instance.isIndustrialBuildingsMenuOpened)
            {
                // Drop some code here
                GameHendler.Instance.buildingsManageMenuReference.RemoveCrystalShaftFromBuildingsMenu(this.name);
            }
        }
    }
}
