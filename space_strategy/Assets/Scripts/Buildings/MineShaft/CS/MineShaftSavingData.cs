using System.Collections.Generic;
using UnityEngine;

public class MineShaftSavingData
{
    public int ID;
    public int[] _unitsWorkersIDs;             // Units that are living here    

    public string _tileOccupiedName;           // Reference to real MapTile on which building is set
    public string _tileOccupied1Name;          // Reference to real MapTile on which building is set

    public List<Unit> unitsWorkers;// List of Units that are working on this shaft

    public int capacity;
    public int type;
    public int level;

    public float upgradeTimer;

    
    public int healthPoints;
    public int shieldPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn;
    public int shieldGeneratorInfluencers = 0;
    

    public Vector3 position;
    public int rotation;
}
