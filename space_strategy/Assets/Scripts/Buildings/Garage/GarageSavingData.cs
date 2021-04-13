public class GarageSavingData
{
    public string name;
    
    // Garage saving data
    public int[] _garageMembersIDs;             // Units that are living here    
    public string _tileOccupied_name;              // Reference to real MapTile on which building is set
    public string _tileOccupied1_name;             // Reference to real MapTile on which building is set
    public float _timerForCreatingUnit;
    public int _queue;                              
    public int _clicksOnCreateUnitButton;
    public int rotation;


    // Aliva game unit data
    public int healthPoints;
    public int shieldPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn;
    public int shieldGeneratorInfluencers;
}