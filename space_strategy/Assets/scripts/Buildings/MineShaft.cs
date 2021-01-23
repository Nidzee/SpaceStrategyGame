using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MineShaft : AliveGameUnit, IBuilding
{
    //public GameObject shaftPanelReference; // for panel interaction

    public Unit workerRef;            // All shafts have reference for Unit for cashing real unit
    public List<Unit> unitsWorkers;   // All shafts have list of workers
    public Vector3 dispenserPosition; // All shafts have redius object for Unit FSM
    
    public int capacity = 5;

    private void Awake() // Do not touch every shaft have its dispenser field
    {
        unitsWorkers = new List<Unit>();
        gameObject.transform.GetChild(0).position += OffsetConstants.dispenserOffset;
        gameObject.transform.GetChild(0).tag = TagConstants.shaftDispenserTag;
        dispenserPosition = gameObject.transform.GetChild(0).transform.position;
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