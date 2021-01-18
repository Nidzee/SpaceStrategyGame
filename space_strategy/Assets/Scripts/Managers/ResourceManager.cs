using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    // Resources
    private int resourceCrystalCount;
    private int resourceMetalCount;
    private int resourceGelCount;

    // Unit Resources
    public List<Unit> unitsList = new List<Unit>();
    public List<Unit> avaliableUnits = new List<Unit>();
    public List<Unit> homelessUnits = new List<Unit>();
    // public List<Unit> workingUnits = new List<Unit>();    // idk if this list is necessary

    // Shafts list
    public List<CrystalShaft> crystalShaftList = new List<CrystalShaft>();
    public List<IronShaft> ironShaftList = new List<IronShaft>();
    public List<GelShaft> gelShaftList = new List<GelShaft>();


    public void SetAvaliableUnitToWork(Unit workerRef)
    {
        if (avaliableUnits.Count == 0)
        {
            workerRef = null;
            Debug.Log("There is no Avaliable Unit at the moment!");
        }
        else
        {
            workerRef = avaliableUnits[(avaliableUnits.Count) - 1];

            avaliableUnits.Remove(workerRef);
            //workingUnits.Add(workerRef);
            Debug.Log("Avaliable Unit is assigned succesfully!");
        }
    }

    public void SetAllUnitsToAvaliable()
    {
        foreach(var unit in unitsList)
        {
            unit.workPlace = null;
        }
        //AvaliableUnits = UnitList;
    }

    public void AddCrystalResourcePoints()
    {
        resourceCrystalCount++;
    }

    public void AddIronResourcePoints()
    {
        resourceMetalCount++;
    }

    public void AddGelResourcePoints()
    {
        resourceGelCount++;
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
