using UnityEngine;
using System.Collections.Generic;

public class Garage :  AliveGameUnit, IBuilding
{
    public static GarageMenu garageMenuReference;                    // Reference to UI panel (same field for all Garages)
    
    public static Tile_Type PlacingTileType {get; private set;}      // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}      // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}      // Static field - Specific prefab for creating building

    public static GameObject UnitPrefab {get; private set;}          // Static field - Unit prefab
    
    private static int garage_counter = 0;        // For understanding which building number is this    
    public const int garageCapacity = 5;          // Constant field - All garages have same capacity
    public float timerForCreatingUnit = 0f;       // Timer for creating unit
    
    private GameObject tileOccupied = null;       // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;      // Reference to real MapTile on which building is set

    private Unit unitRef = null;                  // Reference tu some Unit for algorithms
    public List<Unit> garageMembers;              // Units that are living here
    
    public Vector3 angarPosition;                 // ANgar position (for Unit FSM transitions)

    public bool isMenuOpened = false;
    public bool isCreationInProgress = false;



    // Unit creation logic
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.name == "G1")
            {
                DestroyGarage();
            }
        }

        if (isCreationInProgress)
        {
            timerForCreatingUnit += 0.005f;

            if (timerForCreatingUnit > 1)
            {
                timerForCreatingUnit = 0f;
                isCreationInProgress = false;

                Unit unit = Instantiate(UnitPrefab, angarPosition, Quaternion.identity).GetComponent<Unit>();
                unit.CreateInGarage(this);

                Debug.Log("Unit created!");

                // Reload all info below
                if (GameHendler.Instance.isUnitManageMenuOpened)
                {
                    // Anyway reload unit counter because it is above all panels and it expands All units list
                    GameHendler.Instance.unitManageMenuReference.ReloadMainUnitCount();
                }

                if (isMenuOpened)
                {
                    // No need for reloading name or HP/SP or icon
                    garageMenuReference.ReloadUnitManage();
                }
            }
        }
    }


    public override void TakeDamage(float damagePoints)
    {
        base.TakeDamage(damagePoints);
        HealthPoints -= damagePoints;

        if (isMenuOpened)
        {
            garageMenuReference.ReloadPanel(this);
        }
    }


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.garagePrefab;

        UnitPrefab = PrefabManager.Instance.unitPrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;

        ResourceManager.Instance.garagesList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        garage_counter++;

        this.gameObject.name = "G" + Garage.garage_counter;

        garageMembers = new List<Unit>();
        HelperObjectInit();
        AddHomelessUnit();
    }

    // Initializing helper GameObject - Angar or throwing ERROR if it is impossible
    private void HelperObjectInit()                     
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.garageAngarTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);

            gameObject.transform.GetChild(0).transform.position = tileOccupied1.transform.position;
            
            angarPosition = gameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("GarageMenu");
        
        if (!garageMenuReference) // executes once
        {
            garageMenuReference = GameObject.Find("GarageMenu").GetComponent<GarageMenu>();
        }

        garageMenuReference.ReloadPanel(this);
    }


#region Garage logic funsctions
    
    public void CreateUnit() // No nned for slider reload because they became free from work and they are nit involved in shaft process
    {
        isCreationInProgress = true;
    }


    public void AddHomelessUnit() // No need for slider reload because they became free from work and they are nit involved in shaft process
    {
        if (ResourceManager.Instance.homelessUnits.Count != 0)
        {
            for (int i = 0; i < garageCapacity; i++)
            {
                unitRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];

                unitRef.home = this;
                garageMembers.Add(unitRef);
                ResourceManager.Instance.homelessUnits.Remove(unitRef);
                ResourceManager.Instance.avaliableUnits.Add(unitRef);

                Debug.Log("Added homeless unit!");
                

                if (ResourceManager.Instance.homelessUnits.Count == 0)
                {
                    return;
                }
            }
        }
        else
        {
            Debug.Log("No homeless units!");
            return;
        }
    }


    public void RemoveUnit(Unit deadUnit) // Formal function
    {
        deadUnit.home = null;
        garageMembers.Remove(deadUnit);
    }


    public void DestroyGarage() // Reload slider here because some units from garage can be on work
    {
        List<MineShaft> shaftsToReloadSliders = new List<MineShaft>();

        foreach (var unit in garageMembers)
        {
            if (unit.workPlace)
            {
                // If garage destroys but unit which was garage member was working and its shaft menu was open
                if (unit.workPlace.isMenuOpened)
                {
                    unit.workPlace.RemoveUnit(unit);
                    MineShaft.shaftMenuReference.ReloadUnitSlider(); // _myShaft reference is already initilized and reloading will be correct
                }
                else // if unit was working but garage destroys
                {
                    shaftsToReloadSliders.Add(unit.workPlace);
                    unit.workPlace.RemoveUnit(unit);
                }
            }
            
            else
            {
                ResourceManager.Instance.avaliableUnits.Remove(unit);
            }

            unit.home = null;
            ResourceManager.Instance.homelessUnits.Add(unit);
        }

        garageMembers.Clear();

        ResourceManager.Instance.garagesList.Remove(this);

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;


        if (GameHendler.Instance.isUnitManageMenuOpened)
        {
            // Always need to reload because some units may be working on shafts
            GameHendler.Instance.unitManageMenuReference.ReloadMainUnitCount();
 
            if (GameHendler.Instance.isMenuAllResourcesTabOpened)
            {
                ReloadMenuSlider();
            }

            else
            {
                for (int i = 0; i < shaftsToReloadSliders.Count; i ++)
                {
                    GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(shaftsToReloadSliders[i], shaftsToReloadSliders[i].type);
                }
            }
        }
        
        Destroy(gameObject);
    }


    // Find out which type of shaft it is and reload that Slider
    public void ReloadMenuSlider()
    {
        GameHendler.Instance.unitManageMenuReference.ReloadCrystalSlider();   
        GameHendler.Instance.unitManageMenuReference.ReloadGelSlider();
        GameHendler.Instance.unitManageMenuReference.ReloadIronSlider();
    }

#endregion

}