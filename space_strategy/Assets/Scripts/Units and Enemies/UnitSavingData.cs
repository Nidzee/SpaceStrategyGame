public class UnitSavingData
{
    public int ID;
    public string name;

    public float position_x;
    public float position_y;
    public float position_z;

    public float destination_x;
    public float destination_y;
    public float destination_z;

    public string targetObjectTransformName;

    
    public float resourcePosition_x;
    public float resourcePosition_y;
    public float resourcePosition_z;
    public int resourceType;


    // public bool isApproachShaft = false;
    // public bool isApproachStorage = false;
    // public bool isApproachHome = false;
    public bool isGatheringComplete = false;

    public int currentStateID;


    public int healthPoints;
    public int shieldPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn;
    public int shieldGeneratorInfluencers;
}

enum UnitStates
{
    UnitIdleState = 0,
    UnitMovingState = 1,
    UnitExtractingState = 2,
    UnitResourceLeavingState = 3,
    UnitNoSignalState = 4
}