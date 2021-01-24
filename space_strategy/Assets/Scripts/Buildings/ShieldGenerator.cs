using UnityEngine;

public class ShieldGenerator :  AliveGameUnit, IBuilding
{
    [SerializeField] private RectTransform shieldGeneratorPanelReference; // Reference to UI panel
    
    public static int shieldGenerator_counter = 0; // For understanding which building number is this
    public static Tile_Type placingTileType;       // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType;       // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab;       // Static field - Specific prefab for creating building
    
    private GameObject tileOccupied = null;        // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;       // Reference to real MapTile on which building is set
    private GameObject tileOccupied2 = null;       // Reference to real MapTile on which building is set

    [SerializeField] private GameObject shieldRangePrefab;
    private GameObject shieldGeneratorRangeRef;


    public static void InitStaticFields()          // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.TripleTileBuilding;
        buildingPrefab = PrefabManager.Instance.shieldGeneratorPrefab;
    }

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
    }

    public void Invoke() // Function for displaying info
    {
        Debug.Log("Selected ShieldGenerator - go menu now");
    }

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