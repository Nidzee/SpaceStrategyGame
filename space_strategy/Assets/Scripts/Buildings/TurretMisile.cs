using UnityEngine;

public class TurretMisile : Turette
{
    [SerializeField] private RectTransform turretPanelReference; // Reference to UI panel

    public static int turetteMisile_counter = 0; // For understanding which building number is this
    public static Tile_Type placingTileType;     // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;     // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;     // Static field - Specific prefab for creating building

    public GameObject tileOccupied = null;       // Reference to real MapTile on which building is set

    public static void InitStaticFields()        // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.turetteMisilePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteMisile_counter++;

        this.gameObject.name = "TurretMisile" + TurretMisile.turetteMisile_counter;
    }

    public override void Invoke()
    {
        
    }
}