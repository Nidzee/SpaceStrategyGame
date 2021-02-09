using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBulletSingle : Turette
{
    public static int turetteBullet_counter = 0;                   // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}     // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}     // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}     // Static field - Specific prefab for creating building

    private static GameObject bulletPrefab;                         // Static field bullet prefab


    private bool isFired = false;

    private static float coolDownTimer = 0.3f;
    private static float bulletSpeed = 1500;




    public GameObject barrel;




    private void Awake() // For prefab test
    {
        isCreated = true;

        InitBarrels();
    }


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.turetteBulletPrefab;
        
        bulletPrefab = PrefabManager.Instance.bulletPrefab;
    }

    private void InitBarrels()
    {
        if (gameObject.transform.childCount != 0)
        {
            barrel = gameObject.transform.GetChild(1).gameObject;
            barrel.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            barrel.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.buildingLayer;
        }
    }


    // Function for creating building
    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteBullet_counter++;

        this.gameObject.name = "TurretBullet" + TurretBullet.turetteBullet_counter;

        HelperObjectInit();

        InitBarrels();
    }


    // Function for displaying info
    public override void Invoke()
    {
        base.Invoke();

        turretMenuReference.ReloadPanel(this);
    }


    // Attack patter
    public override void Attack()
    {
        if (!isFired)
        {
            GameObject temp = GameObject.Instantiate(bulletPrefab, barrel.transform.position, targetRotation);
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
