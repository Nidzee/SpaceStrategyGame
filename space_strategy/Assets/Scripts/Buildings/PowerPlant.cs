using UnityEngine;

public class PowerPlant :  AliveGameUnit, IBuilding
{
    private static PowerPlantMenu powerPlantMenuReference;
    private static int powerPlant_counter = 0; // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}  // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}  // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}  // Static field - Specific prefab for creating building

    private GameObject tileOccupied = null;   // Reference to real MapTile on which building is set

    public bool isMenuOpened = false;



    // Reloads sliders if Turret Menu is opened
    public override void TakeDamage(float DamagePoints)
    {
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            powerPlantMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadPowerPlantHP_SPAfterDamage(this);
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.powerPlantPrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;

        powerPlant_counter++;
        this.gameObject.name = "PP" + PowerPlant.powerPlant_counter;
        ResourceManager.Instance.powerPlantsList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        
        ResourceManager.Instance.CreatePPandAddElectricityWholeCount();
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("PowerPlantMenu");
        
        if (!powerPlantMenuReference) // executes once
        {
            powerPlantMenuReference = GameObject.Find("PowerPlantMenu").GetComponent<PowerPlantMenu>();
        }

        powerPlantMenuReference.ReloadPanel(this);
    }



    public void DestroyPP()
    {
        ResourceManager.Instance.powerPlantsList.Remove(this);

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            powerPlantMenuReference.ExitMenu();
        }

        // No need for reloading unit manage menu
        ReloadBuildingsManageMenuInfo();

        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyPPandRemoveElectricityWholeCount();
    }


    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo_PowerPlant(this);
    }

}
