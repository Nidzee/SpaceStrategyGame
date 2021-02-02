using UnityEngine;
using System.Collections.Generic;

public class MineShaft : AliveGameUnit, IBuilding
{
    public static ShaftMenu shaftMenuReference;  // Reference to UI panel

    private Unit _workerRef;                     // Reference for existing Unit object - for algorithm calculations
    public List<Unit> unitsWorkers;              // List of Units that are working on this shaft
    public Vector3 dispenserPosition;            // Position of helper game object (for Unit FSM transitions)
    
    public int capacity = 3;                     // Standart capacity of shaft (can be extended further)
    public int type = 0;
    public int level = 1;

    public float upgradeTimer = 0f;
    public bool isUpgradeInProgress = false;
    public bool isMenuOpened = false;



    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     if (gameObject.name == "CS1")
        //     {
        //         DestroyShaft();
        //     }
        // }

        if (isUpgradeInProgress)
        {
            upgradeTimer += 0.0005f;

            if (upgradeTimer > 1)
            {
                upgradeTimer = 0f;           // Reset timer
                isUpgradeInProgress = false; // Turn off the timer
                capacity += 2;               // Expand capacity
                level++;                     // Increments level

                Debug.Log("Some Shaft EXPAND!");

                if (isMenuOpened)            // Update menu if it is opened
                {
                    // No need for reloading name
                    // No need for reloading HP/SP because it is TakeDamage buisness

                    shaftMenuReference.ReloadLevelManager(); // update buttons and vizuals
                    shaftMenuReference.ReloadUnitSlider();   // expands slider
                }

                // No need for reloading UnitManageMenu - unitCounter - because no new units created or died or else
                // Only need to reload sliders or specific slider tab
                if (GameHendler.Instance.isUnitManageMenuOpened)
                {
                    if (GameHendler.Instance.isMenuAllResourcesTabOpened)
                    {
                        ReloadMenuSlider();
                    }

                    if (GameHendler.Instance.isMenuCrystalTabOpened && type == 1)
                    {
                        GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(this, 1);
                    }

                    if (GameHendler.Instance.isMenuGelTabOpened && type == 2)
                    {
                        GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(this, 2);
                    }

                    if (GameHendler.Instance.isMenuIronTabOpened && type == 3)
                    {
                         GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(this, 3);
                    }
                }
            }
        }
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
        upgradeTimer = 0;
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
    
    // Upgrade logic in update
    public virtual void Upgrade()
    {
        isUpgradeInProgress = true;
    }



    // No need for reloading because if it is Main SLider Menu - it is reloading there
    // If it is inside Shaft Menu - than it is reloading there
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



    // Removes unit from shaft
    public void RemoveUnit(Unit unit) // Helper function
    {
        unit.workPlace = null;     // Set workplace - null
        unitsWorkers.Remove(unit); // Remove from workers list
    }


    // Add closing menu if shaft destroys
    public virtual void DestroyShaft() // Reload Slider here becuse Shaft is involved in slider process
    {
        if (isMenuOpened)
        {
            shaftMenuReference.ExitMenu();
        }

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

#endregion

}