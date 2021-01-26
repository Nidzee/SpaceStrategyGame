using UnityEngine;
using System.Collections.Generic;

public class Garage :  AliveGameUnit, IBuilding
{
    [SerializeField] private RectTransform garagePanelReference; // Reference to UI panel
    
    public static int garage_counter = 0;    // For understanding which building number is this
    public static Tile_Type placingTileType; // Static field - Tile type on whic building need to be placed
    public static BuildingType buildingType; // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject buildingPrefab; // Static field - Specific prefab for creating building
    public static int garageCapacity = 5;
    
    private GameObject tileOccupied = null;  // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null; // Reference to real MapTile on which building is set

    private Unit unitRef = null;
    public List<Unit> garageMembers;
    
    public Vector3 angarPosition;            // ANgar position (for Unit FSM transitions)


    private void Awake()                     // Initializing helper GameObject - Angar
    {
        garageMembers = new List<Unit>();

        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.shaftDispenserTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.noninteractibleRadiusLayer);
            
            angarPosition = gameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
        // No sprite renderer
    }

    public static void InitStaticFields()    // Static info about building - determins all info about every object of this building class
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = PrefabManager.Instance.garagePrefab;
    }

    public void Creation(Model model)
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        garage_counter++;

        this.gameObject.name = "Garage" + Garage.garage_counter;

        AddHomelessUnit();
    }

#region Garage logic funsctions
    public void CreateUnit() // FIX
    {
        // Unit unit;
        // unit.AddToGarage(this);
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


    public void Invoke() // Function for displaying info
    {
        //Debug.Log("Selected Garage - go menu now");
        //garagePanelReference.ReloadPanel(this);
        //UIPannelManager.Instance.ResetPanels((int)InitPannelIndex.garagePanel);
    }
}