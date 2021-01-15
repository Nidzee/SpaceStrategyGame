using UnityEngine;
using System.Collections.Generic;

public class MineShaft : AliveGameUnit, IBuilding
{
    public Unit workerRef;
    public List<Unit> unitsWorkers;
    

    public virtual void AddWorker() // Correct
    {
        ResourceManager.Instance.SetAvaliableUnitToWork(workerRef); // 1, 3, 4
        if (workerRef)
        {
            Debug.Log("Unit is successfully added to work progress!");
            workerRef.workPlace = this.gameObject; // 2
            unitsWorkers.Add(workerRef); // 5
        }     
        else
        {
            Debug.Log("No Units avaliable!");
        }   
    }
    
    public virtual void DeleteWorker() // Correct
    {
        if (unitsWorkers.Count == 0 )
        {
            Debug.Log("There are no Units in this WorkPlace!");
        }
        else
        {
            // !!!!!!!!!!!!!!!!!!MAYBE ORDER IS WRONG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            workerRef = unitsWorkers[(unitsWorkers.Count)-1]; // 1
            workerRef.workPlace = null; // 2
            ResourceManager.Instance.workingUnits.Remove(workerRef); // 3
            ResourceManager.Instance.avaliableUnits.Add(workerRef); // 4
            unitsWorkers.Remove(workerRef); // 5
            workerRef.resourcePrefab = null;
            Debug.Log("Removed Unit from WorkPlace!");
        }
    }

    public virtual void RemoveUnit(Unit unit) // Correct
    {
        unitsWorkers.Remove(unit);
        ResourceManager.Instance.workingUnits.Remove(unit);
        unit.workPlace = null; // OPTIONAL
        unit.resourcePrefab = null; // OPTIONAL
    }

    public virtual void DeleteAllWorkers() // Correct
    {
        foreach (var unit in unitsWorkers)
        {
            unit.workPlace = null;
            unit.resourcePrefab = null;
            ResourceManager.Instance.workingUnits.Remove(unit);
            ResourceManager.Instance.avaliableUnits.Add(unit);
        }
        unitsWorkers.Clear();
    }

    
    public virtual void Invoke() // Function for displaying info
    {
        // UI
    }
}
