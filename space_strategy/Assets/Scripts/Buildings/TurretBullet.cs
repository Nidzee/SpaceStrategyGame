﻿using UnityEngine;

public class TurretBullet : Turette
{
    private static int turetteBullet_counter = 0;                   // For understanding which building number is this
    
    public static Tile_Type PlacingTileType {get; private set;}     // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}     // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}     // Static field - Specific prefab for creating building

    private static GameObject bulletPrefab;                         // Static field bullet prefab

    private GameObject tileOccupied = null;                         // Reference to real MapTile on which building is set

    private bool isFired = false;

    private static float coolDownTimer = 0.3f;
    private static float bulletSpeed = 900f;



    // private void Awake() // For prefab test
    // {
    //     isCreated = true;
    // }


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.SingleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.turetteBulletPrefab;
        
        bulletPrefab = PrefabManager.Instance.bulletPrefab;
    }


    // Function for creating building
    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        turetteBullet_counter++;

        this.gameObject.name = "TurretBullet" + TurretBullet.turetteBullet_counter;

        HelperObjectInit();
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