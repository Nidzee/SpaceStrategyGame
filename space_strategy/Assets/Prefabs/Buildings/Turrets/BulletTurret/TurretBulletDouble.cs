using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBulletDouble : Turette
{
    public static int turetteBullet_counter = 0;                   // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}     // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}     // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}     // Static field - Specific prefab for creating building

    private static GameObject bulletPrefab;                         // Static field bullet prefab


    private bool isFired = false;

    private static float coolDownTimer = 0.3f;
    private static float bulletSpeed = 100;




    public GameObject barrel;
    public GameObject barrel1;

    public Quaternion targetRotationForBarrel = new Quaternion();
    public Quaternion targetRotationForBarrel1 = new Quaternion();

    public bool isBarrelFacingEnemy = false;
    public bool isBarrel1FacingEnemy = false;

    private float barrelTurnSpeed = 200f;




    // private void Awake() // For prefab test
    // {
    //     isCreated = true;

    //     InitBarrels();
    // }


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

            barrel1 = gameObject.transform.GetChild(2).gameObject;
            barrel1.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            barrel1.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.buildingLayer;
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
        RotateBarrelTowardsEnemy();
        RotateBarrel1TowardsEnemy();

        if (isBarrelFacingEnemy && isBarrel1FacingEnemy) // Attack  && isBarrel1FacingEnemy
        {
            Debug.Log("Attack Laser Turret");

            if (!isFired)
            {
                GameObject temp = GameObject.Instantiate(bulletPrefab, barrel.transform.position, targetRotationForBarrel);
                temp.GetComponent<Rigidbody2D>().velocity = (transform.right * bulletSpeed);

                GameObject temp1 = GameObject.Instantiate(bulletPrefab, barrel1.transform.position, targetRotationForBarrel1);
                temp1.GetComponent<Rigidbody2D>().velocity = (transform.right * bulletSpeed);

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

    private void RotateBarrelTowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetRotationForBarrel, barrelTurnSpeed * Time.deltaTime);

            if ( barrel.transform.rotation == targetRotationForBarrel && !isBarrelFacingEnemy)
            {
                isBarrelFacingEnemy = true;
            }
        }
    }

    private void RotateBarrel1TowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel1.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel1 = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel1.transform.rotation = Quaternion.RotateTowards(barrel1.transform.rotation, targetRotationForBarrel1, barrelTurnSpeed * Time.deltaTime);

            if (barrel1.transform.rotation == targetRotationForBarrel1 && !isBarrel1FacingEnemy)
            {
                isBarrel1FacingEnemy = true;
            }
        }
    }
}