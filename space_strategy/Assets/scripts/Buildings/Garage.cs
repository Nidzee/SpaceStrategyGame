using UnityEngine;
using System.Collections.Generic;

public class Garage :  AliveGameUnit, IBuilding
{
    private static GarageMenu garageMenuReference;// Reference to UI panel (same field for all Garages)
    
    public static Tile_Type PlacingTileType {get; private set;}      // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}      // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}      // Static field - Specific prefab for creating building

    public static GameObject UnitPrefab {get; private set;}          // Static field - Unit prefab
    
    private static int garage_counter = 0;        // For understanding which building number is this    
    public const int garageCapacity = 5;          // Constant field - All garages have same capacity
    
    private GameObject tileOccupied = null;       // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;      // Reference to real MapTile on which building is set

    private Unit unitRef = null;                  // Reference tu some Unit for algorithms
    public List<Unit> garageMembers;              // Units that are living here
    
    public Vector3 angarPosition;                 // ANgar position (for Unit FSM transitions)




    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.H))
    //     {
    //         DestroyGarage();
    //     }
    // }




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
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        garage_counter++;

        this.gameObject.name = "Garage" + Garage.garage_counter;

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
    
    public void CreateUnit()
    {
        Unit unit = Instantiate(UnitPrefab, angarPosition, Quaternion.identity).GetComponent<Unit>();
        
        unit.CreateInGarage(this);

        Debug.Log("UnitCreated!");
    }

    public void AddHomelessUnit() // Correct
    {
        if (ResourceManager.Instance.homelessUnits.Count != 0)
        {
            for (int i = 0; i < garageCapacity; i++)
            {
                unitRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];

                unitRef.AddToGarage(this);
                garageMembers.Add(unitRef);
                ResourceManager.Instance.homelessUnits.Remove(unitRef);

                Debug.Log("Added homeless unit!");
                
                if (ResourceManager.Instance.homelessUnits.Count == 0)
                    return;
            }
        }
        else
        {
            Debug.Log("No homeless units!");
            return;
        }
    }

    public void RemoveDeadUnit(Unit deadUnit) // Correct
    {
        deadUnit.RemoveFromGarage(HomeLostCode.Death);    // Remove from garage
        garageMembers.Remove(deadUnit);                   // Remove from garageMembers list
    }

    public void DestroyGarage() // Correct
    {
        foreach (var unit in garageMembers)
        {
            unit.RemoveFromGarage(HomeLostCode.GarageDestroy);
        }
        garageMembers.Clear();
    }

#endregion

}