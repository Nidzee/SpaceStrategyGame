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

    public Vector3 angarPosition;
    private int garageCapacity = 5;


    private void Awake() // Maybe useless
    {
        gameObject.transform.GetChild(0).tag = "HomeRadius";
        angarPosition = gameObject.transform.GetChild(0).transform.position;
    }

    public static void InitStaticFields() // Do not touch!
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = PrefabManager.Instance.garagePrefab;
    }

    public void Creation(Model model)
    {
        garage_counter++;

        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        this.gameObject.tag = "Building";
        this.gameObject.name = "Garage" + Garage.garage_counter;

        gameObject.transform.GetChild(0).tag = "HomeRadius";
        angarPosition = gameObject.transform.GetChild(0).transform.position;

        AddHomelessUnit();
    }


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


    public void Invoke()
    {
        
    }
}