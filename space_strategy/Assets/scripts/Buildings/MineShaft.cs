using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public class MineShaft : AliveGameUnit, IBuilding
{
    public static ShaftMenu shaftMenuReference;  // Reference to UI panel
    private Unit _workerRef;                     // Reference for existing Unit object - for algorithm calculations
    public List<Unit> unitsWorkers = new List<Unit>();              // List of Units that are working on this shaft
    public GameObject dispenser;            // Position of helper game object (for Unit FSM transitions)
    
    // Every shaft must-have variables
    public int capacity = 3;                     // Standart capacity of shaft (can be extended further)
    public int type = 0;
    public int level = 1;
    public bool isMenuOpened = false;

    // Upgrade logic
    public float upgradeTimer = 0f;
    private float _timerStep = 0.5f;







    private static int _crystalNeedForBuilding = 0;
    private static int _ironNeedForBuilding = 0;
    private static int _gelNeedForBuilding = 0;

    private static int _crystalNeedForExpand_ToLvl2;
    private static int _ironNeedForForExpand_ToLvl2;
    private static int _gelNeedForForExpand_ToLvl2;

    private static int _crystalNeedForExpand_ToLvl3;
    private static int _ironNeedForForExpand_ToLvl3;
    private static int _gelNeedForForExpand_ToLvl3;


    public static string GetResourcesNeedToBuildAsText()
    {
        return _crystalNeedForBuilding.ToString() + " " + _ironNeedForBuilding.ToString() +" "+_gelNeedForBuilding.ToString();
    }



    public static void GetResourcesNeedToBuild(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        crystalNeed = _crystalNeedForBuilding;
        ironNeed = _ironNeedForBuilding;
        gelNeed = _gelNeedForBuilding;
    }

    public static void GetResourcesNeedToExpand(out int crystalNeed, out int ironNeed, out int gelNeed, MineShaft shaft)
    {
        if (shaft.level == 1)
        {
            crystalNeed = _crystalNeedForExpand_ToLvl2;
            ironNeed = _ironNeedForForExpand_ToLvl2;
            gelNeed = _gelNeedForForExpand_ToLvl2;
        }
        else
        {
            crystalNeed = _crystalNeedForExpand_ToLvl3;
            ironNeed = _ironNeedForForExpand_ToLvl3;
            gelNeed = _gelNeedForForExpand_ToLvl3;
        }
    }

    public static void InitCost_ToLvl2()
    {
        _crystalNeedForExpand_ToLvl2 = 5;
        _ironNeedForForExpand_ToLvl2 = 5;
        _gelNeedForForExpand_ToLvl2 = 5;

        shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl2.ToString() + " " + _ironNeedForForExpand_ToLvl2.ToString() +" "+_gelNeedForForExpand_ToLvl2.ToString();
    }

    public static void InitCost_ToLvl3()
    {
        _crystalNeedForExpand_ToLvl3 = 10;
        _ironNeedForForExpand_ToLvl3 = 10;
        _gelNeedForForExpand_ToLvl3 = 10;

        shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl3.ToString() + " " + _ironNeedForForExpand_ToLvl3.ToString() +" "+_gelNeedForForExpand_ToLvl3.ToString();
    }
















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
        // capacity += 2;               // Expand capacity
        // level++;                     // Increments level

        if (level == 1)
        {
            InitStaticsLevel_2();
            Debug.Log("Some Shaft EXPAND!");
        }
        else if (level == 2)
        {
            InitStaticsLevel_3();
            Debug.Log("Some Shaft EXPAND!");
        }
        else
        {
            Debug.Log("ERROR! - Invalid shaft level!!!!!");
        }


        

        if (isMenuOpened)            // Update menu if it is opened
        {
            // No need for reloading name
            // No need for reloading HP/SP because it is TakeDamage buisness

            if (level == 1)
            {
                InitCost_ToLvl2();
            }
            else if (level == 2)
            {
                InitCost_ToLvl3();
            }
            else
            {
                shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
            }

            shaftMenuReference.ReloadLevelManager(); // update buttons and vizuals
            shaftMenuReference.ReloadUnitSlider();   // expands slider
        }

        // No need for reloading UnitManageMenu - unitCounter - because no new units created or died or else
        // Only need to reload sliders or specific slider tab
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(this);
    }


















    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);


        // Reloads sliders if Damage taken
        if (isMenuOpened)
        {
            shaftMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadShaftHP_SPAfterDamage(this);
    }


    private static int maxHealth_Lvl1 = 100; 
    private static int maxHealth_Lvl2 = 200; 
    private static int maxHealth_Lvl3 = 300;

    private static int maxShiled_Lvl1 = 100; 
    private static int maxShiled_Lvl2 = 200; 
    private static int maxShiled_Lvl3 = 300;

    private static int deffencePoints_Lvl1 = 10; 
    private static int deffencePoints_Lvl2 = 12; 
    private static int deffencePoints_Lvl3 = 15;



    public void InitStaticsLevel_1()
    {
        level = 1;
        capacity = 3; 

        healthPoints = maxHealth_Lvl1;
        maxCurrentHealthPoints = maxHealth_Lvl1;

        shieldPoints = maxShiled_Lvl1;
        maxCurrentShieldPoints = maxShiled_Lvl1;

        deffencePoints = deffencePoints_Lvl1;
    }

    public void InitStaticsLevel_2()
    {
        Debug.Log("InitStaticsLevel_2");
        level = 2;
        capacity = 5; 

        healthPoints = (maxHealth_Lvl2 * healthPoints) / maxHealth_Lvl1;
        maxCurrentHealthPoints = maxHealth_Lvl2;

        shieldPoints = (maxShiled_Lvl2 * shieldPoints) / maxShiled_Lvl1;
        maxCurrentShieldPoints = maxShiled_Lvl2;

        deffencePoints = deffencePoints_Lvl2;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            shaftMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadShaftHP_SPAfterDamage(this); // Or after shaft maxHealth expand
    }

    public void InitStaticsLevel_3()
    {
        Debug.Log("InitStaticsLevel_3");
        level = 3;
        capacity = 7; 

        healthPoints = (maxHealth_Lvl3 * healthPoints) / maxHealth_Lvl2;
        maxCurrentHealthPoints = maxHealth_Lvl3;

        shieldPoints = (maxShiled_Lvl3 * shieldPoints) / maxShiled_Lvl2;
        maxCurrentShieldPoints = maxShiled_Lvl3;

        deffencePoints = deffencePoints_Lvl3;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            shaftMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadShaftHP_SPAfterDamage(this); // Or after shaft maxHealth expand
    }







    private static int baseUpgradeStep = 30;

    public static void UpgradeStatisticsAfterBaseUpgrade()
    {
        maxHealth_Lvl1 += baseUpgradeStep;
        maxHealth_Lvl2 += baseUpgradeStep;
        maxHealth_Lvl3 += baseUpgradeStep;

        maxShiled_Lvl1 += baseUpgradeStep;
        maxShiled_Lvl2 += baseUpgradeStep;
        maxShiled_Lvl3 += baseUpgradeStep;
    }

    public void InitStatisticsAfterBaseUpgrade()
    {
        switch (level)
        {
            case 1:
            healthPoints = ((maxHealth_Lvl1 + baseUpgradeStep) * healthPoints) / maxHealth_Lvl1;
            maxCurrentHealthPoints = (maxHealth_Lvl1 + baseUpgradeStep);

            shieldPoints = ((maxShiled_Lvl1 + baseUpgradeStep) * shieldPoints) / maxShiled_Lvl1;
            maxCurrentShieldPoints = (maxShiled_Lvl1 + baseUpgradeStep);

            deffencePoints = deffencePoints_Lvl1; // not changing at all
            break;

            case 2:
            healthPoints = ((maxHealth_Lvl2 + baseUpgradeStep) * healthPoints) / maxHealth_Lvl2;
            maxCurrentHealthPoints = (maxHealth_Lvl2 + baseUpgradeStep);

            shieldPoints = ((maxShiled_Lvl2 + baseUpgradeStep) * shieldPoints) / maxShiled_Lvl2;
            maxCurrentShieldPoints = (maxShiled_Lvl2 + baseUpgradeStep);

            deffencePoints = deffencePoints_Lvl2; // not changing at all
            break;

            case 3:
            healthPoints = ((maxHealth_Lvl3 + baseUpgradeStep) * healthPoints) / maxHealth_Lvl3;
            maxCurrentHealthPoints = (maxHealth_Lvl3 + baseUpgradeStep);

            shieldPoints = ((maxShiled_Lvl3 + baseUpgradeStep) * shieldPoints) / maxShiled_Lvl3;
            maxCurrentShieldPoints = (maxShiled_Lvl3 + baseUpgradeStep);

            deffencePoints = deffencePoints_Lvl3; // not changing at all
            break;
        }
        

        // reload everything here
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

            dispenser = gameObject.transform.GetChild(0).gameObject;
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

        if (level == 1)
        {
            InitCost_ToLvl2();
        }
        else if (level == 2)
        {
            InitCost_ToLvl3();
        }
        else
        {
            shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
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
