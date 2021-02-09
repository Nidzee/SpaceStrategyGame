using UnityEngine;

public class TurretMisile : Turette
{
    public static int turetteMisile_counter = 0;                   // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}    // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}    // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}    // Static field - Specific prefab for creating building

    public static GameObject misilePrefab;                         // Static field - misile prefab
    
    public GameObject tileOccupied = null;                         // Reference to real MapTile on which building is set
    public bool isFired = false;
    public float coolDownTimer = 1f;



    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.turetteMisilePrefab;
        
        misilePrefab = PrefabManager.Instance.misilePrefab;
    }

    // Function for creating building
    public virtual void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteMisile_counter++;

        this.gameObject.name = "TM" + TurretMisile.turetteMisile_counter;

        ResourceManager.Instance.misileTurretsList.Add(this);

        HelperObjectInit();
    }

}