public class GameUnit
{
    public string name;

    public int healthPoints = 100;
    public int maxCurrentHealthPoints = 100;  // For correct percentage recalculation

    public int shieldPoints = 100;            
    public int maxCurrentShieldPoints = 100;  // For correct percentage recalculation

    public int deffencePoints = 10;

    public bool isShieldOn = false;
    public int shieldGeneratorInfluencers = 0;


    public GameUnit(int _maxHealth, int _maxShield, int _maxDefensePoints)
    {
        healthPoints = _maxHealth;
        maxCurrentHealthPoints = _maxHealth;

        shieldPoints = _maxShield;
        maxCurrentShieldPoints = _maxShield;

        deffencePoints = _maxDefensePoints;

        isShieldOn = false;
        shieldGeneratorInfluencers = 0;
    }

    public void UpgradeStats(int newHealth, int newShield, int newDefense)
    {
        healthPoints = (newHealth * healthPoints) / maxCurrentHealthPoints;
        maxCurrentHealthPoints = newHealth;

        shieldPoints = (newHealth * shieldPoints) / maxCurrentShieldPoints;
        maxCurrentShieldPoints = newShield;

        deffencePoints = newDefense;
    }


    public bool TakeDamage(int damagePoints)
    {
        healthPoints -= damagePoints;

        if (healthPoints > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
