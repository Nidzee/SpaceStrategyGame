using UnityEngine;

public class AliveGameUnitData
{
    public int healthPoints;
    public int maxCurrentHealthPoints;  // For correct percentage recalculation

    public int shieldPoints;            
    public int maxCurrentShieldPoints;  // For correct percentage recalculation

    public int deffencePoints;

    public bool isShieldOn = false;
    public int shieldGeneratorInfluencers = 0;

    
    // private void InitStatsAfterConstructing()
    // {
    //     healthPoints = StatsManager._maxHealth_Garage;
    //     maxCurrentHealthPoints = StatsManager._maxHealth_Garage;

    //     shieldPoints = StatsManager._maxShiled_Garage;
    //     maxCurrentShieldPoints = StatsManager._maxShiled_Garage;

    //     deffencePoints = StatsManager._maxDeffensePoints_Garage;
    // }
}
