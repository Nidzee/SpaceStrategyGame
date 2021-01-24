using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : Turette
{
    public static int turetteBullet_counter = 0;
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;

    public GameObject tileOccupied = null; // Tile on which building is set

    public static void InitStaticFields()
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.SingleTileBuilding;
        buildingPrefab = PrefabManager.Instance.turetteBulletPrefab;
    }

    public void Creation(Model model)
    {
        turetteBullet_counter++;

        tileOccupied = model.BTileZero;

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "TurretBullet" + TurretBullet.turetteBullet_counter;

        //Aditional fields like ammo and so om
    }

    public override void Invoke()
    {
        
    }
}
