using UnityEngine;
using System.Collections.Generic;

public class Garage :  AliveGameUnit, IBuilding
{
    public static int garage_counter = 0; // for easy Debug
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    private GameObject tileOccupied = null; // Tile on which building is set
    private GameObject tileOccupied1 = null; // Tile on which building is set

    private Unit unitRef;
    private List<Unit> garageMembers = null;
    private int garageCapacity = 5;

    public Vector3 angarPosition;


    private void Awake() // Maybe useless
    {
        gameObject.transform.GetChild(0).position += OffsetConstants.dispenserOffset;
        gameObject.transform.GetChild(0).tag = TagConstants.garageAngarTag;
        angarPosition = gameObject.transform.GetChild(0).transform.position;
    }

    public static void InitStaticFields() // Untouchable
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = PrefabManager.Instance.garagePrefab;
    }

    public void Creation(Model model)     // Untouchable
    {
        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        garage_counter++;
        this.gameObject.tag = TagConstants.buildingTag;
        this.gameObject.name = "Garage" + Garage.garage_counter;

        // Tag buisness in Awake

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


    public void Invoke()
    {
        
    }
}