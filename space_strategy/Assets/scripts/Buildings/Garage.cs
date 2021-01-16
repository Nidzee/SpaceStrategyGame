﻿using UnityEngine;
using System.Collections.Generic;

public class Garage :  AliveGameUnit, IBuilding
{
    public static int garage_counter = 0; // for easy Debug
    public static Tile_Type placingTileType;
    public static BuildingType buildingType;
    public static GameObject buildingPrefab;
    

    private static int garageCapacity = 5;
    public GameObject tileOccupied = null; // Tile on which building is set
    public GameObject tileOccupied1 = null; // Tile on which building is set
    private Unit workerRef;
    private List<Unit> myFellas = null;


    public static void InitStaticFields()
    {
        placingTileType = Tile_Type.FreeTile;
        buildingType = BuildingType.DoubleTileBuilding;
        buildingPrefab = BuildingManager.Instance.garagePrefab;
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

        AddHomelessUnit(); // Can be executed via button?
    }

    public void Invoke()
    {
        
    }



    public void CreateUnit() // FIX
    {

    }

    public void RemoveUnit(Unit deadUnit) // Correct
    {
        myFellas.Remove(deadUnit);
        deadUnit.home = null; // OPTIONAL
    }

    public void AddHomelessUnit() // Correct
    {
        if (ResourceManager.Instance.homelessUnits.Count == 0)
        {
            Debug.Log("No homeless units!");
            return;
        }
        else
        {
            for (int i = 0; i < garageCapacity; i++)
            {
                workerRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];
                workerRef.home = this.gameObject;
                myFellas.Add(workerRef);
                ResourceManager.Instance.homelessUnits.Remove(workerRef);
                ResourceManager.Instance.avaliableUnits.Add(workerRef);
                
                Debug.Log("Added homeless unit!");

                if (ResourceManager.Instance.homelessUnits.Count == 0)
                    return;
            }
        }
    }

    public void DestroyGarage() // Correct
    {
        foreach (var unit in myFellas)
        {
            if (unit.workPlace) //2
            {
                unit.workPlace.GetComponent<Garage>().RemoveUnit(unit);
            }
            else // than he is avaliable
            {
                ResourceManager.Instance.avaliableUnits.Remove(unit);
            }
            ResourceManager.Instance.homelessUnits.Add(unit); // 3
            unit.home = null; // 1
        }
        myFellas.Clear();
    }

}


        // myOwnGarageSprite = model.modelSprite;
        
        // GameObject go = GameObject.Instantiate (myOwnGarageSprite, 
        //     model.BTileZero.transform.position + new Vector3 (0,0,-0.1f), 
        //     Quaternion.Euler(0f, 0f, (model.rotation*60)), this.transform);

        // go.tag = "Building";
        // go.name = "Garage" + garage_counter + " Sprite";
