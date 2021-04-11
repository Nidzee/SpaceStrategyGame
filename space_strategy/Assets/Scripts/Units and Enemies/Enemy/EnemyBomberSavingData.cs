public class EnemyBomberSavingData
{
    public string name;

    public int healthPoints;
    public int shieldPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation
    public int maxCurrentShieldPoints;  // For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn;
    public int shieldGeneratorInfluencers;  

    public int currentStateID;

    public float pos_x;
    public float pos_y;
    public float pos_z;
}
