using UnityEngine;

public class TurretLaser : Turette
{
    [SerializeField] private RectTransform turretPanelReference; // Reference to UI panel

    public static int turetteLaser_counter = 0; // For understanding which building number is this
    public static Tile_Type placingTileType;    // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;    // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;    // Static field - Specific prefab for creating building

    public GameObject tileOccupied = null;      // Reference to real MapTile on which building is set


    private Ray ray;



    public static void InitStaticFields()       // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.turetteLaserPrefab;
    }

    public void Creation(Model model)
    {
        //RadiusRangeCreation();

        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteLaser_counter++;

        this.gameObject.name = "TurretLaser" + TurretLaser.turetteLaser_counter;
    }

    public override void Invoke()
    {
        Debug.Log("Selected TurretLaser - go menu now");
    }



    public override void Attack()
    {
        ray = new Ray(transform.position, target.transform.position);
        Gizmos.DrawRay(ray);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }

}