using UnityEngine;

public class Antenne :  AliveGameUnit, IBuilding
{
    public GameObject powerPlantPanelReference;
    
    public static int antenne_counter = 0;
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    
    private GameObject tileOccupied = null;
    private GameObject tileOccupied1 = null;


    public static void InitStaticFields() // Untouchable
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = PrefabManager.Instance.antennePrefab;
    }

    public void Creation(Model model)     // Untouchable Adds : Tag, Layer, SortingLayer, Name
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        antenne_counter++;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
        this.gameObject.name = "Antenne" + Antenne.antenne_counter;
    }

    public void Invoke() // Function for displaying info
    {
        Debug.Log("Selected Antenne - go menu now");
    }
}
