using UnityEngine;

public class ShieldGenerator :  AliveGameUnit, IBuilding
{
    private static ShiledGeneratorMenu shieldGeneratorMenuReference; // Reference to UI panel
    private static int shieldGenerator_counter = 0; // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}       // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}       // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}       // Static field - Specific prefab for creating building
    
    private GameObject tileOccupied = null;        // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;       // Reference to real MapTile on which building is set
    private GameObject tileOccupied2 = null;       // Reference to real MapTile on which building is set

    private GameObject shieldRangePrefab;
    private GameObject shieldGeneratorRangeRef;



    public int level = 1;




    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.TripleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.shieldGeneratorPrefab;
    }


    // Function for creating building
    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied2 = model.BTileTwo;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        shieldGenerator_counter++;
        
        this.gameObject.name = "ShieldGenerator" + ShieldGenerator.shieldGenerator_counter;

        shieldRangePrefab = PrefabManager.Instance.shieldGeneratorRangePrefab;
    }


    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShieldGeneratorMenu");
        
        if (!shieldGeneratorMenuReference) // executes once
        {
            shieldGeneratorMenuReference = GameObject.Find("ShieldGeneratorMenu").GetComponent<ShiledGeneratorMenu>();
        }

        shieldGeneratorMenuReference.ReloadPanel(this);
    }

#region  ShieldGenerator functions

    public void Upgrade()
    {
        level++;
    }

    public void ActivateShield()
    {
        if (!shieldGeneratorRangeRef)
        {
            shieldGeneratorRangeRef = GameObject.Instantiate (shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
            
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";
        }
        else
        {
            Debug.Log("Error! Shield is already On!");
        }
    }

    public void DisableShield()
    {
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

#endregion
}