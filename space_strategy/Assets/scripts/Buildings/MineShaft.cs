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
    public int capacity;                     // Standart capacity of shaft (can be extended further)
    public int type;
    public int level;
    public bool isMenuOpened = false;

    // Upgrade logic
    public float upgradeTimer = 0f;
    private float _timerStep = 0.5f;


    private static int _crystalNeedForBuilding;
    private static int _ironNeedForBuilding;
    private static int _gelNeedForBuilding;

    private static int _crystalNeedForExpand_ToLvl2;
    private static int _ironNeedForForExpand_ToLvl2;
    private static int _gelNeedForForExpand_ToLvl2;

    private static int _crystalNeedForExpand_ToLvl3;
    private static int _ironNeedForForExpand_ToLvl3;
    private static int _gelNeedForForExpand_ToLvl3;

    private static int _maxHealth_Lvl1; 
    private static int _maxHealth_Lvl2; 
    private static int _maxHealth_Lvl3;

    private static int _maxShiled_Lvl1; 
    private static int _maxShiled_Lvl2; 
    private static int _maxShiled_Lvl3;

    private static int _defensePoints_Lvl1; 
    private static int _defensePoints_Lvl2; 
    private static int _defensePoints_Lvl3;

    private static int _baseUpgradeStep;



    public static void InitAllStaticFields()
    {
        _crystalNeedForBuilding = 5;
        _ironNeedForBuilding = 5;
        _gelNeedForBuilding = 5;

        _crystalNeedForExpand_ToLvl2 = 10;
        _ironNeedForForExpand_ToLvl2 = 10;
        _gelNeedForForExpand_ToLvl2 = 10;

        _crystalNeedForExpand_ToLvl3 = 15;
        _ironNeedForForExpand_ToLvl3 = 15;
        _gelNeedForForExpand_ToLvl3 = 15;

        _maxHealth_Lvl1 = 100; 
        _maxHealth_Lvl2 = 200; 
        _maxHealth_Lvl3 = 300;

        _maxShiled_Lvl1 = 100; 
        _maxShiled_Lvl2 = 200; 
        _maxShiled_Lvl3 = 300;

        _defensePoints_Lvl1 = 10; 
        _defensePoints_Lvl2 = 12; 
        _defensePoints_Lvl3 = 15;

        _baseUpgradeStep = 30;
    }


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

    public void GetResourcesNeedToExpand(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        if (level == 1)
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
        shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl2.ToString() + " " + _ironNeedForForExpand_ToLvl2.ToString() +" "+_gelNeedForForExpand_ToLvl2.ToString();
    }

    public static void InitCost_ToLvl3()
    {
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

            // Reload fill circles
            if (isMenuOpened)
            {
                switch(level)
                {
                    case 1:
                    shaftMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    shaftMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.LogError("Error!   Unknown uprading circle!");
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
            Debug.LogError("ERROR! - Invalid shaft level!!!!!");
        }

        if (isMenuOpened)
        {
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

            shaftMenuReference.ReloadShaftLevelVisuals(); // update buttons and vizuals
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


    public void InitStaticsLevel_1()
    {
        level = 1;
        capacity = 3; 

        healthPoints = _maxHealth_Lvl1;
        maxCurrentHealthPoints = _maxHealth_Lvl1;

        shieldPoints = _maxShiled_Lvl1;
        maxCurrentShieldPoints = _maxShiled_Lvl1;

        deffencePoints = _defensePoints_Lvl1;
    }

    public void InitStaticsLevel_2()
    {
        level = 2;
        capacity = 5; 

        healthPoints = (_maxHealth_Lvl2 * healthPoints) / _maxHealth_Lvl1;
        maxCurrentHealthPoints = _maxHealth_Lvl2;

        shieldPoints = (_maxShiled_Lvl2 * shieldPoints) / _maxShiled_Lvl1;
        maxCurrentShieldPoints = _maxShiled_Lvl2;

        deffencePoints = _defensePoints_Lvl2;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            shaftMenuReference.ReloadSlidersHP_SP();
        }

        GameViewMenu.Instance.ReloadShaftHP_SPAfterDamage(this);
    }

    public void InitStaticsLevel_3()
    {
        level = 3;
        capacity = 7; 

        healthPoints = (_maxHealth_Lvl3 * healthPoints) / _maxHealth_Lvl2;
        maxCurrentHealthPoints = _maxHealth_Lvl3;

        shieldPoints = (_maxShiled_Lvl3 * shieldPoints) / _maxShiled_Lvl2;
        maxCurrentShieldPoints = _maxShiled_Lvl3;

        deffencePoints = _defensePoints_Lvl3;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            shaftMenuReference.ReloadSlidersHP_SP();
        }

        GameViewMenu.Instance.ReloadShaftHP_SPAfterDamage(this);
    }


    public static void UpgradeStatisticsAfterBaseUpgrade()
    {
        _maxHealth_Lvl1 += _baseUpgradeStep;
        _maxHealth_Lvl2 += _baseUpgradeStep;
        _maxHealth_Lvl3 += _baseUpgradeStep;

        _maxShiled_Lvl1 += _baseUpgradeStep;
        _maxShiled_Lvl2 += _baseUpgradeStep;
        _maxShiled_Lvl3 += _baseUpgradeStep;
    }

    public void InitStatisticsAfterBaseUpgrade()
    {
        switch (level)
        {
            case 1:
            healthPoints = ((_maxHealth_Lvl1 + _baseUpgradeStep) * healthPoints) / _maxHealth_Lvl1;
            maxCurrentHealthPoints = (_maxHealth_Lvl1 + _baseUpgradeStep);

            shieldPoints = ((_maxShiled_Lvl1 + _baseUpgradeStep) * shieldPoints) / _maxShiled_Lvl1;
            maxCurrentShieldPoints = (_maxShiled_Lvl1 + _baseUpgradeStep);

            deffencePoints = _defensePoints_Lvl1; // not changing at all
            break;

            case 2:
            healthPoints = ((_maxHealth_Lvl2 + _baseUpgradeStep) * healthPoints) / _maxHealth_Lvl2;
            maxCurrentHealthPoints = (_maxHealth_Lvl2 + _baseUpgradeStep);

            shieldPoints = ((_maxShiled_Lvl2 + _baseUpgradeStep) * shieldPoints) / _maxShiled_Lvl2;
            maxCurrentShieldPoints = (_maxShiled_Lvl2 + _baseUpgradeStep);

            deffencePoints = _defensePoints_Lvl2; // not changing at all
            break;

            case 3:
            healthPoints = ((_maxHealth_Lvl3 + _baseUpgradeStep) * healthPoints) / _maxHealth_Lvl3;
            maxCurrentHealthPoints = (_maxHealth_Lvl3 + _baseUpgradeStep);

            shieldPoints = ((_maxShiled_Lvl3 + _baseUpgradeStep) * shieldPoints) / _maxShiled_Lvl3;
            maxCurrentShieldPoints = (_maxShiled_Lvl3 + _baseUpgradeStep);

            deffencePoints = _defensePoints_Lvl3; // not changing at all
            break;
        }
        
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
            dispenser = gameObject.transform.GetChild(0).gameObject;

            dispenser.tag = TagConstants.shaftDispenserTag;
            dispenser.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
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