using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator :  AliveGameUnit, IBuilding
{
    public GameObject shieldGeneratorPanelReference;
    
    public static int shieldGenerator_counter = 0;
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    
    private GameObject tileOccupied = null;
    private GameObject tileOccupied1 = null;
    private GameObject tileOccupied2 = null;

    [SerializeField] private GameObject shieldRangePrefab;
    private GameObject shieldGeneratorRangeRef;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateShield();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DisableShield();
        }
    }

    public static void InitStaticFields() // Untouchable
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.TripleTileBuilding;
        buildingPrefab = PrefabManager.Instance.shieldGeneratorPrefab;
    }

    public void Creation(Model model)     // Untouchable
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied2 = model.BTileTwo;

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        shieldGenerator_counter++;
        this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "ShieldGenerator" + ShieldGenerator.shieldGenerator_counter;

        //shieldRangePrefab = gameObject.transform.GetChild(0).gameObject;
    }

    public void Invoke() // Function for displaying info
    {
        Debug.Log("Selected ShieldGenerator - go menu now");
    }

    private void ActivateShield()
    {
        // 1 - Roll animation
        // 2 - Instantiate shield circle
        if (!shieldGeneratorRangeRef)
        {
            shieldGeneratorRangeRef = GameObject.Instantiate (shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.radiusLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.shieldGeneratorRangeLayer;
            
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";
        }
        else
        {
            Debug.Log("Error! Shield is already On!");
        }
    }

    private void DisableShield()
    {
        // 1 - Roll animation
        // 2 - Delete shield circle
        if (shieldGeneratorRangeRef)
        {
            Debug.Log("Deleting ShieldGeneratorRange!");
            Destroy(shieldGeneratorRangeRef);
        }
        else
        {
            Debug.Log("Error! Shield is already Off!");
        }
    }
}