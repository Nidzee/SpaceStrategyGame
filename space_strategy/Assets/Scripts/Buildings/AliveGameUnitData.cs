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


    public void TurnShieldOn()
    {
        isShieldOn = true;
        deffencePoints += 5;
    }

    public void TurnShieldOff()
    {
        isShieldOn = false;
        deffencePoints -= 5;
    }
}
