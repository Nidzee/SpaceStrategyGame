using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    // Resources
    private int resourceCrystalCount;
    private int resourceMetalCount;
    private int resourceGelCount;
    private int electricityCount;


    // Unit Resources
    public List<Unit> unitsList;
    public List<Unit> avaliableUnits;
    public List<Unit> homelessUnits;


    // Shafts list
    public List<CrystalShaft> crystalShaftList;
    public List<IronShaft> ironShaftList;
    public List<GelShaft> gelShaftList;


    public Unit SetAvaliableUnitToWork(Unit workerRef)
    {
        if (avaliableUnits.Count == 0)
        {
            Debug.Log("There is no Avaliable Unit at the moment!");
            return null;
        }
        else
        {
            workerRef = avaliableUnits[(avaliableUnits.Count) - 1];

            avaliableUnits.Remove(workerRef);
            
            Debug.Log("Avaliable Unit is assigned succesfully!");
            
            return workerRef;
        }
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

    public void IncreaseElectricityCount()
    {
        electricityCount += 50;
    }

    public void DecreaseElectricityCount()
    {
        electricityCount -= 50;
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

        crystalShaftList = new List<CrystalShaft>();
        ironShaftList = new List<IronShaft>();
        gelShaftList = new List<GelShaft>();

        unitsList = new List<Unit>();
        avaliableUnits = new List<Unit>();
        homelessUnits = new List<Unit>();
    }
}
