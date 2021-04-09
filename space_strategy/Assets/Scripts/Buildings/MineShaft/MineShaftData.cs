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



















}