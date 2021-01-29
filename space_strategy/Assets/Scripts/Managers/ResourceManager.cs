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
    public List<Garage> garagesList;



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



    public CrystalShaft FindFreeCrystalShaft()
    {
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++) // MAYBE PROBLEM HERE
        {
            CrystalShaft temp = ResourceManager.Instance.crystalShaftList[i];
            
            if (temp.unitsWorkers.Count < temp.capacity)
            {
                Debug.Log("Found free CrystalShaft!");
                return temp;
            }
        }

        // Debug.Log("There is no free CrystalShaft");
        return null;
    }

    public IronShaft FindFreeIronShaft()
    {
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count-1; i++) // MAYBE PROBLEM HERE
        {
            IronShaft temp = ResourceManager.Instance.ironShaftList[i];
            
            if (temp.unitsWorkers.Count < temp.capacity)
            {
                Debug.Log("Found free IronShaft");
                return temp;
            }
        }

        Debug.Log("There is no free IronShaft");
        return null;
    }

    public GelShaft FindFreeGelShaft()
    {
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count-1; i++) // MAYBE PROBLEM HERE
        {
            GelShaft temp = ResourceManager.Instance.gelShaftList[i];
            
            if (temp.unitsWorkers.Count < temp.capacity)
            {
                Debug.Log("Found free GelShaft!");
                return temp;
            }
        }

        Debug.Log("There is no free GelShaft!");
        return null;
    }

    public Garage FindFreeGarage()
    {
        for (int i = 0; i < ResourceManager.Instance.garagesList.Count-1; i++) // MAYBE PROBLEM HERE
        {
            Garage temp = ResourceManager.Instance.garagesList[i];
            
            if (temp.garageMembers.Count < Garage.garageCapacity)
            {
                Debug.Log("Found free garage!");
                return temp;
            }
        }

        Debug.Log("There is no free!");
        return null;
    }



    public int ShaftSumCapacity(List<MineShaft> shafts)
    {
        int counter = 0;

        for (int i=0; i < shafts.Count; i++)
        {
            counter += shafts[i].capacity;
        }

        return counter;
    }

    public int ShaftSumFillness(List<MineShaft> shafts)
    {
        int counter = 0;

        for (int i=0; i < shafts.Count; i++)
        {
            counter += shafts[i].unitsWorkers.Count;
        }

        return counter;
    }


    public int GarageSumCapacity(List<Garage> garages)
    {
        int counter = 0;

        for (int i=0; i < garages.Count; i++)
        {
            counter += Garage.garageCapacity;
        }

        return counter;
    }

    public int GarageSumFillness(List<Garage> garages)
    {
        int counter = 0;

        for (int i=0; i < garages.Count; i++)
        {
            counter += garages[i].garageMembers.Count;
        }

        return counter;
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
        garagesList = new List<Garage>();

        unitsList = new List<Unit>();
        avaliableUnits = new List<Unit>();
        homelessUnits = new List<Unit>();
    }
}
