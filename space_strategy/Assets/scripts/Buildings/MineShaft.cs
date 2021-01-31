using UnityEngine;
using System.Collections.Generic;

public class MineShaft : AliveGameUnit, IBuilding
{
    public static ShaftMenu shaftMenuReference;  // Reference to UI panel

    private Unit _workerRef;                     // Reference for existing Unit object - for algorithm calculations
    public List<Unit> unitsWorkers;              // List of Units that are working on this shaft
    public Vector3 dispenserPosition;            // Position of helper game object (for Unit FSM transitions)
    
    public int capacity = 3;                     // Standart capacity of shaft (can be extended further)

    public bool isMenuOpened = false;

    public int type = 0;




    private void Update() // TEST ONLY
    {
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     DestroyShaft();
        // }
    }

    public override void TakeDamage(float damagePoints)
    {
        base.TakeDamage(damagePoints);
        HealthPoints -= damagePoints;

        if (isMenuOpened)
        {
            shaftMenuReference.ReloadPanel(this);
        }
    }

    // Initializing helper GameObject - Dispenser
    public void HelperObjectInit()
    {
        unitsWorkers = new List<Unit>();

        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.shaftDispenserTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            
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
    
    public virtual void Upgrade() // Reload here (IN FUTURE) becuse we can start timer to upgrade and stay in UnitManageMenu
    {
        capacity += 2;




        /////////////////////////////////////////////////////////////////////////////////////////////////


        // Reload Unit Manage Menu - shaft became larger and capacity extends
        ReloadMenuSlider();



        /////////////////////////////////////////////////////////////////////////////////////////////////






    }





    public void AddWorkerViaSlider() // Reload slider here because they are involved in process
    {
        _workerRef = ResourceManager.Instance.SetAvaliableUnitToWork(_workerRef); // Initialize adding unit reference

        if (!_workerRef)
        {
            Debug.Log("ERROR - No Units avaliable!");
            return;
        }     
        
        _workerRef.workPlace = this;
        unitsWorkers.Add(_workerRef);
        ResourceManager.Instance.avaliableUnits.Remove(_workerRef);
        _workerRef = null;

        Debug.Log("Unit is successfully added to work progress!"); 
    }
    
    public void RemoveWorkerViaSlider() // Reload slider here because they are involved in process
    {
        _workerRef = unitsWorkers[(unitsWorkers.Count)-1];

        RemoveUnit(_workerRef);

        ResourceManager.Instance.avaliableUnits.Add(_workerRef);
        
        _workerRef = null;
        
        Debug.Log("Removed Unit from WorkPlace!");
    }





    public void RemoveUnit(Unit unit) // Helper function
    {
        unit.workPlace = null;     // Set workplace - null
        unitsWorkers.Remove(unit); // Remove from workers list
    }

    public virtual void DestroyShaft() // Reload Slider here becuse Shaft is involved in slider process
    {
        foreach (var unit in unitsWorkers)
        {
            unit.workPlace = null;
            ResourceManager.Instance.avaliableUnits.Add(unit);
        }
        unitsWorkers.Clear();
    }





















    // Find out which type of shaft it is and reload that Slider
    public void ReloadMenuSlider()
    {
        if (GameHendler.Instance.isMenuAllResourcesTabOpened == true)
        {
            if (this.GetComponent<CrystalShaft>())
            {
                GameHendler.Instance.unitManageMenuReference.ReloadCrystalSlider();   
            }
            else if (this.GetComponent<GelShaft>())
            {
                GameHendler.Instance.unitManageMenuReference.ReloadGelSlider();
            }
            else if (this.GetComponent<IronShaft>())
            {
                GameHendler.Instance.unitManageMenuReference.ReloadIronSlider();
            }
        }
    }





#endregion

}