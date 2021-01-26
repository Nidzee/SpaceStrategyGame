using UnityEngine;

public class TurretBullet : Turette
{
    [SerializeField] private RectTransform turretPanelReference; // Reference to UI panel

    public static int turetteBullet_counter = 0; // For understanding which building number is this
    public static Tile_Type placingTileType;     // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;     // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;     // Static field - Specific prefab for creating building

    public GameObject tileOccupied = null;       // Reference to real MapTile on which building is set


    private float coolDownTimer = 0.3f;
    private bool isFired = false;
    private static float bulletSpeed = 900f;
    private static GameObject bulletPrefab;


    public static void InitStaticFields()        // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.turetteBulletPrefab;
        bulletPrefab = PrefabManager.Instance.bulletPrefab;
    }

    public void Creation(Model model)
    {
        //RadiusRangeCreation();

        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteBullet_counter++;

        this.gameObject.name = "TurretBullet" + TurretBullet.turetteBullet_counter;
    }

    public override void Invoke()
    {
        Debug.Log("Selected TurretBullet - go menu now");
    }

    public override void Attack()
    {
        if (!isFired)
        {
            base.Attack();
            GameObject temp = GameObject.Instantiate(bulletPrefab,transform.position, targetRotation);

            temp.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);

            isFired = true;
        }
        else
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer < 0)
            {
                coolDownTimer = 0.3f;
                isFired = false;
            }
        }
    }
}
