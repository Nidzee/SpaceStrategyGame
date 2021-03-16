using UnityEngine;
using System.Collections.Generic;

public class GarageSavingData
{
    public GameObject _tileOccupied;              // Reference to real MapTile on which building is set
    public GameObject _tileOccupied1;             // Reference to real MapTile on which building is set
    public List<Unit> _garageMembers;             // Units that are living here    
    public float _timerForCreatingUnit;
    public int _queue;                              
    public int _clicks;
    public int _numberOfUnitsToCome;

    public int healthPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int shieldPoints;            
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn = false;
    public int shieldGeneratorInfluencers = 0;
}