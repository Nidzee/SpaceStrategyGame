using UnityEngine;
using System.Collections.Generic;

public class GarageSavingData
{
    public int ID;
    public int[] _garageMembersIDs;             // Units that are living here    

    public string _tileOccupiedName;              // Reference to real MapTile on which building is set
    public string _tileOccupied1Name;             // Reference to real MapTile on which building is set
    // public List<Unit> _garageMembers;             // Units that are living here    
    public float _timerForCreatingUnit;
    public int _queue;                              
    public int _clicks;
    public int _numberOfUnitsToCome;

    public int healthPoints;
    public int shieldPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn;
    public int shieldGeneratorInfluencers = 0;
    
    public int rotation;
    public string positionTileName;
}