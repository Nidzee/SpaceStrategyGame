using UnityEngine;
using System.Collections.Generic;

public class MineShaft : AliveGameUnit, IBuilding
{
    private Sprite BuildingSprite;
    private GameObject Resource;
    private int UnitsCapacity = 4;
    private Unit WorkerRef;
    private List<Unit> UnitsWorkers;

    public void UnitsCapacityExpand()
    {
        UnitsCapacity += 3;
    }

    private void AddWorker()
    {
        ResourceManager.Instance.SetFreeUnitToWork(WorkerRef);
        if (WorkerRef)
        {
            Debug.Log("Unit is successfully added to work progress!");
            WorkerRef.WorkPlace = this.gameObject;
            WorkerRef = null;
        }     
        else
        {
            Debug.Log("No Units avaliable!");
        }   
    }
    
    private void DeleteWorker()
    {
        if (UnitsWorkers.Count == 0 )
        {
            Debug.Log("There are no Units in this WorkPlace!");
        }
        else
        {
            // !!!!!!!!!!!!!!!!!!MAYBE ORDER IS WRONG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            UnitsWorkers[(UnitsWorkers.Count)-1].WorkPlace = null;
            ResourceManager.Instance.AvaliableUnits.Add(UnitsWorkers[(UnitsWorkers.Count)-1]);
            UnitsWorkers.RemoveAt((UnitsWorkers.Count)-1);
            Debug.Log("Removed Unit from WorkPlace!");
        }
    }
    
    private void DeleteAllWorkers()
    {
        foreach(var unit in UnitsWorkers)
        {
            unit.WorkPlace = null;
            ResourceManager.Instance.AvaliableUnits.Add(unit);
        }
        UnitsWorkers.Clear();
    }

    public void RemoveDeadUnit(Unit deadUnit)
    {
        
    }
    
    public void Invoke() // Function for displaying info
    {

    }
}
