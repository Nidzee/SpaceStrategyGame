using System.Collections.Generic;
using UnityEngine;

public class MineShaftSavingData
{
    public GameObject _tileOccupied;           // Reference to real MapTile on which building is set
    public GameObject _tileOccupied1;          // Reference to real MapTile on which building is set

    public List<Unit> unitsWorkers;// List of Units that are working on this shaft

    public int capacity;
    public int type;
    public int level;

    public float upgradeTimer;
}
