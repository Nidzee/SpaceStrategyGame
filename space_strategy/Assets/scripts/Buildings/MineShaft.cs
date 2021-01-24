using UnityEngine;
using System.Collections.Generic;

public class MineShaft : AliveGameUnit, IBuilding
{
    public Unit workerRef;            // Reference for existing Unit object - for algorithm calculations
    public List<Unit> unitsWorkers;   // List of Units that are working on this shaft
    public Vector3 dispenserPosition; // Position of helper game object (for Unit FSM transitions)
    
    public int capacity = 5;          // Standart capacity of shaft (can be extended further)


    private void Awake()              // Initializing helper GameObject - Dispenser
    {
        unitsWorkers = new List<Unit>();
        gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.radiusLayer);
        gameObject.transform.GetChild(0).tag = TagConstants.shaftDispenserTag;
        dispenserPosition = gameObject.transform.GetChild(0).transform.position;
        // No sprite renderer
    }
 
#region Shaft logic functions
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
#endregion

    public virtual void Invoke() // Function for displaying info
    {
        // Function for displaying info
    }
}