public class GarageSavingData
{
    public int ID;
    public string name;
    public int[] _garageMembersIDs;             // Units that are living here    
    public string _tileOccupied_name;              // Reference to real MapTile on which building is set
    public string _tileOccupied1_name;             // Reference to real MapTile on which building is set
    public float _timerForCreatingUnit;
    public int _queue;                              
    public int _clicks;
    public int _numberOfUnitsToCome;
    public int rotation;


    
    public int healthPoints;
    public int shieldPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn;
    public int shieldGeneratorInfluencers;
}