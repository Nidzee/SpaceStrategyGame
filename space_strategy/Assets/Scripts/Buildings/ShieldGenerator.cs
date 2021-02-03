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
    public GameObject shieldGeneratorRangeRef;

    public int level = 1;

    public bool isMenuOpened = false;


    private static Vector3 standartScale = new Vector3(30,30,1);
    private static Vector3 expandedScale = new Vector3(50,50,1);

    private Vector3 scale = standartScale;

    public bool isShieldCreationStarted = false;
    public bool isDisablingStarted = false;


    public bool isUpgraded = false;


    private GameObject rangeRef;

    public float upgradeTimer = 0f;
    public bool isUpgradeInProgress = false;



    private void Update()
    {
        if (isUpgradeInProgress)
        {
            upgradeTimer += 0.005f;

            if (upgradeTimer > 1)
            {
                upgradeTimer = 0f;           // Reset timer
                isUpgradeInProgress = false; // Turn off the timer
                
                level++;  
                scale += new Vector3(20,20,0); 
                isUpgraded = true;

                Debug.Log("Shield Generator levelUP!");

                if (isMenuOpened)
                {
                    shieldGeneratorMenuReference.ReloadLevelManager();
                }
            }
        }

        if (isUpgraded && shieldGeneratorRangeRef && !isShieldCreationStarted && !isDisablingStarted)
        {
            isShieldCreationStarted = true;
            shieldGeneratorMenuReference.OFFbutton.interactable = false;
            shieldGeneratorMenuReference.ONbutton.interactable = false;
        }

        if (isShieldCreationStarted)
        {
            rangeRef.transform.localScale += new Vector3(0.5f,0.5f,0);

            if (rangeRef.transform.localScale == scale)
            {
                isShieldCreationStarted = false;
                shieldGeneratorMenuReference.OFFbutton.interactable = true;
                isUpgraded = false;
            }
        }

        if (isDisablingStarted)
        {
            rangeRef.transform.localScale -= new Vector3(0.5f,0.5f,0);
            
            if (rangeRef.transform.localScale == new Vector3(1,1,1))
            {
                isDisablingStarted = false;
                shieldGeneratorMenuReference.ONbutton.interactable = true;
                Destroy(shieldGeneratorRangeRef);   
            }
        }
    }










    // Reloads sliders if Turret Menu is opened
    public override void TakeDamage(float DamagePoints)
    {
        base.TakeDamage(DamagePoints);

        if (isMenuOpened)
        {
            shieldGeneratorMenuReference.ReloadSlidersHP_SP();
        }
    }

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
        isUpgradeInProgress = true;
    }

    public void EnableShield()
    {
        if (!shieldGeneratorRangeRef)
        {
            shieldGeneratorRangeRef = GameObject.Instantiate (shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
            
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            rangeRef = shieldGeneratorRangeRef;

            isShieldCreationStarted = true;

                
        }
        else
        {
            Debug.Log("Error! Shield is already On!");
        }
    }

    public void DisableShield()
    {
        if (shieldGeneratorRangeRef && !isShieldCreationStarted)
        {
            isDisablingStarted = true;
            Debug.Log("Deleting ShieldGeneratorRange!");
        }
        else
        {
            Debug.Log("Error! Shield is already Off!");
        }
    }

#endregion
}