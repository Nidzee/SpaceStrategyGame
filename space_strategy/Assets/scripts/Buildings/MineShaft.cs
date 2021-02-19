﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class MineShaft : AliveGameUnit, IBuilding
{
    public static ShaftMenu shaftMenuReference;  // Reference to UI panel
    private Unit _workerRef;                     // Reference for existing Unit object - for algorithm calculations
    public List<Unit> unitsWorkers = new List<Unit>();              // List of Units that are working on this shaft
    public Vector3 dispenserPosition;            // Position of helper game object (for Unit FSM transitions)
    
    // Every shaft must-have variables
    public int capacity = 3;                     // Standart capacity of shaft (can be extended further)
    public int type = 0;
    public int level = 1;
    public bool isMenuOpened = false;

    // Upgrade logic
    public float upgradeTimer = 0f;
    private float _timerStep = 0.5f;



    // Upgrade logic in update
    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }

    IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += _timerStep * Time.deltaTime;

            if (isMenuOpened)
            {
                // Reload fill circles
                switch(level)
                {
                    case 1:
                    {
                        shaftMenuReference.level2.fillAmount = upgradeTimer;
                    }
                    break;

                    case 2:
                    {
                        shaftMenuReference.level3.fillAmount = upgradeTimer;
                    }
                    break;

                    case 3:
                    {
                        Debug.Log("Error");
                    }
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;

        ShaftUpgrading();
    }

    private void ShaftUpgrading()
    {
        upgradeTimer = 0f;           // Reset timer
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
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);
    }


















    public override void TakeDamage(float damagePoints)
    {
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads sliders if Damage taken
        if (isMenuOpened)
        {
            shaftMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadShaftHP_SPAfterDamage(this);
    }

    // Initializing helper GameObject - Dispenser
    public void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.shaftDispenserTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            // No sorting layer because it is invisible

            dispenserPosition = gameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            Debug.LogError("ERROR!       No child object (For range) in shaft!     Cannot get dispenser coords!");
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

    // Removes unit from shaft - helper function
    public void RemoveUnit(Unit unit)
    {
        unit.workPlace = null;     // Set workplace - null
        unitsWorkers.Remove(unit); // Remove from workers list
    }



    // Reload Slider here becuse Shaft is involved in slider process
    public virtual void DestroyShaft()
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

        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();

        // Rest in CrystalShaft IronShaft and GelShaft
    }

}



    // private void UpgardingLogic()
    // {
    //     if (isUpgradeInProgress)
    //     {
    //         upgradeTimer += 0.0005f;

    //         if (upgradeTimer > 1)
    //         {
    //             upgradeTimer = 0f;           // Reset timer
    //             isUpgradeInProgress = false; // Turn off the timer
    //             capacity += 2;               // Expand capacity
    //             level++;                     // Increments level

    //             Debug.Log("Some Shaft EXPAND!");

    //             if (isMenuOpened)            // Update menu if it is opened
    //             {
    //                 // No need for reloading name
    //                 // No need for reloading HP/SP because it is TakeDamage buisness

    //                 shaftMenuReference.ReloadLevelManager(); // update buttons and vizuals
    //                 shaftMenuReference.ReloadUnitSlider();   // expands slider
    //             }

    //             // No need for reloading UnitManageMenu - unitCounter - because no new units created or died or else
    //             // Only need to reload sliders or specific slider tab
    //             GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);
    //         }
    //     }
    // }
