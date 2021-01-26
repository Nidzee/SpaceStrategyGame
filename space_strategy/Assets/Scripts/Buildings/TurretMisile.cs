using UnityEngine;

public class TurretMisile : Turette
{
    [SerializeField] private RectTransform turretPanelReference; // Reference to UI panel

    public static int turetteMisile_counter = 0; // For understanding which building number is this
    public static Tile_Type placingTileType;     // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;     // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;     // Static field - Specific prefab for creating building

    public GameObject tileOccupied = null;       // Reference to real MapTile on which building is set

    private float coolDownTimer = 1f;
    private bool isFired = false;
    private static GameObject misilePrefab;

    public static void InitStaticFields()        // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.turetteMisilePrefab;
        misilePrefab = PrefabManager.Instance.misilePrefab;
    }

    public void Creation(Model model)
    {
        //RadiusRangeCreation();

        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteMisile_counter++;

        this.gameObject.name = "TurretMisile" + TurretMisile.turetteMisile_counter;
    }

    public override void Invoke()
    {
        Debug.Log("Selected TurretMisile - go menu now");
    }

    public override void Attack()
    {
        if (!isFired)
        {
            base.Attack();
            GameObject temp = GameObject.Instantiate(misilePrefab, transform.position, base.targetRotation);
            temp.GetComponent<Misile>().target = base.target;

            isFired = true;
        }
        else
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer < 0)
            {
                coolDownTimer = 1f;
                isFired = false;
            }
        }
    }
}