using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    // Resources
    public int resourceCrystalCount;
    public int resourceMetalCount;
    public int resourceGelCount;
    public GameObject crystalResourcePrefab;
    public GameObject ironResourcePrefab;
    public GameObject gelResourcePrefab;


    // Unit Resources
    public List<Unit> unitsList = new List<Unit>();
    public List<Unit> avaliableUnits = new List<Unit>();
    public List<Unit> workingUnits = new List<Unit>();
    public List<Unit> homelessUnits = new List<Unit>();

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
            workingUnits.Add(workerRef);
            //avaliableUnits.RemoveAt((avaliableUnits.Count) - 1);
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

    public void AddResourcePoints(int ResourcePoints)
    {

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

        CrystalShaft.crystalShaftResourcePrefab = ResourceManager.Instance.crystalResourcePrefab;
    }
}
