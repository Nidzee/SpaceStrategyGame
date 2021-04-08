public class ShieldGeneratorSavingData
{
    public string name;

    public string _tileOccupied_name;        // Reference to real MapTile on which building is set
    public string _tileOccupied1_name;       // Reference to real MapTile on which building is set
    public string _tileOccupied2_name;       // Reference to real MapTile on which building is set

    public int rotation;
    public int level;
    public int shield_state;
    public float upgradeTimer;

    public float shieldScale_x;
    public float shieldScale_y;
    public float shieldScale_z;


    public int healthPoints;
    public int shieldPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn;
    public int shieldGeneratorInfluencers;
}
