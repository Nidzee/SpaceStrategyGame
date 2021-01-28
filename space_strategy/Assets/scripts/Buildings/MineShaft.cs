﻿using UnityEngine;
using System.Collections.Generic;

public class MineShaft : AliveGameUnit, IBuilding
{
    public static ShaftMenu shaftMenuReference;  // Reference to UI panel

    private Unit _workerRef;                     // Reference for existing Unit object - for algorithm calculations
    public List<Unit> unitsWorkers;              // List of Units that are working on this shaft
    public Vector3 dispenserPosition;            // Position of helper game object (for Unit FSM transitions)
    
    public int capacity = 5;                     // Standart capacity of shaft (can be extended further)



    public int test = 0;



    // Initializing helper GameObject - Dispenser
    public void HelperObjectInit()
    {
        unitsWorkers = new List<Unit>();

        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.shaftDispenserTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.helperRadiusLayer);
            
            dispenserPosition = gameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }


    // Function for displaying info
    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShaftMenu");
        
        if (!shaftMenuReference)
        {
            shaftMenuReference = GameObject.Find("ShaftMenu").GetComponent<ShaftMenu>();
        }
    }


#region Shaft logic functions
    
    public void Upgrade()
    {
        capacity += 2;
    }

    public virtual void AddWorkerViaSlider() // Correct
    {
        ResourceManager.Instance.SetAvaliableUnitToWork(_workerRef); // Initialize adding unit reference
        if (_workerRef)
        {            
            _workerRef.AddToJob(this);    // Add to job
            unitsWorkers.Add(_workerRef); // Add to workers list

            _workerRef = null; // Delete unit reference
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
            _workerRef = unitsWorkers[(unitsWorkers.Count)-1]; // Initialize deleting unit reference

            _workerRef.RemoveFromJob(JobLostCode.Slider);      // Remove from job
            unitsWorkers.Remove(_workerRef);                   // Remove from workers list

            _workerRef = null; // Delete unit reference
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

}