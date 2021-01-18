using UnityEngine;
using System.Collections.Generic;

public class MineShaft : AliveGameUnit, IBuilding
{
    public Unit workerRef;
    public List<Unit> unitsWorkers;

    public Vector3 dispenserPosition; // All shafts have redius object for Unit FSM
    
    
    // Maybe useless function
    private void Awake() // TODO FIX REDO
    {
        gameObject.transform.GetChild(0).tag = "ShaftRadius";
        dispenserPosition = gameObject.transform.GetChild(0).transform.position;
    }
 

    public virtual void AddWorkerViaSlider() // Correct
    {
        ResourceManager.Instance.SetAvaliableUnitToWork(workerRef); // Initialize adding unit reference
        if (workerRef)
        {            
            workerRef.AddToJob(this);    // Add to job
            unitsWorkers.Add(workerRef); // Add to workers list

            workerRef = null; // Delete unit reference
            Debug.Log("Unit is successfully added to work progress!");
        }     
        else
        {
            Debug.Log("ERROR - No Units avaliable!");
        } 
    }
    
    public virtual void RemoveWorkerViaSlider() // Correct
    {
        if (unitsWorkers.Count != 0 )
        {
            workerRef = unitsWorkers[(unitsWorkers.Count)-1]; // Initialize deleting unit reference

            workerRef.RemoveFromJob(JobLostCode.Slider);      // Remove from job
            unitsWorkers.Remove(workerRef);                   // Remove from workers list

            workerRef = null; // Delete unit reference
            Debug.Log("Removed Unit from WorkPlace!");
        }
        else
        {
            Debug.Log("ERROR - There are no Units in this WorkPlace!");            
        }
    }

    public virtual void RemoveDeadUnit(Unit unit) // Correct
    {
        unit.RemoveFromJob(JobLostCode.Death);  // Remove from job
        unitsWorkers.Remove(unit);                 // Remove from workers list
    }

    public virtual void RemoveHomelessUnit(Unit unit)
    {
        unit.RemoveFromJob(JobLostCode.GarageDestroy);
        unitsWorkers.Remove(unit);
    }


    public virtual void DestroyShaft() // Correct
    {
        foreach (var unit in unitsWorkers)
        {
            unit.RemoveFromJob(JobLostCode.ShaftDestroy);
            //unitsWorkers.Remove(unit);
        }
        unitsWorkers.Clear(); // TODO FIX REDO idk if it clears the length/capacity
    }


    public virtual void Invoke() // Function for displaying info
    {
        // UI
    }
}
