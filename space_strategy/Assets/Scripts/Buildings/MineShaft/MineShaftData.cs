using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MineShaftData
{
    public int ID;
    public int[] _shaftWorkersIDs;             // Units that are living here  

    public int rotation;  

    public Unit _workerRef;                    // Reference for existing Unit object - for algorithm calculations
    public GameObject dispenser;               // Position of helper game object (for Unit FSM transitions)
    public bool isMenuOpened;

    public GameObject _tileOccupied;           // Reference to real MapTile on which building is set
    public GameObject _tileOccupied1;          // Reference to real MapTile on which building is set

    public List<Unit> unitsWorkers;            // List of Units that are working on this shaft

    public int capacity;
    public int type;
    public int level;

    public float upgradeTimer;



    public MineShaft _myMineShaft;



    public void InitMineShaftDataFromFile(MineShaftSavingData mineShaftSavingData)
    {
        HelperObjectInit();

        ID = mineShaftSavingData.ID;

        _tileOccupied = GameObject.Find(mineShaftSavingData._tileOccupiedName);
        if(mineShaftSavingData._tileOccupied1Name != "")
        {
            _tileOccupied1 = GameObject.Find(mineShaftSavingData._tileOccupied1Name);
        }

        _shaftWorkersIDs = mineShaftSavingData._shaftWorkersIDs;

        capacity = mineShaftSavingData.capacity;
        level = mineShaftSavingData.level;
        type = mineShaftSavingData.type;

        upgradeTimer = mineShaftSavingData.upgradeTimer;
    }


    public MineShaftData(MineShaft thisShaft)
    {
        _myMineShaft = thisShaft;

        _workerRef = null;                    // Reference for existing Unit object - for algorithm calculations
        isMenuOpened = false;

        dispenser = null;               // Position of helper game object (for Unit FSM transitions)

        _tileOccupied = null;           // Reference to real MapTile on which building is set
        _tileOccupied1 = null;          // Reference to real MapTile on which building is set

        unitsWorkers = new List<Unit>();            // List of Units that are working on this shaft

        upgradeTimer = 0f;
    }

    public Transform GetUnitDestination()
    {
        return dispenser.transform;
    }





















    public IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += MineShaftStaticData._timerStep * Time.deltaTime;

            // Reload fill circles
            if (isMenuOpened)
            {
                switch(level)
                {
                    case 1:
                    MineShaftStaticData.shaftMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    MineShaftStaticData.shaftMenuReference.level3.fillAmount = upgradeTimer;
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
        upgradeTimer = 0f; // Reset timer

        if (level == 1)
        {
            _myMineShaft.UpgradeToLvl2();
        }
        else if (level == 2)
        {
            _myMineShaft.UpgradeToLvl3();
        }
        else
        {
            Debug.LogError("ERROR! - Invalid shaft level!!!!!");
        }


        // Update UI
        // GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterShaftExpand(thisShaft);
    }





    public void AddWorkerViaSlider() // Reload slider here because they are involved in process
    {
        _workerRef = ResourceManager.Instance.SetAvaliableUnitToWork(_workerRef); // Initialize adding unit reference

        if (!_workerRef)
        {
            Debug.Log("No Units avaliable!");
            return;
        }
        
        _workerRef.WorkPlace = _myMineShaft;
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

    public void RemoveUnit(Unit unit)
    {
        unit.WorkPlace = null;     // Set workplace - null
        unitsWorkers.Remove(unit); // Remove from workers list
    }
















    public void ConstructBuilding(Model model)
    {
        level = 1;
        capacity = 3;

        rotation = model.rotation;

        // HelperObjectInit();
    }

    public void PlaceBuilding(Model model)
    {
        switch(type)
        {
            case 1:
            _tileOccupied = model.BTileZero;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            break;

            case 2:
            _tileOccupied = model.BTileZero;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            break;

            case 3:
            _tileOccupied = model.BTileZero;
            _tileOccupied1 = model.BTileOne;
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
            break;
        }
    }

    public void DestroyBuilding()
    {
        if (isMenuOpened)
        {
            MineShaftStaticData.shaftMenuReference.ExitMenu();
        }

        foreach (var unit in unitsWorkers)
        {




            unit.WorkPlace = null;
            ResourceManager.Instance.avaliableUnits.Add(unit);





        }

        unitsWorkers.Clear();


        switch (type)
        {
            case 1: // Crystal
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS1_crystal;
            break;

            case 2: // Iron
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS2_iron;
            break;

            case 3: // Gel
            _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.RS3_gel;
            _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
            break;
        }
    }







    public void UpgradeToLvl2()
    {
        level = 2;
        capacity = 5; 
    }

    public void UpgradeToLvl3()
    {
        level = 3;
        capacity = 7; 
    }

    public void HelperObjectInit()
    {
        if (_myMineShaft.gameObject.transform.childCount != 0)
        {
            dispenser = _myMineShaft.gameObject.transform.GetChild(0).gameObject;

            dispenser.tag = TagConstants.shaftDispenserTag;
            dispenser.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        }

        else
        {
            Debug.LogError("ERROR!       No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    // Can be extracted to Menu
    public void Invoke()
    {
        if (level == 1)
        {
            StatsManager.InitCost_ToLvl2();
        }
        else if (level == 2)
        {
            StatsManager.InitCost_ToLvl3();
        }
        else
        {
            MineShaftStaticData.shaftMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
        }
    }


    public void UpdateUI()
    {
        if (isMenuOpened)
        {
            Invoke();
        }
    }

    public void UpgradeStatsAfterShtabUpgrade()
    {
        int newHealth = 0;
        int newShield = 0;
        int newDefense = 0;
        
        switch (level)
        {
            case 1:
            newHealth = StatsManager._maxHealth_Lvl1_Shaft + StatsManager._baseUpgradeStep_Shaft;
            newShield = StatsManager._maxShiled_Lvl1_Shaft + StatsManager._baseUpgradeStep_Shaft;
            newDefense = StatsManager._defensePoints_Lvl1_Shaft;
            break;

            case 2:
            newHealth = StatsManager._maxHealth_Lvl2_Shaft + StatsManager._baseUpgradeStep_Shaft;
            newShield = StatsManager._maxShiled_Lvl2_Shaft + StatsManager._baseUpgradeStep_Shaft;
            newDefense = StatsManager._defensePoints_Lvl2_Shaft;
            break;

            case 3:
            newHealth = StatsManager._maxHealth_Lvl3_Shaft + StatsManager._baseUpgradeStep_Shaft;
            newShield = StatsManager._maxShiled_Lvl3_Shaft + StatsManager._baseUpgradeStep_Shaft;
            newDefense = StatsManager._defensePoints_Lvl3_Shaft;
            break;
        }

        _myMineShaft.UpgradeStats(newHealth, newShield, newDefense);
    }

    public void GetResourcesNeedToExpand(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        if (level == 1)
        {
            crystalNeed = StatsManager._crystalNeedForExpand_ToLvl2_Shaft;
            ironNeed = StatsManager._ironNeedForForExpand_ToLvl2_Shaft;
            gelNeed = StatsManager._gelNeedForForExpand_ToLvl2_Shaft;
        }

        else
        {
            crystalNeed = StatsManager._crystalNeedForExpand_ToLvl3_Shaft;
            ironNeed = StatsManager._ironNeedForForExpand_ToLvl3_Shaft;
            gelNeed = StatsManager._gelNeedForForExpand_ToLvl3_Shaft;
        }
    }
}