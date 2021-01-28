using UnityEngine;

public class TurretMisile : Turette
{
    private static int turetteMisile_counter = 0;                   // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}     // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}     // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}     // Static field - Specific prefab for creating building

    private static GameObject misilePrefab;                         // Static field - misile prefab

    private GameObject tileOccupied = null;                          // Reference to real MapTile on which building is set

    private bool isFired = false;

    private float coolDownTimer = 1f;




    // private void Awake() // For prefab test
    // {
    //     isCreated = true;
    // }


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.turetteMisilePrefab;
        
        misilePrefab = PrefabManager.Instance.misilePrefab;
    }


    // Function for creating building
    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteMisile_counter++;

        this.gameObject.name = "TurretMisile" + TurretMisile.turetteMisile_counter;

        HelperObjectInit();
    }


    // Function for diaplying info
    public override void Invoke()
    {
        base.Invoke();

        turretMenuReference.ReloadPanel(this);
    }


    // Attack pattern
    public override void Attack()
    {
        if (!isFired)
        {
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