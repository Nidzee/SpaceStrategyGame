using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    //static readonly int IDgarage = 1;

    public static ResourceManager Instance {get; private set;}

    // Resources
    public int resourceCrystalCount;
    public int resourceMetalCount;
    public int resourceGelCount;

    // Unit Resources
    public List<Unit> UnitList;
    public List<Unit> AvaliableUnits;
    public List<Unit> HomelessUnits;

    public void SetFreeUnitToWork(Unit worker)
    {
        if (AvaliableUnits.Count == 0)
        {
            worker = null;
            Debug.Log("There is no Avaliable Unit at the moment!");
        }
        else
        {
            AvaliableUnits.RemoveAt((AvaliableUnits.Count) - 1);
            worker = AvaliableUnits[(AvaliableUnits.Count) - 1];
            Debug.Log("Avaliable Unit is assigned succesfully!");
        }
    }

    public void SetAllUnitsToAvaliable()
    {
        foreach(var unit in UnitList)
        {
            unit.WorkPlace = null;
        }
        //AvaliableUnits = UnitList;
    }

    // public void AddResourcePoints(int ResourcePoints)
    // {

    // }



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
